using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using JoeCalc;
using JoeCalc.iOS;

[assembly: ExportRenderer(typeof(MyEntry), typeof(MyEntryRenderer))]
namespace JoeCalc.iOS
{
	public class MyEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
		}
	}
}