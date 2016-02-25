using System.Linq;
using Bridge.Html5;
using Bridge.React;
using BridgeReactTutorial.Actions;
using BridgeReactTutorial.API;
using BridgeReactTutorial.Components;
using BridgeReactTutorial.Stores;

namespace BridgeReactTutorial
{
	public class App
	{
		[Ready]
		public static void Go()
		{
			var dispatcher = new AppDispatcher();
			var store = new AppUIStore(dispatcher, new MessageApi(dispatcher));

			var container = Document.GetElementById("main");
			container.ClassName = string.Join(" ", container.ClassName.Split().Where(c => c != "loading"));
			React.Render(
				new AppContainer(new AppContainer.Props { Store = store, Dispatcher = dispatcher }),
				container
			);

			// After the Dispatcher and the Store and the Container Component are all associated with each other, the Store needs to be told that
			// it's time to set its initial state, so that the Component can receive an OnChange event and draw itself accordingly. In a more
			// complicated app, this would probably be an event fired by the router - initialising the Store appropriate to the current URL,
			// but in this case there's only a single Store to initialise.
			dispatcher.HandleViewAction(new StoreInitialised(store));
		}
	}
}
