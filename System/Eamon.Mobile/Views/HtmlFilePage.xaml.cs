
// HtmlFilePage.xaml.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class HtmlFilePage : ContentPage
	{
		HtmlFileViewModel viewModel;

		public HtmlFilePage(BatchFile batchFile)
		{
			InitializeComponent();

			BindingContext = viewModel = new HtmlFileViewModel(batchFile);
		}
	}
}
