using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Intellisence
{ 
    [Export(typeof(ICompletionSourceProvider))]

    [ContentType("text")]
    [Name("UeqtDynamicCompletion")]
    internal class IntellisenceDynamicCompletionSourceProvider : ICompletionSourceProvider
    {
        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new IntellisenceDynamicCompletionSource(this, textBuffer);
        }
        [Import]
        internal ITextStructureNavigatorSelectorService NavigatorService { get; set; }
        [Import]
        internal IGlyphService GlyphService { get; set; }



    }
}
