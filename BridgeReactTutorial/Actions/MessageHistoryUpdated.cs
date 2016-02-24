using System;
using System.Collections.Generic;
using Bridge.React;
using BridgeReactTutorial.API;

namespace BridgeReactTutorial.Actions
{
	public class MessageHistoryUpdated : IDispatcherAction
	{
		public RequestId RequestId;
		public IEnumerable<Tuple<int, MessageDetails>> Messages;
	}
}
