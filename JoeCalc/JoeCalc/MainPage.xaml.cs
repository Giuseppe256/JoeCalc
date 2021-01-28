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
        //Double value = 0;
        String operation = "";
        String operand = "";
        //bool operationPressed = false;

        public MainPage()
        {
            InitializeComponent();

            //cursorPosition = result.CursorPosition;
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
                    //result.Text.Insert(result.CursorPosition, " / ");
                    formattedOperator = "/";
                    break;
                case "X":
                    operation = "*";
                    //result.Text.Insert(result.CursorPosition, " x ");
                    formattedOperator = "x";
                    break;
                case "-":
                    operation = "-";
                    //result.Text.Insert(result.CursorPosition, " - ");
                    formattedOperator = "-";
                    break;
                case "+":
                    operation = "+";
                    //result.Text.Insert(result.CursorPosition, " + ");
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

            //result.CursorPosition = result.Text.Length;
            //value = Double.Parse(result.Text);
            //operationPressed = true;
            //equation.Text = value + " " + operation;
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
                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition + selectionLength);
                str0 = str1 + str2;
                result.Text = str0;
                result.CursorPosition = cursorPosition;
            }
            else
            {
                string str1 = str0.Substring(0, cursorPosition - 1);
                string str2 = str0.Substring(cursorPosition);
                str0 = str1 + str2;
                result.Text = str0;
                result.CursorPosition = cursorPosition - 1;
            }
        }

        void OnNumButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if ((result.Text == "") || (result.Text == "0"))
            {
                //cursorPosition = 0;
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
                operand += b.Text;
                result.CursorPosition = cursorPosition + 1;
            }
            //result.Focus();
            //result.Text.Insert(result.CursorPosition, b.Text);
            //result.Text += b.Text;
        }

        void OnSignButtonClicked(object sender, EventArgs e)
        {

        }

        void OnDotButtonClicked(object sender, EventArgs e)
        {
            if (operand.Contains(".")) { }
            else if (operand == "")
            {
                int cursorPosition = result.CursorPosition;
                string str0 = result.Text;
                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition);
                result.Text = str1 + "0." + str2;
                operand += "0.";
                result.CursorPosition = cursorPosition + 2;
            }
            else
            {
                int cursorPosition = result.CursorPosition;
                string str0 = result.Text;
                string str1 = str0.Substring(0, cursorPosition);
                string str2 = str0.Substring(cursorPosition);
                result.Text = str1 + "." + str2;
                operand += ".";
                result.CursorPosition = cursorPosition + 1;
            }
        }

        void OnEqualsButtonClicked(object sender, EventArgs e)
        {
            
        }
    }
}
