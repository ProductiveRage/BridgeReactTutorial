﻿using System.Linq;
using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Components
{
	public class MessageHistory : PureComponent<MessageHistory.Props>
	{
		public MessageHistory(NonNullList<SavedMessageDetails> messages, Optional<NonBlankTrimmedString> className = new Optional<NonBlankTrimmedString>())
			: base(new Props(className, messages)) { }

		public override ReactElement Render()
		{
			var className = props.ClassName.IsDefined ? props.ClassName.Value : "";
			if (!props.Messages.Any())
				className += (className == "" ? "" : " ") + "zero-messages";

			var messageElements = props.Messages
				.Select(savedMessage => DOM.Div(new Attributes { Key = savedMessage.Id.ToString(), ClassName = "historical-message" },
					DOM.Span(new Attributes { ClassName = "title" }, savedMessage.Message.Title.Value),
					DOM.Span(new Attributes { ClassName = "content" }, savedMessage.Message.Content.Value)
				));

			return DOM.FieldSet(new FieldSetAttributes { ClassName = (className == "" ? null : className) },
				DOM.Legend(null, "Message History"),
				DOM.Div(messageElements)
			);
		}

		public class Props : IAmImmutable
		{
			public Props(Optional<NonBlankTrimmedString> className, NonNullList<SavedMessageDetails> messages)
			{
				this.CtorSet(_ => _.ClassName, className);
				this.CtorSet(_ => _.Messages, messages);
			}
			public Optional<NonBlankTrimmedString> ClassName { get; private set; }
			public NonNullList<SavedMessageDetails> Messages { get; private set; }
		}
	}
}
