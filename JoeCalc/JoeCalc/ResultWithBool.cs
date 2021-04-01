using System;
using System.Collections.Generic;
using System.Text;

namespace JoeCalc
{
    public class ResultWithBool
    {
        private string _Result;
        private bool _Error;

        public ResultWithBool(string Result, bool Error)
        {
            _Result = Result;
            _Error = Error;
        }

        public string Result
        {
            get => _Result;
            set => _Result = value;
        }

        public bool Error
        {
            get => _Error;
            set => _Error = value;
        }
    }
}
