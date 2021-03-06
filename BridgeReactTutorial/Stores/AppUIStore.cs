﻿using System;
using Bridge.React;
using BridgeReactTutorial.Actions;
using BridgeReactTutorial.API;
using BridgeReactTutorial.ViewModels;
using ProductiveRage.Immutable;

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
			MessageHistory = NonNullList<SavedMessageDetails>.Empty;

			_saveActionRequestId = null;

			dispatcher.Receive(a => a
				.If<StoreInitialised>(
					condition: action => action.Store == this,
					work: action =>
					{
						// When it's time for a Store to be initialised (to set its initial state and call OnChange to let any interested Components know
						// that it's ready), a StoreInitialised action will be dispatched that references the Store. In a more complicated app, a router
						// might choose an initial Store based upon the current URL. (We don't need to do anything within this callback, we just need to
						// match the StoreInitialised so that IfAnyMatched will fire and call OnChange).
					}
				)
				.Else<MessageEditStateChanged>(action => NewMessage = UpdateValidationFor(action.NewState))
				.Else<MessageSaveRequested>(action =>
				{
					NewMessage = NewMessage.With(_ => _.IsSaveInProgress, true);
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
				.IfAnyMatched(OnChange)
			);
		}

		public MessageEditState NewMessage;
		public NonNullList<SavedMessageDetails> MessageHistory;

		public event Action Change;

		private MessageEditState UpdateValidationFor(MessageEditState messageEditState)
		{
			if (messageEditState == null)
				throw new ArgumentNullException("messageEditState");

			return messageEditState
				.With(_ => _.Caption, new NonBlankTrimmedString(messageEditState.Title.Text.Trim() == "" ? "Untitled" : messageEditState.Title.Text))
				.With(_ => _.Title, SetValidationError(messageEditState.Title, messageEditState.Title.Text.Trim() == "", "Must enter a title"))
				.With(_ => _.Content, SetValidationError(messageEditState.Content, messageEditState.Content.Text.Trim() == "", "Must enter message content"));
		}

		private TextEditState SetValidationError(TextEditState textEditState, bool isInvalid, string ifInvalid)
		{
			if (textEditState == null)
				throw new ArgumentNullException("textEditState");

			return textEditState.With(_ => _.ValidationError, isInvalid ? new NonBlankTrimmedString(ifInvalid) : null);
		}

		private MessageEditState GetEmptyNewMessage()
		{
			return UpdateValidationFor(
				new MessageEditState(
					caption: new NonBlankTrimmedString("Untitled"),
					title: new TextEditState(""),
					content: new TextEditState(""),
					isSaveInProgress: false
				)
			);
		}

		private void OnChange()
		{
			Change?.Invoke();
		}
	}
}
