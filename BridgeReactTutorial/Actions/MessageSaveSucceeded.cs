using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Actions
{
	public class MessageSaveSucceeded : IDispatcherAction, IAmImmutable
	{
		public MessageSaveSucceeded(RequestId requestId)
		{
			this.CtorSet(_ => _.RequestId, requestId);
		}
		public RequestId RequestId { get; private set; }
	}
}
