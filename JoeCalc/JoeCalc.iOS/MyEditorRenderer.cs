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

[assembly: ExportRenderer(typeof(MyEditor), typeof(MyEditorRenderer))]
namespace JoeCalc.iOS
{
    public class MyEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
        }
    }
}