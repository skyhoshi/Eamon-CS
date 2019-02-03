
// AboutViewModel.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Eamon.Mobile.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutViewModel()
		{
			Title = "About";

			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://github.com/firstmethod/Eamon-CS")));
		}

		public ICommand OpenWebCommand { get; }
	}
}
