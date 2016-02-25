using System;
using Bridge.React;
using BridgeReactTutorial.API;

namespace BridgeReactTutorial.Actions
{
	public class MessageSaveSucceeded : IDispatcherAction
	{
		public MessageSaveSucceeded(RequestId requestId)
		{
			if (requestId == null)
				throw new ArgumentNullException("requestId");

			RequestId = requestId;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public RequestId RequestId { get; private set; }
	}
}
