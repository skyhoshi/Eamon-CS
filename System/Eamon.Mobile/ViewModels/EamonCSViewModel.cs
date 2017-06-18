
// EamonCSViewModel.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;

namespace Eamon.Mobile.ViewModels
{
	public class EamonCSViewModel : BaseViewModel
	{
		public List<string> Folders { get; set; }

		public EamonCSViewModel()
		{
			Title = "Eamon CS";

			Folders = new List<string>()
			{
				"Documentation",
				"QuickLaunch"
			};
		}

	}
}