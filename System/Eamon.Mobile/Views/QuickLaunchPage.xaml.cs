using System;

using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

using Xamarin.Forms;

namespace Eamon.Mobile.Views
{
	public partial class QuickLaunchPage : ContentPage
	{
		QuickLaunchViewModel viewModel;

		async protected virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			FoldersListView.SelectedItem = null;

			var folder = args.Item as string;

			if (!string.IsNullOrWhiteSpace(folder))
			{
				if (string.Equals(folder, "EamonDD", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new EamonDDPage());
				}
				else if (string.Equals(folder, "EamonMH", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new EamonMHPage());
				}
				else
				{
					await Navigation.PushAsync(new EamonRTPage());
				}
			}
		}

		public QuickLaunchPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new QuickLaunchViewModel();
		}
	}
}
