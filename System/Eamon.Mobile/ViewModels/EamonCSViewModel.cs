
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