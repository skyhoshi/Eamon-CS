
// TextFilePage.xaml.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class TextFilePage : ContentPage
	{
		TextFileViewModel viewModel;

		public TextFilePage(BatchFile batchFile)
		{
			InitializeComponent();

			BindingContext = viewModel = new TextFileViewModel(batchFile);

			App.TextFilePage = this;
		}

		public void RedrawTextFilePageControls()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				OutputLabel.IsVisible = false;

				OutputLabel.IsVisible = true;

				TextFileScrollView.IsVisible = false;

				TextFileScrollView.IsVisible = true;
			});
		}

		protected void BrowseTextFilePage_SizeChanged(object sender, EventArgs e)
		{
			RedrawTextFilePageControls();
		}
	}
}
