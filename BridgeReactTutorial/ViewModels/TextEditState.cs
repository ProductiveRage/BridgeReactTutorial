﻿using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.ViewModels
{
	public class TextEditState : IAmImmutable
	{
		public TextEditState(string text) : this(text, Optional<NonBlankTrimmedString>.Missing) { }
		public TextEditState(string text, Optional<NonBlankTrimmedString> validationError)
		{
			this.CtorSet(_ => _.Text, text);
			this.CtorSet(_ => _.ValidationError, validationError);
		}
		public string Text { get; private set; }
		public Optional<NonBlankTrimmedString> ValidationError { get; private set; }
	}
}
