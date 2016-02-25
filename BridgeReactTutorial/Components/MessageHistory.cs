using System.Linq;
using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Components
{
	public class MessageHistory : StatelessComponent<MessageHistory.Props>
	{
		public MessageHistory(Props props) : base(props) { }

		public override ReactElement Render()
		{
			var className = props.ClassName;
			if (!props.Messages.Any())
				className = (className + " zero-messages").Trim();

			var messageElements = props.Messages
				.Select(savedMessage => DOM.Div(new Attributes { Key = savedMessage.Id.ToString(), ClassName = "historical-message" },
					DOM.Span(new Attributes { ClassName = "title" }, savedMessage.Message.Title.Value),
					DOM.Span(new Attributes { ClassName = "content" }, savedMessage.Message.Content.Value)
				));

			return DOM.FieldSet(new FieldSetAttributes { ClassName = className },
				DOM.Legend(null, "Message History"),
				DOM.Div(null, messageElements.ToChildComponentArray())
			);
		}

		public class Props
		{
			public string ClassName;
			public Set<SavedMessageDetails> Messages;
		}
	}
}
