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
        int cursorPosition = 0;

        public MainPage()
        {
            InitializeComponent();

            //cursorPosition = result.CursorPosition;
        }

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            result.Text = "";
        }

        void OnOperatorButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            operation = b.Text;
            switch (operation)
            {
                case "/":
                    operation = "/";
                    //result.Text.Insert(result.CursorPosition, " / ");
                    result.Text += " / ";
                    break;
                case "X":
                    operation = "*";
                    //result.Text.Insert(result.CursorPosition, " x ");
                    result.Text += " x ";
                    break;
                case "-":
                    operation = "-";
                    //result.Text.Insert(result.CursorPosition, " - ");
                    result.Text += " - ";
                    break;
                case "+":
                    operation = "+";
                    //result.Text.Insert(result.CursorPosition, " + ");
                    result.Text += " + ";
                    break;
                default:
                    break;
            }

            result.CursorPosition += 3;
            //result.CursorPosition = result.Text.Length;

            operand = "";

            //value = Double.Parse(result.Text);
            //operationPressed = true;
            //equation.Text = value + " " + operation;
        }

        void OnParenthesesButtonClicked(object sender, EventArgs e)
        {

        }

        void OnDivideButtonClicked(object sender, EventArgs e)
        {

        }

        void OnDeleteButtonClicked(object sender, EventArgs e)
        {

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
                cursorPosition = result.CursorPosition;
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

        void OnTimesButtonClicked(object sender, EventArgs e)
        {

        }

        void OnMinusButtonClicked(object sender, EventArgs e)
        {

        }

        void OnPlusButtonClicked(object sender, EventArgs e)
        {

        }

        void OnSignButtonClicked(object sender, EventArgs e)
        {

        }

        void OnDotButtonClicked(object sender, EventArgs e)
        {

        }

        void OnEqualsButtonClicked(object sender, EventArgs e)
        {

        }
    }
}
