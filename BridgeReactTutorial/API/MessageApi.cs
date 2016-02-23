﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge.Html5;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.API
{
	/// <summary>
	/// In a real application, this would talk to the server to send and retrieve data - to keep this example simple, it handles all of the data internally
	/// (but introduces a few artificial delays to simulate the server communications)
	/// </summary>
	public class MessageApi : IReadAndWriteMessages
	{
		private readonly List<Tuple<int, MessageDetails>> _messages;
		public MessageApi()
		{
			_messages = new List<Tuple<int, MessageDetails>>();
		}

		public Task SaveMessage(MessageDetails message)
		{
			if (message == null)
				throw new ArgumentNullException("message");
			if (string.IsNullOrWhiteSpace(message.Title))
				throw new ArgumentException("The message must have a non-null-or-whitespace-only Title value");
			if (string.IsNullOrWhiteSpace(message.Content))
				throw new ArgumentException("The message must have a non-null-or-whitespace-only Content value");

			var task = new Task<object>(null);
			Window.SetTimeout(
				() =>
				{
					_messages.Add(Tuple.Create(_messages.Count, message));
					task.Complete();
				},
				1000 // Simulate a roundtrip to the server
			);
			return task;
		}

		public Task<IEnumerable<Tuple<int, MessageDetails>>> GetMessages()
		{
			// ToArray is used to return a clone of the message set - otherwise, the caller would end up with a list that is updated when the internal
			// reference within this class is updated (which sounds convenient but it's not the behaviour that would be exhibited if this was "API"
			// was really persisting messages to a server somewhere)
			var task = new Task<IEnumerable<Tuple<int, MessageDetails>>>(null);
			Window.SetTimeout(
				() => task.Complete(_messages.ToArray()),
				1000 // Simulate a roundtrip to the server
			);
			return task;
		}
	}
}
