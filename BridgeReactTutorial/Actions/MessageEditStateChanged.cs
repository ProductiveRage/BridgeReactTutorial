using Bridge.React;
using BridgeReactTutorial.ViewModels;
using ProductiveRage.Immutable;

namespace BridgeReactTutorial.Actions
{
	public class MessageEditStateChanged : IDispatcherAction, IAmImmutable
	{
		public MessageEditStateChanged(MessageEditState newState)
		{
			this.CtorSet(_ => _.NewState, newState);
		}
		public MessageEditState NewState { get; private set; }
	}
}
