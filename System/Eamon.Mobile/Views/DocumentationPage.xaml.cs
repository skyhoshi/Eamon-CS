using System;

using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

using Xamarin.Forms;

namespace Eamon.Mobile.Views
{
	public partial class DocumentationPage : ContentPage
	{
		DocumentationViewModel viewModel;

		public DocumentationPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new DocumentationViewModel();
		}
	}
}
