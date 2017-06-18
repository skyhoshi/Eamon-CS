
// DocumentationPage.xaml.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class DocumentationPage : ContentPage
	{
		DocumentationViewModel viewModel;

		async protected virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			BatchFilesListView.SelectedItem = null;

			var batchFile = args.Item as BatchFile;

			if (batchFile != null)
			{
				if (batchFile.FileName.ToLower().EndsWith(".txt"))
				{
					await Navigation.PushAsync(new TextFilePage(batchFile));
				}
				else if (batchFile.FileName.ToLower().EndsWith(".htm"))
				{
					await Navigation.PushAsync(new HtmlFilePage(batchFile));
				}
				else
				{
					// unknown file type
				}

				while (Navigation.NavigationStack.Count > 1)
				{
					Navigation.RemovePage(Navigation.NavigationStack[0]);
				}
			}
		}

		public DocumentationPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new DocumentationViewModel();
		}
	}
}
