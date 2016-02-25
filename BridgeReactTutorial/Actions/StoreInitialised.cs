using System;
using Bridge.React;

namespace BridgeReactTutorial.Actions
{
	/// <summary>
	/// This action is raised when the app is ready, when the Dispatcher has been created and the initial Store is ready to be fired up (in a more
	/// complex app
	/// </summary>
	public class StoreInitialised : IDispatcherAction
	{
		public StoreInitialised(object store)
		{
			if (store == null)
				throw new ArgumentNullException("store");

			Store = store;
		}

		/// <summary>
		/// This will never be null
		/// </summary>
		public object Store { get; private set; }
	}
}
