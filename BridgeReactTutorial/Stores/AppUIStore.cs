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
				if ((message.Action is StoreInitialised) && (((StoreInitialised)message.Action).Store == this))
				{
					// When it's time for a Store to be initialised (to set its initial state and call OnChange to let any interested Components know
					// that it's ready), a StoreInitialised action will be dispatched that references the Store. In a more complicated app, a router
					// might choose an initial Store based upon the current URL.
					OnChange();
				}

				if (message.Action is MessageEditStateChanged)
				{
					var newState = ((MessageEditStateChanged)message.Action).NewState;
					UpdateValidationFor(newState);
					NewMessage = newState;
					OnChange();
				}

				if (message.Action is MessageSaveRequested)
				{
					NewMessage.IsSaveInProgress = true;
					OnChange();
					_saveActionRequestId = messageApi.SaveMessage(((MessageSaveRequested)message.Action).Message);
				}

				if ((message.Action is MessageSaveSucceeded) && (((MessageSaveSucceeded)message.Action).RequestId == _saveActionRequestId))
				{
					// The API's SaveMessage function will fire a MessageSaveSucceeded action when (if) the save is successful and then a subsequent
					// MessageHistoryUpdated action after it's automatically retrieved fresh data, including the newly-saved item (so we need only
					// reset the form here, a MessageHistoryUpdated should be along shortly containig the new item..) 
					_saveActionRequestId = null;
					NewMessage = GetEmptyNewMessage();
					OnChange();
				}

				if (message.Action is MessageHistoryUpdated)
				{
					MessageHistory = ((MessageHistoryUpdated)message.Action).Messages;
					OnChange();
				}
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
