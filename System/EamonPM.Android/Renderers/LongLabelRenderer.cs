
// LongLabelRenderer.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Eamon.Mobile.Models.LongLabel), typeof(EamonPM.LongLabelRenderer))]

namespace EamonPM
{
	public class LongLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				TextView label = Control;

				label.SetMaxLines(9000);
			}
		}
	}
}