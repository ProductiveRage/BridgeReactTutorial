using System;
using System.Collections.Generic;
using Bridge.React;
using BridgeReactTutorial.API;

namespace BridgeReactTutorial.Actions
{
	public class MessageHistoryUpdated : IDispatcherAction
	{

		public MessageHistoryUpdated(RequestId requestId, IEnumerable<Tuple<int, MessageDetails>> messages)
		{
			if (requestId == null)
				throw new ArgumentNullException("requestId");
			if (messages == null)
				throw new ArgumentNullException("messages");

			RequestId = requestId;
			Messages = messages;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public RequestId RequestId { get; private set; }
		
		/// <summary>
		/// This will never be null
		/// </summary>
		public IEnumerable<Tuple<int, MessageDetails>> Messages { get; private set; }
	}
}
