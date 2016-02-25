using System;

namespace BridgeReactTutorial.API
{
	public class MessageDetails
	{
		public MessageDetails(string title, string content)
		{
			if (string.IsNullOrWhiteSpace(title))
				throw new ArgumentException("The message must have a non-null-or-whitespace-only Title value");
			if (string.IsNullOrWhiteSpace(content))
				throw new ArgumentException("The message must have a non-null-or-whitespace-only Content value");

			Title = title.Trim();
			Content = content.Trim();
	}

		/// <summary>
		/// This will never be null, blank or have any leading or trailing whitespace
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// This will never be null, blank or have any leading or trailing whitespace
		/// </summary>
		public string Content { get; private set; }
}
}
