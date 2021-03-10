using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JoeCalc
{
    public partial class MainPage : ContentPage
    {
        List<Operand> operands = new List<Operand>();
        Operation op = new Operation();
        String operation = "";
        String operand = "";

        public MainPage()
        {
            InitializeComponent();
        }

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            result.Text = "";
            runningResult.Text = "";
            operand = "";
            operation = "";
        }

        void OnOperatorButtonClicked(object sender, EventArgs e)
        {
            int cursorPosition = result.CursorPosition;
            if (cursorPosition > 0)
            {
                Button b = (Button)sender;
                operation = b.Text;
                string formattedOperator = "";
                switch (operation)
                {
                    case "/":
                        operation = "/";
                        formattedOperator = "/";
                        break;
                    case "X":
                        operation = "*";
                        formattedOperator = "x";
                        break;
                    case "-":
                        operation = "-";
                        formattedOperator = "-";
                        break;
                    case "+":
                        operation = "+";
                        formattedOperator = "+";
                        break;
                    default:
                        break;
                }

                string str0 = result.Text;
                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition);
                char before = '\0';
                char after = '\0';
                if (str1.Length > 0)
                {
                    before = str1[str1.Length - 1];
                }
                if (str2.Length > 0)
                {
                    after = str2[0];
                }

                if (before == 'x' || before == '/' || before == '+' || before == '-')
                {
                    string str3 = str1.Substring(0, str1.Length - 1);
                    result.Text = str3 + formattedOperator + str2;
                    result.CursorPosition = cursorPosition;
                }
                else if (after == 'x' || after == '/' || after == '+' || after == '-')
                {
                    string str3 = str2.Substring(1);
                    result.Text = str1 + formattedOperator + str3;
                    result.CursorPosition = cursorPosition + 1;
                }
                else
                {
                    result.Text = str1 + formattedOperator + str2;
                    result.CursorPosition = cursorPosition + 1;
                }
                operand = "";
                operands = op.BreakDownOperation(result.Text);
                UpdateRunningResult();
            }
        }

        void OnParenthesesButtonClicked(object sender, EventArgs e)
        {
            string str0 = result.Text;
            int cursorPosition = result.CursorPosition;
            
            if (str0.Length > 0 && cursorPosition > 0)
            {
                char x = str0[cursorPosition - 1];
                string parenth;
                int parenthSize = 1;
                bool parenthSwitch = false;
                int openCount = 0;
                int closedCount = 0;


                for (int i = 0; i < cursorPosition; i++)
                {
                    if (str0[i] == '(')
                    {
                        openCount++;
                        parenthSwitch = true;
                    }
                    else if (str0[i] == ')')
                    {
                        closedCount++;
                        if (closedCount >= openCount)
                        {
                            parenthSwitch = false;
                        }
                    }
                }

                if (Char.IsNumber(x) && parenthSwitch || (x == ')' && closedCount < openCount))
                {
                    parenth = ")";
                }
                else if ((Char.IsNumber(x) && !parenthSwitch) || (x == ')' && closedCount >= openCount))
                {
                    parenth = "x(";
                    parenthSize = 2;
                }
                else if (x == '.' && parenthSwitch)
                {
                    parenth = "0)";
                    parenthSize = 2;
                }
                else if (x == '.' && !parenthSwitch)
                {
                    parenth = "0x(";
                    parenthSize = 3;
                }
                else
                {
                    parenth = "(";
                }

                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition);
                string str3 = str1 + parenth + str2;
                result.Text = str3;
                operand += parenth;
                result.CursorPosition = cursorPosition + parenthSize;
            }
            else if (cursorPosition == 0)
            {
                result.Text = "(" + str0;
                operand = "(" + operand;
                result.CursorPosition = 1;
            }
            else
            {
                operand = "(";
                result.Text = "(";
                result.CursorPosition = 1;
            }

            operands = op.BreakDownOperation(result.Text);
            UpdateRunningResult();
        }

        void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            int cursorPosition = result.CursorPosition;
            int selectionLength = result.SelectionLength;
            string str0 = result.Text;
            if (str0 == "") { }
            else if (selectionLength > 0)
            {
                string str1 = str0.Substring(0, cursorPosition - selectionLength);
                string str2 = str0.Substring(cursorPosition);
                str0 = str1 + str2;
                result.Text = str0;
                result.CursorPosition = cursorPosition;
                operands = op.BreakDownOperation(result.Text);
                operand = op.GetOperand(operands, cursorPosition).Number;
                UpdateRunningResult();
            }
            else
            {
                string str1 = str0.Substring(0, cursorPosition - 1);
                string str2 = str0.Substring(cursorPosition);
                str0 = str1 + str2;
                result.Text = str0;
                cursorPosition--;
                result.CursorPosition = cursorPosition;
                operands = op.BreakDownOperation(result.Text);
                operand = op.GetOperand(operands, cursorPosition).Number;
                UpdateRunningResult();
            }
        }

        void OnNumButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if ((result.Text == "") || (result.Text == "0"))
            {
                result.Text = b.Text;
                operand = b.Text;
                result.CursorPosition++;
            }
            else
            {
                int cursorPosition = result.CursorPosition;
                string str0 = result.Text;
                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition);
                char after = '\0';
                if (str2.Length > 0)
                {
                    after = str2[0];
                }
                if (str1[str1.Length - 1] == ')')
                {
                    result.Text = str1 + 'x' + b.Text + str2;
                }
                else if (after == '(')
                {
                    result.Text = str1 + b.Text + 'x' + str2;
                }
                else
                {
                    result.Text = str1 + b.Text + str2;
                }
                
                if (cursorPosition == str0.Length)
                    operand += b.Text;
                result.CursorPosition = cursorPosition + 1;
            }
            operands = op.BreakDownOperation(result.Text);
            UpdateRunningResult();

        }

        void OnSignButtonClicked(object sender, EventArgs e)
        {
            string str0 = result.Text;
            int cursorPosition = result.CursorPosition;

            if (str0 == "")     // If Result is Empty
            {
                result.Text = "(-";    
                operand = "(-";    
                result.CursorPosition = 2;     
                operands = op.BreakDownOperation(result.Text);
            }
            else  // If Result is Not Empty
            {
                Operand currentOperand = op.GetOperand(operands, cursorPosition);         
                if (cursorPosition < str0.Length)       // If Cursor is not at the end
                    operand = currentOperand.Number;    // Set operand to current operand
                int currentStartPosition = currentOperand.StartPosition;        // Set start postion to current operand's start
                if (operand == "x" || operand == "/" || operand == "+" || operand == "-")   // If operand is an operator
                {                                                                           // 
                    currentOperand = op.GetOperand(operands, cursorPosition + 1);
                    operand = currentOperand.Number;
                    currentStartPosition = currentOperand.StartPosition;
                }

                if (operand.Contains("-"))
                {
                    string str1 = str0.Substring(0, currentStartPosition);
                    string str2 = str0.Substring(currentStartPosition + 2);
                    string str3 = str1 + str2;
                    result.Text = str3;
                    operand = operand.Remove(0, 2);
                    cursorPosition -= 2;
                    result.CursorPosition = cursorPosition;
                }
                else if (operand == "" && cursorPosition == str0.Length)
                {
                    string str1 = str0 + "(-";
                    result.Text = str1;
                    operand = "(-";
                    cursorPosition += 2;
                    result.CursorPosition = cursorPosition;
                }
                else
                {
                    string str1 = str0.Substring(0, currentStartPosition);
                    string str2 = str0.Substring(currentStartPosition);
                    string str3 = str1 + "(-" + str2;
                    result.Text = str3;
                    operand = operand.Insert(0, "(-");
                    cursorPosition += 2;
                    result.CursorPosition = cursorPosition;
                }
                operands = op.BreakDownOperation(result.Text);
                UpdateRunningResult();
            }
        }

        void OnDotButtonClicked(object sender, EventArgs e)
        {
            if (operand.Contains(".")) { }
            else if (result.Text == "")
            {
                result.Text += "0.";
                operand += "0.";
                result.CursorPosition += 2;
                operands = op.BreakDownOperation(result.Text);
            }
            else
            {
                int cursorPosition = result.CursorPosition;
                string str0 = result.Text;
                Operand currentOperand = op.GetOperand(operands, cursorPosition);
                string currentNumber;
                int currentStartPosition;
                
                if ((currentOperand == null) || !double.TryParse(currentOperand.Number, out double _))
                {
                    currentNumber = "";
                    currentStartPosition = cursorPosition;
                }
                else
                {
                    currentNumber = currentOperand.Number;
                    currentStartPosition = currentOperand.StartPosition;
                }

                if (currentNumber.Contains(".")) { }
                else if ((currentNumber == "") || (currentStartPosition == cursorPosition))
                {
                    string str1 = str0.Substring(0, cursorPosition);
                    string str2 = str0.Substring(cursorPosition);
                    string str3 = str1 + "0." + str2;
                    result.Text = str3;
                    operand += "0.";
                    result.CursorPosition = cursorPosition + 2;
                    operands = op.BreakDownOperation(result.Text);
                    UpdateRunningResult();
                }
                else
                {
                    string str1 = str0.Substring(0, cursorPosition);
                    string str2 = str0.Substring(cursorPosition);
                    string str3 = str1 + "." + str2;
                    result.Text = str3;
                    operand += ".";
                    result.CursorPosition = cursorPosition + 1;
                    operands = op.BreakDownOperation(result.Text);
                    UpdateRunningResult();
                }
            }
        }

        void OnEqualsButtonClicked(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string equation = result.Text;
            
            if (equation != "" && (Char.IsNumber(equation[equation.Length - 1]) || equation[equation.Length - 1] == ')'))
            {
                equation = equation.Replace("x", "*");

                int openCount = 0;
                int closedCount = 0;

                for (int i = 0; i < equation.Length; i++)
                {
                    if (equation[i] == '(')
                    {
                        openCount++;
                    }
                    else if (equation[i] == ')')
                    {
                        closedCount++;
                    }
                }

                int missingClosed = openCount - closedCount;

                for (int i = 0; i < missingClosed; i++)
                {
                    equation += ")";
                }


                var answerObject = dt.Compute(equation, "");
                string answer = answerObject.ToString();
                if (answer == "Infinity")
                {
                    runningResult.Text = "Can't divide by zero";
                }
                else
                {
                    result.Text = answer;
                    operand = answer;
                    operation = "";
                    runningResult.Text = "";
                    result.CursorPosition = answer.Length;
                }
            }
            else
            {
                runningResult.Text = "Invalid format";
            }
            
        }

        void UpdateRunningResult()
        {
            DataTable dt = new DataTable();
            string equation = result.Text;
            operands = op.BreakDownOperation(equation);
            char endChar = '\0';
            if (equation != "")
            {
                endChar = equation[equation.Length - 1];
            }
            if (operands.Count > 2 && (Char.IsNumber(endChar) || endChar == ')'))
            {
                equation = equation.Replace("x", "*");

                int openCount = 0;
                int closedCount = 0;

                for (int i = 0; i < equation.Length; i++)
                {
                    if (equation[i] == '(')
                    {
                        openCount++;
                    }
                    else if (equation[i] == ')')
                    {
                        closedCount++;
                    }
                }

                int missingClosed = openCount - closedCount;

                for (int i = 0; i < missingClosed; i++)
                {
                    equation += ")";
                }


                var answerObject = dt.Compute(equation, "");
                string answer = answerObject.ToString();
                if (answer == "Infinity")
                {
                    runningResult.Text = "";
                }
                else
                {
                    runningResult.Text = answer;
                }
            }
            else
            {
                runningResult.Text = "";
            }
        }
    }
}
