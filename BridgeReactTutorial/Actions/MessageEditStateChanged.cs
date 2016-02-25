using System;
using Bridge.React;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.Actions
{
	public class MessageEditStateChanged : IDispatcherAction
	{
		public MessageEditStateChanged(MessageEditState newState)
		{
			if (newState == null)
				throw new ArgumentNullException("newState");

			NewState = newState;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public MessageEditState NewState { get; private set; }
	}
}
