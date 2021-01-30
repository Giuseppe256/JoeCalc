using System;
using System.Collections.Generic;
using System.Text;

namespace JoeCalc
{
    class Operand
    {
        private int _startPosition;
        private string _number;

        public Operand(int startPosition, string number)
        {
            _startPosition = startPosition;
            _number = number;
        }

        public int StartPosition
        {
            get => _startPosition;
            set => _startPosition = value;
        }

        public string Number
        {
            get => _number;
            set => _number = value;
        }
    }
}
