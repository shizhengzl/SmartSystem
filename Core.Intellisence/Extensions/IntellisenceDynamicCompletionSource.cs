using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{
    internal class IntellisenceDynamicCompletionSource : ICompletionSource
    {

        public ITextBuffer m_textBuffer { get; set; }
        IntellisenceDynamicCompletionSourceProvider m_sourceProvider { get; set; }



        public IntellisenceDynamicCompletionSource(IntellisenceDynamicCompletionSourceProvider t, ITextBuffer tb)
        {
            m_textBuffer = tb;
            m_sourceProvider = t;
        }


        void ICompletionSource.AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (completionSets.Count != 0)
                return;

            string inputtext = session.TextView.Caret.Position.BufferPosition.GetContainingLine().GetText();
            var alltext = m_textBuffer.CurrentSnapshot.GetText();
            var inputlist = Core.UsuallyCommon.SearchExtensions.GetStringSingleColumn(inputtext);


            var starttext = inputlist.LastOrDefault();
            string lastChar = starttext.Substring(starttext.Length - 1, 1);
            starttext = starttext.Replace(lastChar, "").Trim();


            var list = DataServices.GetCompletionList(lastChar, starttext);
            var mCompList = new List<Completion>();
            foreach (var intellisences in list)
            {
                mCompList.Add(new Completion(
                 intellisences.DisplayText
               , intellisences.InsertionText
               , intellisences.Description
               , m_sourceProvider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphGroupProperty, StandardGlyphItem.GlyphItemPublic)
               , "72"));

            }
            mCompList.Add(new Completion(
            "不吃肉的狮子"
            , "不吃肉的狮子"
            , "不吃肉的狮子"
            , m_sourceProvider.GlyphService.GetGlyph(StandardGlyphGroup.GlyphExtensionMethod, StandardGlyphItem.GlyphItemPublic)
            , "72"));

            var set = new CompletionSet(
                  "moniker",//"施政的智能提示",
                  "施政的智能提示",
                  FindTokenSpanAtPosition(session.GetTriggerPoint(m_textBuffer), session),
                  mCompList,
                  null);
            completionSets.Add(set);
        }

        ITrackingSpan FindTokenSpanAtPosition(ITrackingPoint point, ICompletionSession session)
        {
            SnapshotPoint currentPoint = (session.TextView.Caret.Position.BufferPosition) - 1;
            ITextStructureNavigator navigator = m_sourceProvider.NavigatorService.GetTextStructureNavigator(m_textBuffer);
            TextExtent extent = navigator.GetExtentOfWord(currentPoint);
            return currentPoint.Snapshot.CreateTrackingSpan(extent.Span, SpanTrackingMode.EdgeInclusive);
        }

        public void Dispose()
        {

        }

    }
}
