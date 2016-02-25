using System;

namespace BridgeReactTutorial.API
{
	public class NonBlankTrimmedString
	{
		public NonBlankTrimmedString(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
				throw new ArgumentException("Null, blank or whitespace-only value specified");

			Value = value.Trim();
		}

		/// <summary>
		/// This will never be null, blank or have any leading or trailing whitespace
		/// </summary>
		public string Value { get; private set; }
	}
}
