using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using Calculator_On_Steroids;
using static Calculator_On_Steroids.Entities;
using static Calculator_On_Steroids.Methods;

namespace Calculator_On_Steroids
{
    public partial class frmCalculator : Form
    {
        Methods _methods = new Methods();
        ArrayList _OperatorList = new ArrayList();
        int HistID = 1;

        decimal Result1;
        decimal Result2;
        string OperatorSign = "";
        string Operator = "";
        bool reCalcu;

        private decimal memory = 0;
        private List<string> memoryTrail = new List<string>();
        DataTable dt = new DataTable();
        DataRow row; 
        public frmCalculator()
        {

            InitializeComponent();
            DisableTabIndex(this);
            label1.Text = string.Empty;
        }

        #region PopulateOperatorList
        private void PopulateOperatorList(ComboBox OperatorBox)
        {
            try
            {
                _OperatorList = _methods.GetOperatorList(_OperatorList);
                foreach (var List in _OperatorList)
                {
                    OperatorBox.Items.Add(List);
                }
            }
            catch (Exception ex)
            {
                MsgShow(ex.Message, Entities.MessageType.Error);
            }
        }
        #endregion

        #region [DisableTabIndex]
        private void DisableTabIndex(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                control.TabStop = false;

                if (control.HasChildren)
                {
                    DisableTabIndex(control);
                }
            }
        }
        #endregion

        #region [HistoryTrail]
        private void AddHistory(DataGridView dataGridView, int Hist_ID, string Hist_Action, string Hist_Value)
        {
            dataGridView.DataSource = HistoryTrail(Hist_ID, Hist_Action, Hist_Value);
        }

