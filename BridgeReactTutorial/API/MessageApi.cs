using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bridge;
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

			// To further mimic a server-based API (where other people may be recording messages of their own), after a 10s delay a periodic task will be
			// executed to retrieve a new message
			Window.SetTimeout(
				() => Window.SetInterval(GetChuckNorrisFact, 5000),
				10000
			);
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

		private void GetChuckNorrisFact()
		{
			var request = new XMLHttpRequest();
			request.ResponseType = XMLHttpRequestResponseType.Json;
			request.OnReadyStateChange = () =>
			{
				if (request.ReadyState != AjaxReadyState.Done)
					return;

				if ((request.Status == 200) || (request.Status == 304))
				{
					try
					{
						var apiResponse = (ChuckNorrisFactApiResponse)request.Response;
						if ((apiResponse.Type == "success") && (apiResponse.Value != null) && !string.IsNullOrWhiteSpace(apiResponse.Value.Joke))
						{
							// The Chuck Norris Facts API (http://www.icndb.com/api/) returns strings html-encoded, so they need decoding before
							// be wrapped up in a MessageDetails instance
							SaveMessage(new MessageDetails { Title = "Fact", Content = HtmlDecode(apiResponse.Value.Joke) });
							return;
						}
					}
					catch
					{
						// Ignore any error and drop through to the fallback message-generator below
					}
				}
				SaveMessage(new MessageDetails { Title = "Fact", Content = "API call failed when polling for server content :(" });
			};
			request.Open("GET", "http://api.icndb.com/jokes/random");
			request.Send();
		}

		private string HtmlDecode(string value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			var wrapper = Document.CreateElement("div");
			wrapper.InnerHTML = value;
			return wrapper.TextContent;
		}

		[IgnoreCast]
		private class ChuckNorrisFactApiResponse
		{
			public extern string Type { [Template("type")] get; }
			public extern FactDetails Value { [Template("value")] get; }

			[IgnoreCast]
			public class FactDetails
			{
				public extern int Id { [Template("id")] get; }
				public extern string Joke { [Template("joke")]get; }
			}
		}
	}
}
