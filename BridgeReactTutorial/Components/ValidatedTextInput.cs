using Bridge.React;

namespace BridgeReactTutorial.Components
{
	public class ValidatedTextInput : StatelessComponent<ValidatedTextInput.Props>
	{
		public ValidatedTextInput(Props props) : base(props) { }

		public override ReactElement Render()
		{
			var className = props.ClassName;
			if (!string.IsNullOrWhiteSpace(props.ValidationMessage))
				className = (className + " invalid").Trim();

			return DOM.Span(new Attributes { ClassName = className },
				new TextInput(props),
				!string.IsNullOrWhiteSpace(props.ValidationMessage)
					? DOM.Span(new Attributes { ClassName = "validation-message" }, props.ValidationMessage)
					: null
			);
		}

		public class Props : TextInput.Props
		{
			public string ValidationMessage;
		}
	}
}
