using System;
using System.Collections.Generic;
using Bridge.React;
using BridgeReactTutorial.Actions;
using BridgeReactTutorial.API;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.Stores
{
	public class AppUIStore
	{
		private RequestId _saveActionRequestId;
		public AppUIStore(AppDispatcher dispatcher, IReadAndWriteMessages messageApi)
		{
			if (dispatcher == null)
				throw new ArgumentNullException("dispatcher");
			if (messageApi == null)
				throw new ArgumentNullException("messageApi");

			NewMessage = GetEmptyNewMessage();
			MessageHistory = new Tuple<int, MessageDetails>[0];

			_saveActionRequestId = null;

			dispatcher.Register(message =>
			{
				message
					.If<StoreInitialised>(action =>
					{
						// When it's time for a Store to be initialised (to set its initial state and call OnChange to let any interested Components know
						// that it's ready), a StoreInitialised action will be dispatched that references the Store. In a more complicated app, a router
						// might choose an initial Store based upon the current URL. (We don't need to do anything within this callback, we just need to
						// match the StoreInitialised so that IfAnyMatched will fire and call OnChange).
					})
					.Else<MessageEditStateChanged>(action =>
					{
						var newState = action.NewState;
						UpdateValidationFor(newState);
						NewMessage = newState;
					})
					.Else<MessageSaveRequested>(action =>
					{
						NewMessage.IsSaveInProgress = true;
						_saveActionRequestId = messageApi.SaveMessage(action.Message);
					})
					.Else<MessageSaveSucceeded>(
						condition: action => action.RequestId == _saveActionRequestId,
						work: action =>
						{
							// The API's SaveMessage function will fire a MessageSaveSucceeded action when (if) the save is successful and then a subsequent
							// MessageHistoryUpdated action after it's automatically retrieved fresh data, including the newly-saved item (so we need only
							// reset the form here, a MessageHistoryUpdated should be along shortly containig the new item..) 
							_saveActionRequestId = null;
							NewMessage = GetEmptyNewMessage();
						}
					)
					.Else<MessageHistoryUpdated>(action => MessageHistory = action.Messages)
					.IfAnyMatched(OnChange);
			});
		}

		public MessageEditState NewMessage;
		public IEnumerable<Tuple<int, MessageDetails>> MessageHistory;

		public event Action Change;

		private void UpdateValidationFor(MessageEditState messageEditState)
		{
			if (messageEditState == null)
				throw new ArgumentNullException("messageEditState");

			messageEditState.Caption = string.IsNullOrWhiteSpace(messageEditState.Title.Text) ? "Untitled" : messageEditState.Title.Text;
			messageEditState.Title.ValidationError = string.IsNullOrWhiteSpace(messageEditState.Title.Text) ? "Must enter a title" : "";
			messageEditState.Content.ValidationError = string.IsNullOrWhiteSpace(messageEditState.Content.Text) ? "Must enter message content" : "";
		}

		private MessageEditState GetEmptyNewMessage()
		{
			var initialState = new MessageEditState
			{
				Caption = "Untitled",
				Title = new TextEditState { Text = "", ValidationError = "" },
				Content = new TextEditState { Text = "", ValidationError = "" },
				IsSaveInProgress = false
			};
			UpdateValidationFor(initialState);
			return initialState;
		}

		private void OnChange()
		{
			if (Change != null)
				Change();
		}
	}
}
