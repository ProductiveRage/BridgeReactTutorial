using System;
using Bridge.React;
using BridgeReactTutorial.API;

namespace BridgeReactTutorial.Actions
{
	public class MessageSaveRequested : IDispatcherAction
	{
		public MessageSaveRequested(MessageDetails message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			Message = message;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public MessageDetails Message { get; private set; }
	}
}
