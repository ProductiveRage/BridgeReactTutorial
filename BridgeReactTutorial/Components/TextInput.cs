using System;
using Bridge.Html5;
using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Components
{
	public class TextInput : StatelessComponent<TextInput.Props>
	{
		public TextInput(Props props) : base(props) { }

		public override ReactElement Render()
		{
			return DOM.Input(new InputAttributes
			{
				Type = InputType.Text,
				ClassName = props.ClassName.IsDefined ? props.ClassName.Value : null,
				Disabled = props.Disabled,
				Value = props.Content,
				OnChange = e => props.OnChange(e.CurrentTarget.Value)
			});
		}

		public class Props : IAmImmutable
		{
			public Props(Optional<NonBlankTrimmedString> className, bool disabled, string content, Action<string> onChange)
			{
				this.CtorSet(_ => _.ClassName, className);
				this.CtorSet(_ => _.Disabled, disabled);
				this.CtorSet(_ => _.Content, content);
				this.CtorSet(_ => _.OnChange, onChange);
			}
			public Optional<NonBlankTrimmedString> ClassName { get; private set; }
			public bool Disabled { get; private set; }
			public string Content { get; private set; }
			public Action<string> OnChange { get; private set; }
		}
	}
}
