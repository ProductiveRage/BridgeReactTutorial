using Bridge.React;
using BridgeReactTutorial.API;
using BridgeReactTutorial.ViewModels;

namespace BridgeReactTutorial.Components
{
	public class AppContainer : Component<AppContainer.Props, AppContainer.State>
	{
		public AppContainer(AppContainer.Props props) : base(props) { }

		protected override State GetInitialState()
		{
			return State.GetDefault();
		}

		public override ReactElement Render()
		{
			return new MessageEditor(new MessageEditor.Props
			{
				ClassName = "message",
				Title = state.Message.Title,
				Content = state.Message.Content,
				OnChange = newMessage => SetState(new State { Message = newMessage }),
				OnSave = () =>
				{
					SetState(new State { Message = state.Message, IsSaveInProgress = true });
					props.MessageApi.SaveMessage(state.Message).ContinueWith(task => SetState(State.GetDefault()));
				},
				Disabled = state.IsSaveInProgress
			});
		}

		public sealed class Props
		{
			public IReadAndWriteMessages MessageApi;
		}

		public class State
		{
			public MessageDetails Message;
			public bool IsSaveInProgress;

			public static State GetDefault()
			{
				return new State
				{
					Message = new MessageDetails { Title = "", Content = "" },
					IsSaveInProgress = false
				};
			}
		}
	}
}
