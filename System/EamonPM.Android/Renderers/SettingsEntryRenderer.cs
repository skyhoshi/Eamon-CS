﻿
using Android.Views.InputMethods;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Eamon.Mobile;

[assembly: ExportRenderer(typeof(Eamon.Mobile.Models.SettingsEntry), typeof(EamonPM.SettingsEntryRenderer))]

namespace EamonPM
{
	public class SettingsEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				var entry = (TextView)Control;

				entry.ImeOptions = (ImeAction)ImeFlags.NoExtractUi;
			}
		}
	}
}