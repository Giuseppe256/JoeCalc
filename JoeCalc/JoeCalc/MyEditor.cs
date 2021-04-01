using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace JoeCalc
{
    public class MyEditor : Editor
    {
        public int CursorPosition;
        public int SelectionLength;
        public bool SetCursor = false;
    }
}
