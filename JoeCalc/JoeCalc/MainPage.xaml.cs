using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            operand = "";
            operation = "";
        }

        void OnOperatorButtonClicked(object sender, EventArgs e)
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

            int cursorPosition = result.CursorPosition;
            string str0 = result.Text;
            string str1 = str0.Substring(0, cursorPosition);
            string str2 = str0.Substring(cursorPosition);
            result.Text = str1 + formattedOperator + str2;
            result.CursorPosition = cursorPosition + 1;
            operand = "";
            operands = op.BreakDownOperation(result.Text);
        }

        void OnParenthesesButtonClicked(object sender, EventArgs e)
        {

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
                result.Text = str1 + b.Text + str2;
                if (cursorPosition == str0.Length)
                    operand += b.Text;
                result.CursorPosition = cursorPosition + 1;
            }
            operands = op.BreakDownOperation(result.Text);
        }

        void OnSignButtonClicked(object sender, EventArgs e)
        {
            string str0 = result.Text;
            int cursorPosition = result.CursorPosition;

            if (str0 == "")
            {
                result.Text = "(-";
                operand = "(-";
                result.CursorPosition = 2;
                operands = op.BreakDownOperation(result.Text);
            }
            else
            {
                Operand currentOperand = op.GetOperand(operands, cursorPosition);
                if (cursorPosition < str0.Length)
                    operand = currentOperand.Number;
                int currentStartPosition = currentOperand.StartPosition;
                if (operand == "x" || operand == "/" || operand == "+" || operand == "-")
                {
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
                }
                else
                {
                    string str1 = str0.Substring(0, cursorPosition);
                    string str2 = str0.Substring(cursorPosition);
                    string str3 = str1 + "." + str2;
                    result.Text = str3;
                    operand += ".";
                    result.CursorPosition = cursorPosition + 1;
                }
            }
        }

        void OnEqualsButtonClicked(object sender, EventArgs e)
        {
            
        }
    }
}
