using System;
using System.Collections.Generic;
using Bridge.React;
using BridgeReactTutorial.Actions;
using BridgeReactTutorial.API;
using BridgeReactTutorial.Stores;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.Components
{
	public class AppContainer : Component<AppContainer.Props, AppContainer.State>
	{
		public AppContainer(AppContainer.Props props) : base(props) { }

		protected override void ComponentDidMount()
		{
			props.Store.Change += StoreChanged;
		}
		protected override void ComponentWillUnmount()
		{
			props.Store.Change -= StoreChanged;
		}
		private void StoreChanged()
		{
			SetState(new State
			{
				NewMessage = props.Store.NewMessage,
				MessageHistory = props.Store.MessageHistory
			});
		}

		public override ReactElement Render()
		{
			// If state is null then the Store has not been initialised and its OnChange event has not been called - in this case, we are not ready to render
			// anything and so should return null here
			if (state == null)
				return null;

			// A good guideline to follow with stateful components is that the State reference should contain everything required to draw the components and
			// props should only be used to access a Dispatcher reference to deal with callbacks from those components
			return DOM.Div(null,
				new MessageEditor(new MessageEditor.Props
				{
					ClassName = "message",
					Message = state.NewMessage,
					OnChange = newState => props.Dispatcher.HandleViewAction(new MessageEditStateChanged { NewState = newState }),
					OnSave = () =>
					{
						// No validation is required here since the MessageEditor shouldn't let OnSave be called if the current message state is invalid
						// (ie. if either field has a ValidationMessage). In some applications, it is preferred that validation messages not be shown
						// until a save request is attempted (in which case some additional validation WOULD be performed here), but this app keeps
						// things simpler by showing validation messages for all inputs until they have acceptable values (meaning that the first
						// time the form is draw, it has validation messages displayed even though the user hasn't interacted with it yet).
						props.Dispatcher.HandleViewAction(new MessageSaveRequested
						{
							Message = new MessageDetails { Title = state.NewMessage.Title.Text, Content = state.NewMessage.Content.Text }
						});
					}
				}),
				new MessageHistory(new MessageHistory.Props { ClassName = "history", Messages = state.MessageHistory })
			);
		}

		public sealed class Props
		{
			public AppUIStore Store;
			public AppDispatcher Dispatcher;
		}

		public class State
		{
			public MessageEditState NewMessage;
			public IEnumerable<Tuple<int, MessageDetails>> MessageHistory;
		}
	}
}
