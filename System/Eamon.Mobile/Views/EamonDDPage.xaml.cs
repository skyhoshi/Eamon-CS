using System;

using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

using Xamarin.Forms;

namespace Eamon.Mobile.Views
{
	public partial class EamonDDPage : ContentPage
	{
		EamonDDViewModel viewModel;

		async protected virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			BatchFilesListView.SelectedItem = null;

			App.BatchFile = args.Item as BatchFile;

			await Navigation.PushAsync(new PluginLauncherPage(Title));

			while (Navigation.NavigationStack.Count > 1)
			{
				Navigation.RemovePage(Navigation.NavigationStack[0]);
			}
		}

		public EamonDDPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new EamonDDViewModel();
		}
	}
}
