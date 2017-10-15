
// EamonRTPage.xaml.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class EamonRTPage : ContentPage
	{
		EamonRTViewModel viewModel;

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

		public EamonRTPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new EamonRTViewModel();
		}
	}
}
