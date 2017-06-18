
// TextFileViewModel.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Mobile.ViewModels
{
	public class TextFileViewModel : BaseViewModel
	{
		protected string _fontFamily;

		protected long _fontSize;

		protected string _outputText;

		public virtual string FontFamily
		{
			get
			{
				return _fontFamily;
			}
			set
			{
				SetProperty(ref _fontFamily, value);
			}
		}

		public virtual long FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				SetProperty(ref _fontSize, value);
			}
		}

		public virtual string OutputText
		{
			get
			{
				return _outputText;
			}
			set
			{
				SetProperty(ref _outputText, value);
			}
		}

		public virtual void SettingsChangedHandler(object sender, EventArgs args)
		{
			FontFamily = App.SettingsViewModel.FontFamily;

			FontSize = App.SettingsViewModel.FontSize;

			App.TextFilePage.RedrawTextFilePageControls();
		}

		public TextFileViewModel(BatchFile batchFile)
		{
			Debug.Assert(batchFile != null && batchFile.Name != null && batchFile.FileName != null);

			Title = batchFile.Name;

			FontFamily = App.SettingsViewModel.FontFamily;

			FontSize = App.SettingsViewModel.FontSize;

			OutputText = ClassMappings.File.ReadAllText(batchFile.FileName);

			App.SettingsViewModel.SettingsChanged += SettingsChangedHandler;
		}
	}
}