        private DataTable HistoryTrail(int Hist_ID, string Hist_Action, string Hist_Value)
        {
            if (dgvHistory.Columns.Count > 0)
            {
                row = dt.NewRow();
                row[0] = Hist_ID;
                row[1] = Hist_Action;
                row[2] = Hist_Value;
                dt.Rows.Add(row);
            }
            else
            {
                dt.Columns.Add("Hist_ID", typeof(int));
                dt.Columns.Add("Hist_Action", typeof(string));
                dt.Columns.Add("Hist_Value", typeof(string));
                row = dt.NewRow();
                row[0] = Hist_ID;
                row[1] = Hist_Action;
                row[2] = Hist_Value;
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion


        private void Operation()
        {
            Result1 = decimal.Parse(txtDisplay.Text);
            Operator = cmbOperation.Text;
            OperatorSign = _methods.OperatorSign(Operator);
            label1.Text = Result1.ToString() + " " + OperatorSign;
            txtDisplay.Text = "0";
            reCalcu = false;
        }

        private void Clear()
        {
            txtDisplay.Text = "0";
            Result1 = 0;
            Result2 = 0;
            label1.Text = string.Empty;
            cmbOperation.SelectedIndex = -1;
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text == "0")
            {
                txtDisplay.Clear();
            }
            _methods.AppendButtons(sender, txtDisplay);
        }

        private void txtDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.')
            {
                if (txtDisplay.Text.Contains("."))
                {
                    e.Handled = true;
                }
            }
        }

        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Contains("."))
            {
                return;
            }
            _methods.AppendButtons(sender, txtDisplay);

        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDisplay.Text.StartsWith("-"))
                {
                    txtDisplay.Text = txtDisplay.Text.Substring(1);
                }
                else if (!string.IsNullOrEmpty(txtDisplay.Text) && decimal.Parse(txtDisplay.Text) != 0)
                {
                    txtDisplay.Text = "-" + txtDisplay.Text;
                }
            }
            catch (Exception ex)
            {
                MsgShow(ex.Message, Entities.MessageType.Error);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtDisplay.Text = "0";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                if (reCalcu == false)
                {
                    if (Result1 != 0 | Operator != "")
                    {
                        Result2 = decimal.Parse(txtDisplay.Text);
                        label1.Text = Result1.ToString() + " " + OperatorSign + Result2.ToString() + " = ";
                        txtDisplay.Text = _methods.Calculate(Result1, Result2, Operator).ToString();
                        AddHistory(dgvHistory, HistID++, Operator, txtDisplay.Text);
                    }
                    reCalcu = true;
                }
                else
                {
                    if (Result2 != 0 | Operator != "")
                    {
                        label1.Text = txtDisplay.Text + " " + OperatorSign + Result2.ToString() + " = ";
                        txtDisplay.Text = _methods.Calculate(decimal.Parse(txtDisplay.Text), Result2, Operator).ToString();
                        AddHistory(dgvHistory, HistID++, Operator, txtDisplay.Text);
                    }
                }
            }
            catch(Exception ex) 
            {
                MsgShow(ex.Message, MessageType.Error);
            }

        }

        private void frmCalculator_Load(object sender, EventArgs e)
        {
            PopulateOperatorList(cmbOperation);
        }

        private void cmbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Result1 = decimal.Parse(txtDisplay.Text);
                if (Result1 != 0)
                {
                    Operator = cmbOperation.Text;
                    OperatorSign = _methods.OperatorSign(Operator);
                    label1.Text = Result1.ToString() + " " + OperatorSign;
                    reCalcu = false;

                }
                else
                {
                    Operator = cmbOperation.Text;
                    OperatorSign = _methods.OperatorSign(Operator);
                    label1.Text = Result1.ToString() + " " + OperatorSign;
                    txtDisplay.Text = "0";
                    reCalcu = false;
                }
            }
            catch(Exception ex)
            {
                MsgShow(ex.Message,MessageType.Error);
            }

        }

        private void frmCalculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar.ToString())
            {
                case "0":
                    btnZero.PerformClick();
                    break;
                case "1":
                    btnOne.PerformClick();
                    break;
                case "2":
                    btnTwo.PerformClick();
                    break;
                case "3":
                    btnThree.PerformClick();
                    break;
                case "4":
                    btnFour.PerformClick();
                    break;
                case "5":
                    btnFive.PerformClick();
                    break;
                case "6":
                    btnSix.PerformClick();
                    break;
                case "7":
                    btnSeven.PerformClick();
                    break;
                case "8":
                    btnEight.PerformClick();
                    break;
                case "9":
                    btnNine.PerformClick();
                    break;
                case ".":
                    btnDecimal.PerformClick();
                    break;
                case "+":
                    cmbOperation.SelectedIndex = 0;
                    break;
                case "-":
                    cmbOperation.SelectedIndex = 1;
                    break;
                case "*":
                    cmbOperation.SelectedIndex = 2;
                    break;
                case "/":
                    cmbOperation.SelectedIndex = 3;
                    break;
                case "=":
                    btnEquals.PerformClick();
                    break;
                case "\r":
                    btnEquals.PerformClick();
                    break;
                case "\n'":
                    btnEquals.PerformClick();
                    break;
                case "\b":
                    btnBackSpace.PerformClick();
                    break;
                default:
                    break;
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtDisplay.Text);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string copiedText = Clipboard.GetText();
            int intValue;
            decimal decimalValue;
            if (int.TryParse(copiedText, out intValue))
            {
                txtDisplay.Text = intValue.ToString();
            }
            else if (decimal.TryParse(copiedText, out decimalValue))
            {
                txtDisplay.Text = decimalValue.ToString();
            }
            else
            {
                MsgShow("The copied text is not a valid integer or decimal value.", MessageType.Stop);
            }
        }

        private void btnMemPlus_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            decimal value = decimal.Parse(txtDisplay.Text);
            if (button.Text == "M+")
            {
                memory += value;
                _methods.Memory(memory, memoryTrail, lstMemoryTrail);
                AddHistory(dgvHistory, HistID++, button.Text, memory.ToString());
                Clear();
            }
            else if (button.Text == "M-")
            {
                memory -= value;
                _methods.Memory(memory, memoryTrail, lstMemoryTrail);
                AddHistory(dgvHistory, HistID++, button.Text, memory.ToString());
                Clear();
            }
            else if (button.Text == "MR")
            {
                txtDisplay.Text = memory.ToString();
                AddHistory(dgvHistory, HistID++, button.Text, memory.ToString());
            }
            else if (button.Text == "MC")
            {
                memory = 0;
                memoryTrail.Clear();
                lstMemoryTrail.Items.Clear();
                AddHistory(dgvHistory, HistID++, button.Text, "0");
            }
            

        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            int lastCharPostion = txtDisplay.TextLength - 1;
            if (txtDisplay.TextLength == 1 || (txtDisplay.Text.Length == 2 && txtDisplay.Text.Contains("-")))
            {
                txtDisplay.Text = "0";
            }
            else
            {
                txtDisplay.Text = txtDisplay.Text.Remove(lastCharPostion, 1);
            }
        }

        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Convert(Actions.SFD, dgvHistory, FileTypes.CSVType);

        }

        private void xmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Convert(Actions.SFD, dgvHistory, FileTypes.XMLType);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportData(Actions.OFD, dgvHistory, FileTypes.CSVType);
        }
    }
}