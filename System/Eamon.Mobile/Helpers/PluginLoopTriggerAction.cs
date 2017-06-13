
using Xamarin.Forms;

namespace Eamon.Mobile.Helpers
{
	public class PluginLoopTriggerAction : TriggerAction<VisualElement>
	{
		protected static bool LaunchedPluginLoop { get; set; }

		public PluginLoopTriggerAction()
		{

		}

		protected override void Invoke(VisualElement visual)
		{
			if (!LaunchedPluginLoop)
			{
				LaunchedPluginLoop = true;

				App.StartGameThread();
			}
		}
	}
}