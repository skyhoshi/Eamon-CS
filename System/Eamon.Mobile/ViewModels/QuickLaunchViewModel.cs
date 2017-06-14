
using System.Collections.Generic;

namespace Eamon.Mobile.ViewModels
{
	public class QuickLaunchViewModel : BaseViewModel
	{
		public List<string> Folders { get; set; }

		public QuickLaunchViewModel()
		{
			Title = "QuickLaunch";

			Folders = new List<string>()
			{
				"EamonDD",
				"EamonMH",
				"EamonRT"
			};
		}

	}
}