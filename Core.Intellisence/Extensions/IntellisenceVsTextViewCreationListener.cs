using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{
    [Export(typeof(IVsTextViewCreationListener))]
    [Name("IntellisenceCommandFilter completion handler")]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal sealed class IntellisenceVsTextViewCreationListener : IVsTextViewCreationListener
    {
        [Import]
        IVsEditorAdaptersFactoryService AdaptersFactory = null;
        [Import]
        ICompletionBroker CompletionBroker = null;

        [Import]
        internal SVsServiceProvider ServiceProvider = null; 

        [Import]
        internal IEditorOperationsFactoryService EditorOperationsFactoryService
        {
            get;
            set;
        }

        public void VsTextViewCreated(IVsTextView textViewAdapter)
        {
            IWpfTextView view = AdaptersFactory.GetWpfTextView(textViewAdapter);
            if (view == null) return;

            IntellisenceCommandFilter filter = new IntellisenceCommandFilter(view, CompletionBroker);
            IOleCommandTarget next;
            textViewAdapter.AddCommandFilter(filter, out next);
            filter._Next = next; 
        }
    }
}
