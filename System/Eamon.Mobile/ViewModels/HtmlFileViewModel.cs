﻿
// HtmlFileViewModel.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Xamarin.Forms;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Mobile.ViewModels
{
	public class HtmlFileViewModel : BaseViewModel
	{
		protected HtmlWebViewSource _outputHtmlWebViewSource;

		public virtual HtmlWebViewSource OutputHtmlWebViewSource
		{
			get
			{
				return _outputHtmlWebViewSource;
			}
			set
			{
				SetProperty(ref _outputHtmlWebViewSource, value);
			}
		}

		public HtmlFileViewModel(BatchFile batchFile)
		{
			Debug.Assert(batchFile != null && batchFile.Name != null && batchFile.FileName != null);

			Title = batchFile.Name;

			OutputHtmlWebViewSource = new HtmlWebViewSource()
			{
				Html = ClassMappings.File.ReadAllText(batchFile.FileName)
			};
		}
	}
}