using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Calculator_On_Steroids;
using Microsoft.VisualBasic;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.LinkLabel;

namespace Calculator_On_Steroids
{
    internal class Methods : Entities
    {
        public string OperatorSign(string Operation)
        {
            switch (Operation)
            {
                case "Addition":
                    return "+";
                case "Subraction":
                    return "-";
                case "Multiplication":
                    return "*";
                case "Division":
                    return "/";
                default:
                    return "";
            }
        }
        public int Calculate(int Num1, int Num2, string Operation)
        {
            switch (Operation)
            {
                case "Addition":
                    return Num1 + Num2;
                case "Subraction":
                    return Num1 - Num2;
                case "Multiplication":
                    return Num1 * Num2;
                case "Division":
                    return Num1 / Num2;
                default:
                    return 0;
            }
        }
        public decimal Calculate(decimal Num1, decimal Num2, string Operation)
        {
            switch (Operation)
            {
                case "Addition":
                    return Num1 + Num2;
                case "Subraction":
                    return Num1 - Num2;
                case "Multiplication":
                    return Num1 * Num2;
                case "Division":
                    return Num1 / Num2;
                default:
                    return 0;
            }
        }
        public ArrayList GetOperatorList(ArrayList operatorList)
        {
            operatorList.Add("Addition");
            operatorList.Add("Subraction");
            operatorList.Add("Multiplication");
            operatorList.Add("Division");
            return operatorList;
        }

        public void AppendButtons(object sender, TextBox txtDisplay)
        {
            Button button = (Button)sender;
            txtDisplay.Text = txtDisplay.Text + button.Text;
        }

        public void Memory(decimal Memory, List<string> memoryTrail, ListBox list)
        {
            try
            {
                memoryTrail.Add(Memory.ToString());
                list.Items.Clear();
                for (int i = memoryTrail.Count - 1; i >= 0; i--)
                {
                    list.Items.Add(memoryTrail[i].ToString());
                }
                //foreach (string val in memoryTrail)
                //{
                //    list.Items.Add(val);
                //}
            }
            catch (Exception ex)
            {
                MsgShow(ex.Message, MessageType.Error);
            }
        }
        public static void MsgShow(string Title, MessageType MsgType)
        {
            switch (MsgType)
            {
                case MessageType.Success:
                    MessageBox.Show(Title, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case MessageType.Stop:
                    MessageBox.Show(Title, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;

                case MessageType.Error:
                    MessageBox.Show(Title, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
            }
        }
        public static void Convert(SaveFileDialog SvFile, DataGridView DataGrid, FileTypes savefiletype)
        {
            try
            {
                SvFile.Title = "Save File";
                SvFile.Filter = Actions.Filetype(savefiletype);
                switch(savefiletype)
                {
                    case FileTypes.CSVType:
                        if (SvFile.ShowDialog() == DialogResult.OK)
                        {
                            if(DataGrid.Rows.Count == 0) 
                            {
                                MsgShow("No Data to Convert",MessageType.Stop);
                                return;
                            }

                            Actions.SavePath = SvFile.FileName.ToString();
                            if (File.Exists(Actions.SavePath) == true)
                            {
                                File.Delete(Actions.SavePath);
                            }

                            StringBuilder sb = new StringBuilder();

                            for (int i = 0; i < DataGrid.Columns.Count; i++)
                            {
                                sb.Append(DataGrid.Columns[i].HeaderText + ",");
                            }
                            sb.AppendLine();


                            for (int i = 0; i < DataGrid.Rows.Count; i++)
                            {
                                for (int j = 0; j < DataGrid.Columns.Count; j++)
                                {
                                    if (j != DataGrid.Columns.Count - 1)
                                    {
                                        sb.Append(DataGrid.Rows[i].Cells[j].Value + ",");
                                    }
                                    else
                                    {
                                        sb.Append(DataGrid.Rows[i].Cells[j].Value);
                                    }

                                }
                                sb.AppendLine();
                            }
                            using (StreamWriter writer = new StreamWriter(Actions.SavePath, false))
                            {
                                writer.WriteLine(sb.ToString());
                                MsgShow(Actions.SavePath + " Created", MessageType.Success);
                            }
                        }
                        break;
                    case FileTypes.XMLType:
                        if (SvFile.ShowDialog() == DialogResult.OK)
                        {
                            if (DataGrid.Rows.Count == 0)
                            {
                                MsgShow("No Data to Convert", MessageType.Stop);
                                return;
                            }
                            Actions.SavePath = SvFile.FileName.ToString();
                            if (File.Exists(Actions.SavePath) == true)
                            {
                                File.Delete(Actions.SavePath);
                            }
                            DataTable table = xmlDatatable(DataGrid);
                            
                            DataTable newTable = new DataTable("Row");
                            foreach (DataColumn column in table.Columns)
                            {
                                newTable.Columns.Add(column.ColumnName);
                            }
                            int rowCounter = 0;
     
                            foreach (DataRow originalRow in table.Rows)
                            {
                                DataRow newRow = newTable.NewRow();
                                //newRow["Row"] = ++rowCounter;
                                for (int i = 0; i < table.Columns.Count; i++)
                                {
                                    newRow[table.Columns[i].ColumnName] = originalRow[i];
                                }
                                newTable.Rows.Add(newRow);
                            }
                            DataSet ds = new DataSet("HistoryData");
                            ds.Tables.Add(newTable);
                            ds.WriteXml(Actions.SavePath);
                            MsgShow(Actions.SavePath + " Created", MessageType.Success);
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                MsgShow(ex.Message, MessageType.Error);
            }
        }
        public static void ImportData(OpenFileDialog OpnFile,DataGridView dgv, FileTypes openfiletype)
        {
            try
            {
                OpnFile.Title = "Open File";
                OpnFile.Filter = Actions.Filetype(openfiletype);
                OpnFile.FileName = Actions.FilePath;
                switch (openfiletype)
                {
                    case FileTypes.CSVType:
                        if (OpnFile.ShowDialog() == DialogResult.OK)
                        {
                            Actions.FilePath = OpnFile.FileName.ToString();
                            dgv.DataSource = ImportCSV(Actions.FilePath);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                MsgShow(ex.Message, MessageType.Error);
            }

        }
        private static DataTable ImportCSV(string filePath)
        {

            //reading all the lines(rows) from the file.
            string[] rows = File.ReadAllLines(filePath);
            List<string> nonEmptyLines = new List<string>();
            DataTable dtData = new DataTable();
            string[] rowValues;
            DataRow dr = dtData.NewRow();

            foreach (string strRows in rows)
            {
                if (!string.IsNullOrWhiteSpace(strRows))
                {
                    nonEmptyLines.Add(strRows);
                }
            }
            //Creating columns
            if (nonEmptyLines.Count > 0)
            {
                foreach (string strColName in nonEmptyLines[0].Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(strColName))
                    {
                        dtData.Columns.Add(strColName);
                    }
                }
            }
            for (int row = 1; row < nonEmptyLines.Count; row++)
            {
                dr = dtData.NewRow();
                rowValues = nonEmptyLines[row].Split(',');
                dr.ItemArray = rowValues;
                dtData.Rows.Add(dr);

            }
            return dtData;
        }
        private static DataTable xmlDatatable(DataGridView dataGridView)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Visible)
                {
                    dt.Columns.Add(column.Name);
                }
            }
            object[] cellValues = new object[dataGridView.Columns.Count];
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValues);
            }
            return dt;
        }
    }
}
