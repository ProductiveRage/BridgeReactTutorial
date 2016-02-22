using System.Linq;
using Bridge.Html5;

namespace BridgeReactTutorial
{
	public class Class1
	{
		[Ready]
		public static void Go()
		{
			var container = Document.GetElementById("main");
			container.ClassName = string.Join(" ", container.ClassName.Split().Where(c => c != "loading"));
			container.InnerHTML = "Hello!";
		}
	}
}
