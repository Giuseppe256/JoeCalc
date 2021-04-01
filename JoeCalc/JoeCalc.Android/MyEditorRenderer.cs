using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoeCalc;
using JoeCalc.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(MyEditor), typeof(MyEditorRenderer))]
namespace JoeCalc.Droid
{
    public class MyEditorRenderer : EditorRenderer
    {
        MyEditor myEditor;

        public MyEditorRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (Control != null)
            {
                myEditor = (MyEditor)base.Element;

                if (myEditor.SetCursor)
                {
                    Control.RequestFocus();
                    Control.SetSelection(myEditor.CursorPosition);
                    myEditor.SetCursor = false;
                } 
                else
                {
                    int start = Control.SelectionStart;
                    int end = Control.SelectionEnd;
                    int selectionLength = end - start;
                    myEditor.SelectionLength = selectionLength;
                    myEditor.CursorPosition = end;
                    myEditor.SetCursor = true;
                }
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var editText = (EditText)Control;

                editText.SetTextIsSelectable(true);
                editText.ShowSoftInputOnFocus = false;

                editText.Background = null;
                editText.SetBackgroundColor(Android.Graphics.Color.Transparent);

                editText.Gravity = GravityFlags.Right;
            }
        }
    }
}