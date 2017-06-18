
// PluginLoopTriggerAction.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

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