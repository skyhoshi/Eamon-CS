
// EamonCSPage.xaml.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class EamonCSPage : ContentPage
	{
		EamonCSViewModel viewModel;

		/// <summary></summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
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
