using Bridge.React;
using BridgeReactTutorial.API;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Actions
{
	public class MessageSaveRequested : IDispatcherAction, IAmImmutable
	{
		public MessageSaveRequested(MessageDetails message)
		{
			this.CtorSet(_ => _.Message, message);
		}
		public MessageDetails Message { get; private set; }
	}
}
