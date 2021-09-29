using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace JoeCalc
{
    public partial class MainPage : ContentPage
    {
        List<Operand> operands = new List<Operand>();
        
        public static HistoryEntry testEquation = new HistoryEntry()
        {
            equation = "5 + 7",
            isResult = false
        };
        public static HistoryEntry testResult = new HistoryEntry()
        {
            equation = "12",
            isResult = true
        };

        ObservableCollection<HistoryEntry> _HistoryList = new ObservableCollection<HistoryEntry>();
        public ObservableCollection<HistoryEntry> HistoryList { get { return _HistoryList; } }

        public void HistoryListPage()
        {
            historyBox.ItemsSource = _HistoryList;

            // ObservableCollection allows items to be added after ItemsSource
            // is set and the UI will react to changes
            _HistoryList.Add(new HistoryEntry { equation = "5 + 7", isResult = false });
            _HistoryList.Add(new HistoryEntry { equation = "12", isResult = true });
        }

        //private IList<HistoryEntry> HistoryList = new List<HistoryEntry>()
        //{
        //    testEquation,
        //    testResult
        //};
        //private ObservableCollection<HistoryEntry> _HistoryList;

        //public ObservableCollection<HistoryEntry> HistoryList {
        //    get
        //    {
        //        return _HistoryList ?? _HistoryList == new ObservableCollection<HistoryEntry>;
        //    }
        //    set
        //    {
        //        if (_HistoryList != value)
        //        {
        //            _HistoryList = value;
        //            //SetPropertyChanged();
        //        }
        //    }
        //}

        Operation op = new Operation();
        String operation = "";
        String operand = "";
        bool equalsPressed = false;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new ViewModels.HistoryViewModel();
        }

        void OnClearButtonClicked(object sender, EventArgs e)
        {
            result.CursorPosition = 0;
            result.Text = "";
            result.SetCursor = false;
            equalsPressed = false;
            runningResult.Text = "";
            operand = "";
            operation = "";
        }

        void OnOperatorButtonClicked(object sender, EventArgs e)
        {
            string str0 = result.Text;
            int cursorPosition = result.CursorPosition;
            if (equalsPressed)
            {
                equalsPressed = false;
            }
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
                    result.CursorPosition = cursorPosition;
                    result.Text = str3 + formattedOperator + str2;
                }
                else if (after == 'x' || after == '/' || after == '+' || after == '-')
                {
                    string str3 = str2.Substring(1);
                    result.CursorPosition = cursorPosition + 1;
                    result.Text = str1 + formattedOperator + str3;
                }
                else
                {
                    result.CursorPosition = cursorPosition + 1;
                    result.Text = str1 + formattedOperator + str2;
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
            
            if (equalsPressed)
            {
                operand = "(";
                result.CursorPosition = 1;
                result.Text = "(";
                equalsPressed = false;
            }
            else if (str0.Length > 0 && cursorPosition > 0)
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
                operand += parenth;
                result.CursorPosition = cursorPosition + parenthSize;
                result.Text = str3;
            }
            else if (cursorPosition == 0)
            {
                operand = "(" + operand;
                result.CursorPosition = 1;
                result.Text = "(" + str0;
            }
            else
            {
                operand = "(";
                result.CursorPosition = 1;
                result.Text = "(";
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
            else if (equalsPressed)
            {
                result.CursorPosition = 0;
                result.Text = "";
                operand = "";
                equalsPressed = false;
                operands = op.BreakDownOperation("");
            }
            else if (selectionLength > 0)
            {
                string str1 = str0.Substring(0, cursorPosition - selectionLength);
                string str2 = str0.Substring(cursorPosition);
                str0 = str1 + str2;
                result.CursorPosition = cursorPosition;
                result.Text = str0;
                operands = op.BreakDownOperation(result.Text);
                if (operands.Count > 0)
                {
                    operand = op.GetOperand(operands, cursorPosition).Number;
                }
                else
                {
                    operand = "";
                }
                UpdateRunningResult();
            }
            else
            {
                string str1 = str0.Substring(0, cursorPosition - 1);
                string str2 = str0.Substring(cursorPosition);
                str0 = str1 + str2;
                cursorPosition--;
                result.CursorPosition = cursorPosition;
                result.Text = str0;
                operands = op.BreakDownOperation(result.Text);
                if (operands.Count > 0)
                {
                    operand = op.GetOperand(operands, cursorPosition).Number;
                }
                else
                {
                    operand = "";
                }
                UpdateRunningResult();
            }
        }

        void OnNumButtonClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if ((result.Text == "") || (result.Text == "0") || equalsPressed)
            {
                operand = b.Text;
                result.CursorPosition = 1;
                equalsPressed = false;
                result.Text = b.Text;
            }
            else
            {
                int cursorPosition = result.CursorPosition;
                result.CursorPosition = cursorPosition + 1;
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

                if (before == ')')
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
                
            }
            operands = op.BreakDownOperation(result.Text);
            UpdateRunningResult();

        }

        void OnSignButtonClicked(object sender, EventArgs e)
        {
            string str0 = result.Text;
            int cursorPosition = result.CursorPosition;

            if (str0 == "" || equalsPressed)     // If Result is Empty
            {
                operand = "(-";    
                result.CursorPosition = 2;
                equalsPressed = false;
                result.Text = "(-";
                operands = op.BreakDownOperation(result.Text);
            }
            else  // If Result is Not Empty
            {
                operands = op.BreakDownOperation(str0);
                Operand currentOperand = op.GetOperand(operands, cursorPosition);
                operand = currentOperand.Number;
                //if (cursorPosition < str0.Length)       // If Cursor is not at the end
                //    operand = currentOperand.Number;    // Set operand to current operand
                int currentStartPosition = currentOperand.StartPosition;        // Set start postion to current operand's start
                if (operand == "x" || operand == "/" || operand == "+" || operand == "-")   // If operand is an operator
                {                                     
                    if (cursorPosition == str0.Length)
                    {
                        currentOperand.Number = "";
                        operand = "";
                        currentStartPosition = cursorPosition;
                    }
                    else
                    {
                        currentOperand = op.GetOperand(operands, cursorPosition + 1);
                        operand = currentOperand.Number;
                        currentStartPosition = currentOperand.StartPosition;
                    }
                }

                if (operand.Contains("-"))
                {
                    string str1 = str0.Substring(0, currentStartPosition);
                    string str2;
                    if (str0.Length >= currentStartPosition + 2)
                    {
                        str2 = str0.Substring(currentStartPosition + 2);
                    }
                    else if (str0.Length == currentStartPosition + 1)
                    {
                        str2 = str0.Substring(currentStartPosition + 1);
                    }
                    else
                    {
                        str2 = str0.Substring(currentStartPosition);
                    }
                    string str3 = str1 + str2;
                    operand = operand.Remove(0, 2);
                    cursorPosition -= 2;
                    result.CursorPosition = cursorPosition;
                    result.Text = str3;
                }
                else if (operand == "" && cursorPosition == str0.Length)
                {
                    string str1 = str0 + "(-";
                    operand = "(-";
                    cursorPosition += 2;
                    result.CursorPosition = cursorPosition;
                    result.Text = str1;
                }
                else
                {
                    string str1 = str0.Substring(0, currentStartPosition);
                    string str2 = str0.Substring(currentStartPosition);
                    string str3 = str1 + "(-" + str2;
                    operand = operand.Insert(0, "(-");
                    cursorPosition += 2;
                    result.CursorPosition = cursorPosition;
                    result.Text = str3;
                }
                operands = op.BreakDownOperation(result.Text);
                UpdateRunningResult();
            }
        }

        void OnDotButtonClicked(object sender, EventArgs e)
        {
            if (operand.Contains(".")) { }
            else if (result.Text == "" || equalsPressed)
            {
                result.CursorPosition = 2;
                result.Text = "0.";
                operand = "0.";
                equalsPressed = false;
                operands = op.BreakDownOperation(result.Text);
            }
            else
            {
                string str0 = result.Text;
                int cursorPosition = result.CursorPosition;
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
                    operand += "0.";
                    result.CursorPosition = cursorPosition + 2;
                    result.Text = str3;
                    operands = op.BreakDownOperation(result.Text);
                    UpdateRunningResult();
                }
                else
                {
                    string str1 = str0.Substring(0, cursorPosition);
                    string str2 = str0.Substring(cursorPosition);
                    string str3 = str1 + "." + str2;
                    operand += ".";
                    result.CursorPosition = cursorPosition + 1;
                    result.Text = str3;
                    operands = op.BreakDownOperation(result.Text);
                    UpdateRunningResult();
                }
            }
        }

        void OnEqualsButtonClicked(object sender, EventArgs e)
        {
            ResultWithBool resultWithBool = new ResultWithBool("", false);
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

                    if (i < equation.Length - 1 && Char.IsNumber(equation[i]))
                    {
                        if (equation[i + 1] == '(')
                        {
                            string str1 = equation.Substring(0, i + 1);
                            string str2 = equation.Substring(i + 1);
                            equation = str1 + '*' + str2;
                        }
                    }
                    if (i > 0 && Char.IsNumber(equation[i]))
                    {
                        if (equation[i - 1] == ')')
                        {
                            string str1 = equation.Substring(0, i);
                            string str2 = equation.Substring(i);
                            equation = str1 + '*' + str2;
                        }
                    }
                }

                int missingClosed = openCount - closedCount;

                for (int i = 0; i < missingClosed; i++)
                {
                    equation += ")";
                }

                operands = op.BreakDownOperation(equation);
                resultWithBool = op.SolveOperation(operands);
                string answer = resultWithBool.Result;

                if (resultWithBool.Error)
                {
                    DependencyService.Get<IMessage>().ShortAlert(answer);
                    runningResult.Text = "";
                }
                else
                {
                    operand = answer;
                    operation = "";
                    runningResult.Text = "";
                    equalsPressed = true;
                    result.CursorPosition = answer.Length;
                    result.Text = answer;
                    HistoryEntry historyEquation = new HistoryEntry()
                    {
                        equation = equation,
                        isResult = false
                    };
                    HistoryList.Add(historyEquation);
                    HistoryEntry historyResult = new HistoryEntry()
                    {
                        equation = answer,
                        isResult = true
                    };
                    HistoryList.Add(historyResult);
                }
            }
            else
            {
                string errorMsg = "Invalid format";
                DependencyService.Get<IMessage>().ShortAlert(errorMsg);
                runningResult.Text = "";
            }
        }

        void UpdateRunningResult()
        {
            ResultWithBool resultWithBool = new ResultWithBool("", false);
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

                    if (i < equation.Length - 1 && Char.IsNumber(equation[i]))
                    {
                        if (equation[i + 1] == '(')
                        {
                            string str1 = equation.Substring(0, i + 1);
                            string str2 = equation.Substring(i + 1);
                            equation = str1 + '*' + str2;
                        }
                    }
                    if (i > 0 && Char.IsNumber(equation[i]))
                    {
                        if (equation[i - 1] == ')')
                        {
                            string str1 = equation.Substring(0, i);
                            string str2 = equation.Substring(i);
                            equation = str1 + '*' + str2;
                        }
                    }
                }

                int missingClosed = openCount - closedCount;

                for (int i = 0; i < missingClosed; i++)
                {
                    equation += ")";
                }

                operands = op.BreakDownOperation(equation);
                resultWithBool = op.SolveOperation(operands);

                if (resultWithBool.Error)
                {
                    runningResult.Text = "";
                }
                else
                {
                    runningResult.Text = resultWithBool.Result;
                }
            }
            else
            {
                runningResult.Text = "";
            }
        }

        void OnHistoryButtonClicked(object sender, EventArgs e)
        {
            if (historyBox.IsVisible)
            {
                historyBox.IsVisible = false;
            }
            else
            {
                historyBox.IsVisible = true;
            }
        }

        void OnTestButtonClicked(object sender, EventArgs e)
        {
            string message = "Can't enter more than 15 digits";
            DependencyService.Get<IMessage>().ShortAlert(message);
        }
    }
}