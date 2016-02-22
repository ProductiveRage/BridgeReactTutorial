using System;
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
				() => task.Complete(),
				1000 // Simulate a roundtrip to the server
			);
			return task;
		}
	}
}
