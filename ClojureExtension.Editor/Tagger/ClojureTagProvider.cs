﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using ClojureExtension.Parsing;
using Microsoft.ClojureExtension.Editor.Parsing;
using Microsoft.ClojureExtension.Utilities;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Microsoft.ClojureExtension.Editor.Tagger
{
	[Export(typeof (IViewTaggerProvider))]
	[ContentType("Clojure")]
	[TagType(typeof (ClojureTokenTag))]
	public class ClojureTagProvider : IViewTaggerProvider
	{
		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
		{
			Entity<LinkedList<Token>> tokenizedBuffer = TokenizedBufferBuilder.TokenizedBuffers[buffer];
			ClojureTokenTagger tagger = new ClojureTokenTagger(buffer, tokenizedBuffer);
			BufferTextChangeHandler textChangeHandler = new BufferTextChangeHandler(new TextBufferAdapter(buffer), tokenizedBuffer);
			TextChangeAdapter textChangeAdapter = new TextChangeAdapter(textChangeHandler);
			buffer.Changed += textChangeAdapter.OnTextChange;
			textChangeHandler.TokenChanged += tagger.OnTokenChange;
			return tagger as ITagger<T>;
		}
	}
}