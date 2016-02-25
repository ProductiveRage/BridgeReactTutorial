using System;

namespace BridgeReactTutorial.API
{
	public class MessageDetails
	{
		public MessageDetails(NonBlankTrimmedString title, NonBlankTrimmedString content)
		{
			if (title == null)
				throw new ArgumentNullException("title");
			if (content == null)
				throw new ArgumentNullException("content");

			Title = title;
			Content = content;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public NonBlankTrimmedString Title { get; private set; }

		/// <summary>
		/// This will never be null
		/// </summary>
		public NonBlankTrimmedString Content { get; private set; }
	}
}
