using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Actions
{
	public class MessageHistoryUpdated : IDispatcherAction, IAmImmutable
	{

		public MessageHistoryUpdated(RequestId requestId, NonNullList<SavedMessageDetails> messages)
		{
			this.CtorSet(_ => _.RequestId, requestId);
			this.CtorSet(_ => _.Messages, messages);
		}
		public RequestId RequestId { get; private set; }
		public NonNullList<SavedMessageDetails> Messages { get; private set; }
	}
}
