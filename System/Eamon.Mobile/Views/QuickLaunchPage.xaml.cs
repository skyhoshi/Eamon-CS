
// QuickLaunchPage.xaml.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

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
