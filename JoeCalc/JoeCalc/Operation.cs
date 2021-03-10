using System;
using System.Collections.Generic;
using System.Text;

namespace JoeCalc
{
    class Operation
    {
        private string _inOp, _outOp, _currentOperand;
        private int _curPos, _currentStart, _index;
        private bool _operandCreated;
        private Operand _returnOperand;

        public Operand GetOperand(List<Operand> operands, int curPos)
        {
            List<Operand> _operands = operands;
            _curPos = curPos;
            _currentOperand = "";
            
            for (int i = 0; i < _operands.Count; i++)
            {
                if ((_operands[i].StartPosition <= _curPos) && (_curPos <= (_operands[i].StartPosition + _operands[i].Number.Length)))
                {
                    _returnOperand = new Operand(_operands[i].StartPosition, _operands[i].Number);
                    break;
                }
            }
            return _returnOperand;
        }

        public List<Operand> BreakDownOperation(string inOp)
        {
            List<Operand> _operands = new List<Operand>();
            _inOp = inOp;
            _operandCreated = false;
            _currentStart = 0;
            _currentOperand = "";
            _index = 0;
            for (int i = 0; i < _inOp.Length; i++)
            {
                char x = _inOp[i];
                char w = '\0';
                if (x == '-' && i > 0)
                {
                    w = _inOp[i - 1];
                }
                    
                if (Char.IsNumber(x) || x == '.' || x == '(' || (x == '-' && w == '(') || x == ')')
                {
                    if (_currentOperand == "")
                    {
                        _currentStart = _index;
                    }
                    _currentOperand += x;
                    _index++;

                    if (i == _inOp.Length - 1)
                    {
                        Operand _operand = new Operand(_currentStart, _currentOperand);
                        _operands.Add(_operand);
                        _operandCreated = true;
                    }
                }
                else
                {
                    Operand _operand = new Operand(_currentStart, _currentOperand);
                    _operands.Add(_operand);
                    Operand _operator = new Operand(_index, x.ToString());
                    _operands.Add(_operator);
                    _index++;
                    _operandCreated = true;
                }

                if (_operandCreated == true)
                {
                    _currentOperand = "";
                    _operandCreated = false;
                }
            }
            return _operands;
        }

        public List<Operand> BreakDownSubOps(string inOp)
        {
            List<Operand> _subOps = new List<Operand>();
            _inOp = inOp;
            string[] _subOperations = new string[_inOp.Length];
            int _opCount = 0;
            int _subLevel = 0;
            _operandCreated = false;
            _currentStart = 0;
            _currentOperand = "";
            _index = 0;
            for (int i = 0; i < _inOp.Length; i++)
            {
                char x = _inOp[i];

                if (x == '(')
                {
                    _currentOperand = "(";
                    for (int j = 0; j <= _subLevel; j++)
                    {
                        _subOperations[j] += x;
                    }
                    _subLevel++;
                    _opCount++;
                }
                else if (x == ')')
                {
                    _currentOperand += x;
                    for (int j = 0; j <= _subLevel; j++)
                    {
                        _subOperations[j] += x;
                    }


                    Operand _operand = new Operand(_currentStart, _currentOperand);
                    _subOps.Add(_operand);
                    _index++;
                    _operandCreated = true;
                }
                else
                {
                    if (_currentOperand == "")
                    {
                        _currentStart = _index;
                    }
                    _currentOperand += x;
                    _index++;

                    if (i == _inOp.Length - 1)
                    {
                        Operand _operand = new Operand(_currentStart, _currentOperand);
                        _subOps.Add(_operand);
                        _operandCreated = true;
                    }
                }

                if (_operandCreated == true)
                {
                    _currentOperand = "";
                    _operandCreated = false;
                }
            }


            return _subOps;
        }

        private string BuildOperation(List<Operand> operands)
        {
            _outOp = "";
            List<Operand> _operands = operands;

            for (int i = 0; i < _operands.Count; i++)
            {
                _outOp += _operands[i].Number;
            }
            
            return _outOp;
        }
    }
}
