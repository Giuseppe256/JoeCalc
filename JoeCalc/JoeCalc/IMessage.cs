using System;
using System.Collections.Generic;
using System.Text;

// https://stackoverflow.com/questions/35279403/toast-equivalent-for-xamarin-forms

namespace JoeCalc
{
    public interface IMessage
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
