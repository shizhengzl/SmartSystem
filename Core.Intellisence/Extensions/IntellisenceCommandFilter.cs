using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{
    internal sealed class IntellisenceCommandFilter: IOleCommandTarget
    {
        /// <summary>
        /// 当前会话
        /// </summary>
        ICompletionSession _CurrentSession;

        /// <summary>
        /// TextView(WPF)
        /// </summary>
        IWpfTextView _TextView { get; set; }

        /// <summary>
        /// 代理
        /// </summary>
        ICompletionBroker _Broker { get; set; }

        /// <summary>
        /// 执行由VMCommandFilter未执行完的命令
        /// </summary>
        public IOleCommandTarget _Next { get; set; }

        public IntellisenceCommandFilter(IWpfTextView textView, ICompletionBroker broker)
        {
            this._TextView = textView;
            this._Broker = broker;

            //this._Next = next;
        }

        /// <summary>
        /// 获取输入的字符
        /// </summary>
        /// <param name="pvaIn">输入指针</param>
        private char GetTypeChar(IntPtr pvaIn)
        {
            return (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            bool handled = false;
            int hresult = VSConstants.S_OK;

            // 1. Pre-process
            if (pguidCmdGroup == VSConstants.VSStd2K)
            {
                char typedChar = char.MinValue;
                //make sure the input is a char before getting it
                if (pguidCmdGroup == VSConstants.VSStd2K && nCmdID == (uint)VSConstants.VSStd2KCmdID.TYPECHAR)
                {
                    typedChar = (char)(ushort)Marshal.GetObjectForNativeVariant(pvaIn);
                }
                if ((VSConstants.VSStd2KCmdID)nCmdID == VSConstants.VSStd2KCmdID.AUTOCOMPLETE || (VSConstants.VSStd2KCmdID)nCmdID == VSConstants.VSStd2KCmdID.COMPLETEWORD)
                {
                    handled = true;// StartSession();
                }
                else if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN
                   || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB
                   || (char.IsWhiteSpace(typedChar) || char.IsPunctuation(typedChar)) && typedChar != '.')
                {
                    if (nCmdID == (uint)VSConstants.VSStd2KCmdID.RETURN
                   || nCmdID == (uint)VSConstants.VSStd2KCmdID.TAB)
                    {
                        if (_CurrentSession != null && _CurrentSession.SelectedCompletionSet != null)
                        {
                            // 自动完成后不输入回车和Tab
                            handled = true;
                        }
                    }
                    else
                    {
                        handled = false;
                    }

                    Complete(true);
                }
                else if ((VSConstants.VSStd2KCmdID)nCmdID == VSConstants.VSStd2KCmdID.CANCEL)
                {
                    handled = false;//Cancel();
                }
            }

            if (!handled)
                hresult = _Next.Exec(pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);

            if (ErrorHandler.Succeeded(hresult))
            {
                if (pguidCmdGroup == VSConstants.VSStd2K)
                {
                    switch ((VSConstants.VSStd2KCmdID)nCmdID)
                    {
                        case VSConstants.VSStd2KCmdID.TYPECHAR:
                            char ch = GetTypeChar(pvaIn);
                            //如果当前的会话已经被启动
                            if (_CurrentSession != null && _CurrentSession.IsStarted && ch != '.')
                            {
                                //执行过滤
                                _CurrentSession.SelectedCompletionSet.Filter();
                                //选择最佳匹配
                                _CurrentSession.SelectedCompletionSet.SelectBestMatch();
                                //重新计算CompletionSet
                                _CurrentSession.SelectedCompletionSet.Recalculate();
                            }

                            if (DataServices.IsHit(ch.ToString()) || ch == '.')
                            { 
                                //获取插入符号,也就是光标位置.
                                SnapshotPoint caret = _TextView.Caret.Position.BufferPosition;
                                //文本快照
                                ITextSnapshot snapShot = caret.Snapshot;
                                //在当前插入符号位置创建积极的。。跟踪点
                                ITrackingPoint trackingPoint = snapShot.CreateTrackingPoint(caret, PointTrackingMode.Positive);
                                //由代理创建Completion会话
                                _CurrentSession = _Broker.CreateCompletionSession(_TextView, trackingPoint, true); 
                                //添加放弃事件
                                _CurrentSession.Dismissed += (sender, args) => _CurrentSession = null;
                                //启动该会话. 
                                _CurrentSession.Start(); 
                            }
                            break;
                        case VSConstants.VSStd2KCmdID.BACKSPACE:
                            if (_CurrentSession != null && _CurrentSession.SelectedCompletionSet != null)
                            {
                                _CurrentSession.Filter();
                            }
                            break;
                    }
                }
            }

            return hresult;
        }

        /// <summary>
        /// 查询该对象以获得由用户界面事件生成的一个或多个命令的状态
        /// </summary>
        /// <param name="pguidCmdGroup">命令组的 GUID</param>
        /// <param name="cCmds">命令数</param>
        /// <param name="prgCmds">数组指示命令调用方需要状态信息的 OLECMD 结构</param>
        /// <param name="pCmdText">由OLECMDTEXT返回的单个命令的状态信息</param>
        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            return _Next.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        /// <summary>
        /// 确认完成或者丢弃
        /// </summary>
        private bool Complete(bool force)
        {
            if (_CurrentSession == null || !_CurrentSession.IsStarted)
                return false;

            //如果用户没有选择并且主动丢弃
            if (!_CurrentSession.SelectedCompletionSet.SelectionStatus.IsSelected && !force)
            {
                _CurrentSession.Dismiss();
                return false;
            }
            else
            {
                ITextEdit edit = _CurrentSession.TextView.TextBuffer.CreateEdit();
                ITextSnapshot snapshot = edit.Snapshot;


                string inputtext = _CurrentSession.TextView.Caret.Position.BufferPosition.GetContainingLine().GetText();
                 
                var inputlist = Core.UsuallyCommon.SearchExtensions.GetStringSingleColumn(inputtext); 
                var starttext = inputlist.LastOrDefault();
                string lastChar = starttext.Substring(starttext.Length - 1, 1); 
                starttext = starttext.Replace(lastChar, "").Trim();

                int position = (starttext.LastIndexOf(" ") > 0) ? (starttext.Length + 1 - starttext.LastIndexOf(" "))
                        : (starttext.LastIndexOf("\t") > 0 ? (starttext.Length + 1 - starttext.LastIndexOf("\t")) : starttext.Length + 1);

              
                edit.Delete(_CurrentSession.TextView.Caret.Position.BufferPosition.Position - position , position);

                string text = _CurrentSession.SelectedCompletionSet.SelectionStatus.Completion.InsertionText;

                edit.Insert(_CurrentSession.TextView.Caret.Position.BufferPosition.Position - position, text);
                 

                edit.Apply();
                if (_CurrentSession != null)
                    _CurrentSession.Dismiss(); 
                return true;
            }

        }
    }
}
