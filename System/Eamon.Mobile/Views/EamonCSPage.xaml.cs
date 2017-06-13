using System;

using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

using Xamarin.Forms;

namespace Eamon.Mobile.Views
{
	public partial class EamonCSPage : ContentPage
	{
		EamonCSViewModel viewModel;

		async protected virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			FoldersListView.SelectedItem = null;

			var folder = args.Item as string;

			if (!string.IsNullOrWhiteSpace(folder))
			{
				if (string.Equals(folder, "Documentation", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new DocumentationPage());
				}
				else
				{
					await Navigation.PushAsync(new QuickLaunchPage());
				}
			}
		}

		public EamonCSPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new EamonCSViewModel();
		}
	}
}
