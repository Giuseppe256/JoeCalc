using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JoeCalc
{
    class Operation
    {
        private string _inOp, _currentOperand;
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

        public ResultWithBool Solve(string equation)
        {
            ResultWithBool _resultWithBool = new ResultWithBool("", false);
            
            string _equation = equation;
            string _pattern = @"([+\-*/])";
            //string _pattern = @"([*()\^\/]|(?<!E)[\+\-])";
            string[] _operators = Regex.Split(_equation, _pattern);
            List<string> _operatorList = new List<string>(_operators);

            // Remove empty strings
            for (int i = 0; i < _operatorList.Count; i++)
            {
                if (_operatorList[i] == "")
                {
                    _operatorList.RemoveAt(i);
                }
            }

            // Join negative signs with numbers
            for (int i = 0; i < _operatorList.Count; i++)
            {
                if (_operatorList[i] == "-")
                {
                    string str0 = "-";
                    if (i > 0)
                    {
                        string h = _operatorList[i - 1];
                        if (h == "+" || h == "-" || h == "*" || h == "/" || h == "(")
                        {
                            str0 += _operatorList[i + 1];
                            _operatorList[i] = str0;
                            _operatorList.RemoveAt(i + 1);
                        }
                    }
                    else
                    {
                        str0 += _operatorList[i + 1];
                        _operatorList[i] = str0;
                        _operatorList.RemoveAt(i + 1);
                    }
                }
            }

            // Solve all * and / first
            for (int i = 0; i < _operatorList.Count; i++)
            {
                if (_operatorList[i] == "*" || _operatorList[i] == "/")
                {
                    decimal _num0, _num1, _num2;
                    if (i < 1 || !Decimal.TryParse(_operatorList[i - 1], out _num1))
                    {
                        _resultWithBool.Error = true;
                        _resultWithBool.Result = "Invalid format";
                        return _resultWithBool;
                    }
                    if (i >= _operatorList.Count - 1 || !Decimal.TryParse(_operatorList[i + 1], out _num2))
                    {
                        _resultWithBool.Error = true;
                        _resultWithBool.Result = "Invalid format";
                        return _resultWithBool;
                    }

                    if (_operatorList[i] == "*")
                    {
                        _num0 = _num1 * _num2;
                    }
                    else
                    {
                        if (_num2 == 0)
                        {
                            _resultWithBool.Error = true;
                            _resultWithBool.Result = "Can't divide by 0";
                            return _resultWithBool;
                        }
                        else
                        {
                            _num0 = _num1 / _num2;
                        }
                    }

                    _operatorList[i - 1] = _num0.ToString();
                    _operatorList.RemoveAt(i);
                    _operatorList.RemoveAt(i);
                    i--;
                }
            }

            // Solve all + and -
            for (int i = 0; i < _operatorList.Count; i++)
            {
                if (_operatorList[i] == "+" || _operatorList[i] == "-")
                {
                    decimal _num0, _num1, _num2;
                    if (i < 1 || !Decimal.TryParse(_operatorList[i - 1], out _num1)) 
                    {
                        _resultWithBool.Error = true;
                        _resultWithBool.Result = "Invalid format";
                        return _resultWithBool;
                    }
                    if (i >= _operatorList.Count - 1 || !Decimal.TryParse(_operatorList[i + 1], out _num2))
                    {
                        _resultWithBool.Error = true;
                        _resultWithBool.Result = "Invalid format";
                        return _resultWithBool;
                    }

                    if (_operatorList[i] == "+")
                    {
                        _num0 = _num1 + _num2;
                    }
                    else
                    {
                        _num0 = _num1 - _num2;
                    }

                    _operatorList[i - 1] = _num0.ToString();
                    _operatorList.RemoveAt(i);
                    _operatorList.RemoveAt(i);
                    i--;
                }
            }

            _resultWithBool.Result = _operatorList[0];
            return _resultWithBool;
        }

        public ResultWithBool SolveOperation(List<Operand> operands)
        {
            List<Operand> _operands = operands;
            ResultWithBool _resultWithBool = new ResultWithBool("", false);
            string _equation = "";
            
            // Count number of parentheses
            int subEquationCount = 0;
            for (int i = 0; i < _operands.Count; i++)
            {
                if (_operands[i].Number.Contains("("))
                {
                    string str0 = _operands[i].Number;
                    for (int j = 0; j < str0.Length; j++)
                    {
                        if (str0[j] == '(')
                        {
                            subEquationCount++;
                        }
                    }
                }
            }

            // Solve sub equations
            for (int i = 0; i < subEquationCount; i++)
            {
                // Count down through operands
                for (int j = _operands.Count - 1; j >= 0; j--)
                {
                    // Find sub equation start
                    if (_operands[j].Number.Contains("("))
                    {
                        int _openCount = 0;
                        int _closedCount = 0;
                        int _operatorCount = 0;

                        // Add first number to sub equation
                        string _subEquation = _operands[j].Number;
                        
                        // Get rest of sub equation
                        for (int k = j + 1; k < _operands.Count; k++)
                        {
                            _subEquation += _operands[k].Number;
                            _operatorCount++;

                            if (_subEquation.Contains(")"))
                            {
                                break;
                            }
                        }

                        // Get rid of parentheses
                        for (int k = 0; k < _subEquation.Length; k++)
                        {
                            if (_subEquation[k] == '(' || _subEquation[k] == ')')
                            {
                                switch (_subEquation[k])
                                {
                                    case '(':
                                        _openCount++;
                                        break;
                                    case ')':
                                        _closedCount++;
                                        break;
                                }
                                string str1 = _subEquation.Substring(0, k);
                                string str2 = _subEquation.Substring(k + 1);
                                _subEquation = str1 + str2;
                                k--;
                            }
                        }

                        // Solve sub equation
                        _resultWithBool = Solve(_subEquation);

                        // Check for error
                        if (_resultWithBool.Error)
                        {
                            return _resultWithBool;
                        }

                        // Add extra open parentheses to result
                        if (_openCount > 1)
                        {
                            for (int k = 1; k < _openCount; k++)
                            {
                                string str0 = "(" + _resultWithBool.Result;
                                _resultWithBool.Result = str0;
                            }
                        }

                        // Add extra closed parentheses to result
                        if (_closedCount > 1)
                        {
                            for (int k = 1; k < _closedCount; k++)
                            {
                                string str0 = _resultWithBool.Result + ")";
                                _resultWithBool.Result = str0;
                            }
                        }

                        // Replace sub equation with result in full equation
                        _operands[j].Number = _resultWithBool.Result;

                        // Remove solved sub equation from full equation
                        for (int k = 0; k < _operatorCount; k++)
                        {
                            _operands.RemoveAt(j + 1);
                        }

                        // Increment counter so result gets checked for parentheses again
                        j++;
                    }
                }
            }

            // Solve rest of equation
            if (_operands.Count > 1)
            {
                // Convert operands list to string
                for (int i = 0; i < _operands.Count; i++)
                {
                    _equation += _operands[i].Number;
                }

                _resultWithBool = Solve(_equation);
            }
            else
            {
                _resultWithBool.Result = _operands[0].Number;
            }

            return _resultWithBool;
        }
    }
}
