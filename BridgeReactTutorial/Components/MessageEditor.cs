using System;
using Bridge.React;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.Components
{
	public class MessageEditor : StatelessComponent<MessageEditor.Props>
	{
		public MessageEditor(Props props) : base(props) { }

		public override ReactElement Render()
		{
			var formIsInvalid = 
				!string.IsNullOrWhiteSpace(props.Message.Title.ValidationError) ||
				!string.IsNullOrWhiteSpace(props.Message.Content.ValidationError);
			var isSaveDisabled = formIsInvalid || props.Message.IsSaveInProgress;
			return DOM.FieldSet(new FieldSetAttributes { ClassName = props.ClassName },
				DOM.Legend(null, props.Message.Caption),
				DOM.Span(new Attributes { ClassName = "label" }, "Title"),
				new ValidatedTextInput(new ValidatedTextInput.Props
				{
					ClassName = "title",
					Disabled = props.Message.IsSaveInProgress,
					Content = props.Message.Title.Text,
					ValidationMessage = props.Message.Title.ValidationError,
					OnChange = ChangeMessageTitle
				}),
				DOM.Span(new Attributes { ClassName = "label" }, "Content"),
				new ValidatedTextInput(new ValidatedTextInput.Props
				{
					ClassName = "content",
					Disabled = props.Message.IsSaveInProgress,
					Content = props.Message.Content.Text,
					ValidationMessage = props.Message.Content.ValidationError,
					OnChange = ChangeMessageContent
				}),
				DOM.Button(
					new ButtonAttributes { Disabled = isSaveDisabled, OnClick = e => props.OnSave() },
					"Save"
				)
			);
		}

		private void ChangeMessageTitle(string newTitle)
		{
			props.OnChange(new MessageEditState
			{
				Caption = props.Message.Caption,
				Title = new TextEditState { Text = newTitle },
				Content = props.Message.Content,
				IsSaveInProgress = props.Message.IsSaveInProgress
			});
		}

		private void ChangeMessageContent(string newContent)
		{
			props.OnChange(new MessageEditState
			{
				Caption = props.Message.Caption,
				Title = props.Message.Title,
				Content = new TextEditState { Text = newContent },
				IsSaveInProgress = props.Message.IsSaveInProgress
			});
		}

		public class Props
		{
			public string ClassName;
			public MessageEditState Message;
			public Action<MessageEditState> OnChange;
			public Action OnSave;
		}
	}
}
