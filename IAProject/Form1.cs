using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFramework.Forms;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace IAProject
{
    public partial class Form1 : MetroForm
    {
        Excel.Workbook excelBook;
        int canSearch = 0;
        short sAtt = 0;
        short sAttG = 0;
        short sYds = 0;
        short sAvg = 0;
        short sYdsG = 0;
        short sWeight = 0;
        short sLng = 0;
        short sFirst = 0;
        short sFirstPer = 0;
        short sHeight = 0;

        Attribute oPlayer;
        Attribute oAtt;
        Attribute oAttG;
        Attribute oYds;
        Attribute oAvg;
        Attribute oYdsG;
        Attribute oWeight;
        Attribute oLng;
        Attribute oFirst;
        Attribute oFirstPer;
        Attribute oHeight;

        Image pImage;
        Image tImage;

        List<Player> players = new List<Player>();
        List<Player> sPlayers = new List<Player>();
        List<Player> tPlayers = new List<Player>();

        Tree tree = new Tree();
        int step = 0;

        bool leftOne = false;
        bool leftTwo = false;
        bool leftThree = false;
        bool leftFour = false;
        bool leftFive = false;
        bool leftSix = false;
        bool rightOne = false;
        bool rightTwo = false;
        bool rightThree = false;
        bool rightFour = false;
        bool rightFive = false;
        bool rightSix = false;

        bool flag = true;

        const int MAX_DISPERTION = 100;
        const int MIN_DISPERTION = 0;

        public Form1()
        {
            InitializeComponent();

            // Start reading excel file
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook newWorkbook = excelApp.Workbooks.Add();

            string workbookPath = Path.Combine(Environment.CurrentDirectory, "nflRB2.xlsx");

            try
            {
                excelBook = excelApp.Workbooks.Open(workbookPath,
                    0, false, 5, "", "", false, Excel.XlPlatform.xlWindows, "",
                    true, false, 0, true, false, false);

                Excel.Sheets excelSheets = excelBook.Worksheets;

                string currentSheet = "Hoja1";
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);

                oPlayer = new Attribute("Name", excelWorksheet.get_Range("A2", "A301").Value);
                oAtt = new Attribute("Att", excelWorksheet.get_Range("B2", "B301").Value);
                oAttG = new Attribute("AttG", excelWorksheet.get_Range("C2", "C301").Value);
                oYds = new Attribute("Yds", excelWorksheet.get_Range("D2", "D301").Value);
                oAvg = new Attribute("Avg", excelWorksheet.get_Range("E2", "E301").Value);
                oYdsG = new Attribute("YdsG", excelWorksheet.get_Range("F2", "F301").Value);
                oWeight = new Attribute("Weight", excelWorksheet.get_Range("G2", "G301").Value);
                oLng = new Attribute("Lng", excelWorksheet.get_Range("H2", "H301").Value);
                oFirst = new Attribute("First", excelWorksheet.get_Range("I2", "I301").Value);
                oFirstPer = new Attribute("FirstPer", excelWorksheet.get_Range("J2", "J301").Value);
                oHeight = new Attribute("Height", excelWorksheet.get_Range("K2", "K301").Value);

                // Set minimum values to track bars
                this.tbAtt.Minimum = oAtt.MinInt();
                this.tbAttG.Minimum = oAttG.MinInt();
                this.tbYds.Minimum = oYds.MinInt();
                this.tbAvg.Minimum = oAvg.MinInt();
                this.tbYdsG.Minimum = oYdsG.MinInt();
                this.tbWeight.Minimum = oWeight.MinInt() / 2;
                this.tbLng.Minimum = oLng.MinInt();
                this.tbFirst.Minimum = oFirst.MinInt();
                this.tbFirstPer.Minimum = oFirstPer.MinInt();
                this.tbHeight.Minimum = oHeight.MinInt() / 2;
                this.tbDispertion.Minimum = 0;

                // Set max values to track bars
                this.tbAtt.Maximum = oAtt.MaxInt();
                this.tbAttG.Maximum = oAttG.MaxInt();
                this.tbYds.Maximum = oYds.MaxInt();
                this.tbAvg.Maximum = oAvg.MaxInt();
                this.tbYdsG.Maximum = oYdsG.MaxInt();
                this.tbWeight.Maximum = oWeight.MaxInt() / 2;
                this.tbLng.Maximum = oLng.MaxInt();
                this.tbFirst.Maximum = oFirst.MaxInt();
                this.tbFirstPer.Maximum = oFirstPer.MaxInt();
                this.tbHeight.Maximum = oHeight.MaxInt() / 2;
                this.tbDispertion.Maximum = 100;

                // Set init values
                this.tbAtt.Value = oAtt.MedianInt();
                this.tbAttG.Value = oAttG.MedianInt();
                this.tbYds.Value = oYds.MedianInt();
                this.tbAvg.Value = oAvg.MedianInt();
                this.tbYdsG.Value = oYdsG.MedianInt();
                this.tbWeight.Value = oWeight.MedianInt() / 2;
                this.tbLng.Value = oLng.MedianInt();
                this.tbFirst.Value = oFirst.MedianInt();
                this.tbFirstPer.Value = oFirstPer.MedianInt();
                this.tbHeight.Value = oHeight.MedianInt() / 2;
                this.tbDispertion.Value = 50;

                this.txtbAtt.Text = this.tbAtt.Value.ToString();
                this.txtbAttG.Text = this.tbAttG.Value.ToString();
                this.txtbYds.Text = this.tbYds.Value.ToString();
                this.txtbAvg.Text = this.tbAvg.Value.ToString();
                this.txtbYdsG.Text = this.tbYdsG.Value.ToString();
                this.txtbWeight.Text = (this.tbWeight.Value * 2).ToString();
                this.txtbLng.Text = this.tbLng.Value.ToString();
                this.txtbFirst.Text = this.tbFirst.Value.ToString();
                this.txtbFirstPer.Text = this.tbFirstPer.Value.ToString();
                this.txtbHeight.Text = (this.tbHeight.Value * 2).ToString();
                this.txtbDispertion.Text = (this.tbDispertion.Value).ToString();

            }
            finally
            {
                // Finally close it
                excelBook.Close(0);
                excelApp.Quit();
            }
        }

        private void tbAtt_ValueChanged(object sender, EventArgs e)
        {
            this.txtbAtt.Text = this.tbAtt.Value.ToString();
        }
        private void tbAttG_ValueChanged(object sender, EventArgs e)
        {
            this.txtbAttG.Text = this.tbAttG.Value.ToString();
        }
        private void tbYds_ValueChanged(object sender, EventArgs e)
        {
            this.txtbYds.Text = this.tbYds.Value.ToString();
        }
        private void tbAvg_ValueChanged(object sender, EventArgs e)
        {
            this.txtbAvg.Text = this.tbAvg.Value.ToString();
        }
        private void tbYdsG_ValueChanged(object sender, EventArgs e)
        {
            this.txtbYdsG.Text = this.tbYdsG.Value.ToString();
        }
        private void tbWeight_ValueChanged(object sender, EventArgs e)
        {
            this.txtbWeight.Text = (this.tbWeight.Value * 2).ToString();
        }
        private void tbLng_ValueChanged(object sender, EventArgs e)
        {
            this.txtbLng.Text = this.tbLng.Value.ToString();
        }
        private void tbFirst_ValueChanged(object sender, EventArgs e)
        {
            this.txtbFirst.Text = this.tbFirst.Value.ToString();
        }
        private void tbFirstPer_ValueChanged(object sender, EventArgs e)
        {
            this.txtbFirstPer.Text = this.tbFirstPer.Value.ToString();
        }
        private void tbHeight_ValueChanged(object sender, EventArgs e)
        {
            this.txtbHeight.Text = (this.tbHeight.Value * 2).ToString();
        }
        private void tbDispertion_ValueChanged(object sender, EventArgs e)
        {
            this.txtbDispertion.Text = (this.tbDispertion.Value).ToString();
        }
        private void txtbAtt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbAttG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbYds_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbAvg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbYdsG_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbWeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbLng_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbFirst_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbFirstPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txtbHeight_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (canSearch < 3)
            {
                MetroFramework.MetroMessageBox.Show(this, "Selecciona al menos tres atributos", "Error");
            }
            else
                CalculatePlayer();
        }

        private void cbAtt_Click(object sender, EventArgs e)
        {
            if (cbAtt.Checked)
            {
                sAtt = 1;
                canSearch += 1;
            }
            else
            {
                sAtt = 0;
                canSearch -= 1;
            }
        }

        private void cbAttG_Click(object sender, EventArgs e)
        {
            if (cbAttG.Checked)
            {
                sAttG = 1;
                canSearch += 1;
            }
            else
            {
                sAttG = 0;
                canSearch -= 1;
            }
        }

        private void cbYds_Click(object sender, EventArgs e)
        {
            if (cbYds.Checked)
            {
                sYds = 1;
                canSearch += 1;
            }
            else
            {
                sYds = 0;
                canSearch -= 1;
            }
        }

        private void cbAvg_Click(object sender, EventArgs e)
        {
            if (cbAvg.Checked)
            {
                sAvg = 1;
                canSearch += 1;
            }
            else
            {
                sAvg = 0;
                canSearch -= 1;
            }
        }

        private void cbYdsG_Click(object sender, EventArgs e)
        {
            if (cbYdsG.Checked)
            {
                sYdsG = 1;
                canSearch += 1;
            }
            else
            {
                sYdsG = 0;
                canSearch -= 1;
            }
        }

        private void cbWeight_Click(object sender, EventArgs e)
        {
            if (cbWeight.Checked)
            {
                sWeight = 1;
                canSearch += 1;
            }
            else
            {
                sWeight = 0;
                canSearch -= 1;
            }
        }

        private void cbLng_Click(object sender, EventArgs e)
        {
            if (cbLng.Checked)
            {
                sLng = 1;
                canSearch += 1;
            }
            else
            {
                sLng = 0;
                canSearch -= 1;
            }
        }

        private void cbFirst_Click(object sender, EventArgs e)
        {
            if (cbFirst.Checked)
            {
                sFirst = 1;
                canSearch += 1;
            }
            else
            {
                sFirst = 0;
                canSearch -= 1;
            }
        }

        private void cbFirstPer_Click(object sender, EventArgs e)
        {
            if (cbFirstPer.Checked)
            {
                sFirstPer = 1;
                canSearch += 1;
            }
            else
            {
                sFirstPer = 0;
                canSearch -= 1;
            }
        }

        private void cbHeight_Click(object sender, EventArgs e)
        {
            if (cbHeight.Checked)
            {
                sHeight = 1;
                canSearch += 1;
            }
            else
            {
                sHeight = 0;
                canSearch -= 1;
            }
        }

        private void txtbAtt_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAtt.Text))
                txtbAtt.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbAtt.Text))));
                if (Convert.ToInt32(a) > oAtt.MaxInt())
                {
                    if(cbAtt.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAtt.Text = oAtt.MaxInt().ToString();
                    tbAtt.Value = tbAtt.Maximum;
                }
                else if (Convert.ToInt32(a) < oAtt.MinInt())
                {
                    if (cbAtt.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAtt.Text = oAtt.MinInt().ToString();
                    tbAtt.Value = tbAtt.Minimum;
                }
                else
                {
                    tbAtt.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            //tbAtt.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAtt.Text))));
        }

        private void txtbAttG_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAttG.Text))
                txtbAttG.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbAttG.Text))));
                if (Convert.ToInt32(a) > oAttG.MaxInt())
                {
                    if(cbAttG.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAttG.Text = oAttG.MaxInt().ToString();
                    tbAttG.Value = tbAttG.Maximum;
                }
                else if (Convert.ToInt32(a) < oAttG.MinInt())
                {
                    if (cbAttG.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAttG.Text = oAttG.MinInt().ToString();
                    tbAttG.Value = tbAttG.Minimum;
                }
                else
                {
                    tbAttG.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbAttG.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAttG.Text))));
        }

        private void txtbYds_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbYds.Text))
                txtbYds.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbYds.Text))));
                if (Convert.ToInt32(a) > oYds.MaxInt())
                {
                    if(cbYds.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbYds.Text = oYds.MaxInt().ToString();
                    tbYds.Value = tbYds.Maximum;
                }
                else if (Convert.ToInt32(a) < oYds.MinInt())
                {
                    if (cbYds.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbYds.Text = oYds.MinInt().ToString();
                    tbYds.Value = tbYds.Minimum;
                }
                else
                {
                    tbYds.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbYds.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbYds.Text))));
        }

        private void txtbAvg_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAvg.Text))
                txtbAvg.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbAvg.Text))));
                if (Convert.ToInt32(a) > oAvg.MaxInt())
                {
                    if(cbAvg.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAvg.Text = oAvg.MaxInt().ToString();
                    tbAvg.Value = tbAvg.Maximum;
                }
                else if (Convert.ToInt32(a) < oAvg.MinInt())
                {
                    if (cbAvg.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbAvg.Text = oAvg.MinInt().ToString();
                    tbAvg.Value = tbAvg.Minimum;
                }
                else
                {
                    tbAvg.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            //tbAvg.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAvg.Text))));
        }

        private void txtbYdsG_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbYdsG.Text))
                txtbYdsG.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbYdsG.Text))));
                if (Convert.ToInt32(a) > oYdsG.MaxInt())
                {
                    if(cbYdsG.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbYdsG.Text = oYdsG.MaxInt().ToString();
                    tbYdsG.Value = tbYdsG.Maximum;
                }
                else if (Convert.ToInt32(a) < oYdsG.MinInt())
                {
                    if (cbYdsG.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbYdsG.Text = oYdsG.MinInt().ToString();
                    tbYdsG.Value = tbYdsG.Minimum ;
                }
                else
                {
                    tbYdsG.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbYdsG.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbYds.Text))));
        }

        private void txtbWeight_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbWeight.Text))
                txtbWeight.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbWeight.Text))));
                if (Convert.ToInt32(a) > oWeight.MaxInt())
                {
                    if(cbWeight.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbWeight.Text = oWeight.MaxInt().ToString();
                    tbWeight.Value = tbWeight.Maximum / 2;
                }
                else if (Convert.ToInt32(a) < oWeight.MinInt())
                {
                    if (cbWeight.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbWeight.Text = oWeight.MinInt().ToString();
                    tbWeight.Value = tbWeight.Minimum / 2;
                }
                else
                {
                    tbWeight.Value = Convert.ToInt32(a) / 2;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbWeight.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbWeight.Text))));
        }

        private void txtbLng_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbLng.Text))
                txtbLng.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbLng.Text))));
                if (Convert.ToInt32(a) > oLng.MaxInt())
                {
                    if(cbLng.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbLng.Text = oLng.MaxInt().ToString();
                    tbLng.Value = tbLng.Maximum;
                }
                else if (Convert.ToInt32(a) < oLng.MinInt())
                {
                    if (cbLng.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbLng.Text = oLng.MinInt().ToString();
                    tbLng.Value = tbLng.Minimum;
                }
                else
                {
                    tbLng.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbLng.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbLng.Text))));
        }

        private void txtbFirst_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbFirst.Text))
                txtbFirst.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbFirst.Text))));
                if (Convert.ToInt32(a) > oFirst.MaxInt())
                {
                    if(cbFirst.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbFirst.Text = oFirst.MaxInt().ToString();
                    tbFirst.Value = tbFirst.Maximum;
                }
                else if (Convert.ToInt32(a) < oFirst.MinInt())
                {
                    if (cbFirst.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbFirst.Text = oFirst.MinInt().ToString();
                    tbFirst.Value = tbFirst.Minimum;
                }
                else
                {
                    tbFirst.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            //tbFirst.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbFirst.Text))));
        }

        private void txtbFirstPer_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbFirstPer.Text))
                txtbFirstPer.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbFirstPer.Text))));
                if (Convert.ToInt32(a) > oFirstPer.MaxInt())
                {
                    if(cbFirstPer.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbFirstPer.Text = oFirstPer.MaxInt().ToString();
                    tbFirstPer.Value = tbFirstPer.Maximum;
                }
                else if (Convert.ToInt32(a) < oFirstPer.MinInt())
                {
                    if (cbFirstPer.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbFirstPer.Text = oFirstPer.MinInt().ToString();
                    tbFirstPer.Value = tbFirstPer.Minimum;
                }
                else
                {
                    tbFirstPer.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }
            //tbFirstPer.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbFirstPer.Text))));
        }

        private void txtbHeight_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbHeight.Text))
                //txtbHeight.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbHeight.Text))));
                if (Convert.ToInt32(a) > oHeight.MaxInt())
                {
                    if(cbHeight.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbHeight.Text = oHeight.MaxInt().ToString();
                    tbHeight.Value = tbHeight.Maximum / 2;
                }
                else if (Convert.ToInt32(a) < oWeight.MinInt())
                {
                    if (cbHeight.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbHeight.Text = oHeight.MinInt().ToString();
                    tbHeight.Value = tbHeight.Minimum / 2;
                }
                else
                {
                    tbHeight.Value = Convert.ToInt32(a) / 2;
                }
            }
            catch (Exception ex)
            {
                return;
            }

            //tbHeight.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbHeight.Text))));
        }

        private void txtbDispertion_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbDispertion.Text))
                txtbHeight.Text = "0";

            try
            {
                double a = ((Math.Round(Convert.ToDouble(txtbDispertion.Text))));
                if (Convert.ToInt32(a) > MAX_DISPERTION)
                {
                    if(cbDispertion.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbDispertion.Text = MAX_DISPERTION.ToString();
                    tbDispertion.Value = tbDispertion.Maximum;
                }
                else if (Convert.ToInt32(a) < MIN_DISPERTION)
                {
                    if(cbDispertion.Checked)
                        MetroFramework.MetroMessageBox.Show(this, "El valor especificado excede el limite", "Aviso");
                    txtbDispertion.Text = MIN_DISPERTION.ToString();
                    tbDispertion.Value = MIN_DISPERTION;
                }
                else
                {
                    tbDispertion.Value = Convert.ToInt32(a);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /*
         * Set the list of players with initial values**/
        private void SetPlayersArray()
        {
            players.Clear();
            for (int i = 0; i < 300; i++)
            {
                players.Add(new Player()
                {
                    Name = oPlayer.Values[i],
                    Att = oAtt.Values[i],
                    AttG = oAttG.Values[i],
                    Yds = oYds.Values[i],
                    Avg = oAvg.Values[i],
                    YdsG = oYdsG.Values[i],
                    Weight = oWeight.Values[i],
                    Lng = oLng.Values[i],
                    First = oFirst.Values[i],
                    FirstPer = oFirstPer.Values[i],
                    Height = oHeight.Values[i],
                    ImageUrl = Path.Combine(Environment.CurrentDirectory, "nfl_photos\\" + (i + 1)+ ".png")
                });
            }
        }

        /*
         * Difuse calculation start
         * **/
        private void CalculatePlayer()
        {
            SetPlayersArray();
            pbCalculate.Step = 5;
            pbCalculate.PerformStep();

            //List<Player> tempPlayers;
            pbCalculate.Step = 5;
            pbCalculate.PerformStep();

            // tempSum = user's player approach. Taking only the ones checked in consideration

            double tempSum = 0;
            try
            {
                tempSum = (Convert.ToDouble(this.txtbAtt.Text) * sAtt)
                + (Convert.ToDouble(this.txtbAttG.Text) * sAttG)
                + (Convert.ToDouble(this.txtbYds.Text) * sYds)
                + (Convert.ToDouble(this.txtbAvg.Text) * sAvg)
                + (Convert.ToDouble(this.txtbYdsG.Text) * sYdsG)
                + ((Convert.ToDouble(this.txtbWeight.Text)) * sWeight)
                + (Convert.ToDouble(this.txtbLng.Text) * sLng)
                + (Convert.ToDouble(this.txtbFirst.Text) * sFirst)
                + (Convert.ToDouble(this.txtbFirstPer.Text) * sFirstPer)
                + ((Convert.ToDouble(this.txtbHeight.Text)) * sHeight);

            }
            catch (FormatException e)
            {
                MetroFramework.MetroMessageBox.Show(this, "Verifica los atributos.\nSolo numeros y un punto decimal", "Error");
                return;
            }
            catch (OverflowException e)
            {
                MetroFramework.MetroMessageBox.Show(this, "Al parecer un atributo es demasiado grande", "Error");
                return;
            }

            foreach (Player p in players)
            {
                // Set each players custom sum
                p.SetPlayerValues(sAtt, sAttG, sYds, sAvg, sYdsG, sWeight, sLng, sFirst, sFirstPer, sHeight);
                // Substract the user's approach to the players sum
                p.CustomSum = Math.Abs(p.CustomSum - tempSum);
            }
            //var closest = players.Aggregate((x, y) => Math.Abs(x - tempSum) < Math.Abs(y - number) ? x : y);
            //int nearestIndex = Array.IndexOf(playerSum, playerSum.OrderBy(x => Math.Abs((long)x - tempSum)).First());

            // Sort the reults and take the first five
            var tPlayers = players.OrderBy(i => i.CustomSum).Take(5).ToList();

            pbCalculate.PerformStep();
            pbCalculate.PerformStep();

            ShowPlayerFound(tPlayers);
        }

        /*
         * Show the candidate players
         * **/
        private void ShowPlayerFound(List<Player> selectedPlayers)
        {
            //pnlSingle.Visible = true;
            grdDifuse.Rows.Clear();

            /*
             * If dispertion is checked
             * Remove players out of the margin selected**/
            if (cbDispertion.Checked == true)
            {
                // Set dispersation value
                double dispertion = Convert.ToDouble(this.txtbDispertion.Text);

                // Get the most far away player
                var far = 0.0;
                try { far = selectedPlayers.Last().CustomSum; }
                catch (InvalidOperationException opex) { CalculatePlayer(); }

                // Set a margin
                double margin = (dispertion * far) / 100;

                // Remove every player that is out the margin (Dispertion)
                selectedPlayers.RemoveAll(item => item.CustomSum > margin);
            }

            // Add rows

            /* 
             * Use the single display if theresonly one candidate
             * Use a grid if theres more than 1 candidate
             * **/
            //if (selectedPlayers.Count == 1)
            //{
            //    // Show the single format
            //    grdDifuse.Visible = false;
            //    ShowSinglePlayer(selectedPlayers);
            //    pbCalculate.Step = 100;
            //    pbCalculate.PerformStep();
            //}
            //else
            //{
                grdDifuse.Visible = true;
                foreach (Player player in selectedPlayers)
                {
                    grdDifuse.Rows.Add(
                        player.Name,
                        player.Att,
                        player.AttG,
                        player.Yds,
                        player.Avg,
                        player.YdsG,
                        player.Weight,
                        player.Lng,
                        player.First,
                        player.FirstPer,
                        player.Height);
                }

                sPlayers = selectedPlayers;

                pbCalculate.Step = 100;
                pbCalculate.PerformStep();

                while (this.Height < 490)
                {
                    this.Height += 10;
                    Application.DoEvents();
                }

                if (grdTree.Visible == true)
                {
                    grdTree.Height = 394;
                }
            //}
        }

        /*
         * Method that shows if theres only one candidate
         * **/
        private void ShowSinglePlayer(List<Player> selectedPlayers)
        {
            //pnlSingle.Visible = true;

            this.lblPLayerName.Text = selectedPlayers.First().Name;
            this.lblPlayerAttVal.Text = selectedPlayers.First().Att.ToString();
            this.lblPlayerAttGVal.Text = selectedPlayers.First().AttG.ToString();
            this.lblPlayerYdsVal.Text = selectedPlayers.First().Yds.ToString();
            this.lblPlayerAvgVal.Text = selectedPlayers.First().Avg.ToString();
            this.lblPlayerYdsGVal.Text = selectedPlayers.First().YdsG.ToString();
            this.lblPlayerWeightVal.Text = selectedPlayers.First().Weight.ToString();
            this.lblPlayerLngVal.Text = selectedPlayers.First().Lng.ToString();
            this.lblPlayerFirstVal.Text = selectedPlayers.First().First.ToString();
            this.lblPlayerFirstPerVal.Text = selectedPlayers.First().FirstPer.ToString();
            this.lblPlayerHeightVal.Text = selectedPlayers.First().Height.ToString();

            while (this.Height < 490)
            {
                this.Height += 10;
                Application.DoEvents();
            }

            if (grdTree.Visible == true)
            {
                grdTree.Height = 394;
            }
        }

        /*
         * Show the tree search grid**/
        private void ShowGrid()
        {
            pbCalculateTree.Step = 120;
            pbCalculateTree.PerformStep();
            grdTree.Visible = true;

            if (pnlSingle.Visible == true)
            {
                grdTree.Height = 414;
            }

            // Resize
            while (this.Width < 1230)
            {
                this.Width += 40;
                Application.DoEvents();
            }

            // Add rows
            foreach (Player player in players)
            {
                grdTree.Rows.Add(
                    player.Name,
                    player.Att,
                    player.AttG,
                    player.Yds,
                    player.Avg,
                    player.YdsG,
                    player.Weight,
                    player.Lng,
                    player.First,
                    player.FirstPer,
                    player.Height);
            }
            tPlayers = players;
        }

        // Calculate tree search algorithm event, only taking count of the selected attributes
        private void btnCalculateTree_Click(object sender, EventArgs e)
        {
            grdTree.Rows.Clear();
            SetPlayersArray();

            // Height
            if (cbHeight.Checked)
            {
                if ((Convert.ToDouble(this.txtbHeight.Text)) >= 180)
                    players.RemoveAll(item => item.Height <= 179);
                else if ((Convert.ToDouble(this.txtbHeight.Text)) <= 179)
                    players.RemoveAll(item => item.Height >= 180);
            }

            // Weight
            if (cbWeight.Checked)
            {
                if ((Convert.ToDouble(this.txtbWeight.Text)) >= 220)
                    players.RemoveAll(item => item.Weight >= 219);
                else if ((Convert.ToDouble(this.txtbWeight.Text)) <= 219)
                    players.RemoveAll(item => item.Weight >= 220);
            }

            // 1st %
            if (cbFirstPer.Checked)
            {
                if ((Convert.ToDouble(this.txtbFirstPer.Text)) >= 30)
                    players.RemoveAll(item => item.FirstPer <= 29);
                else if ((Convert.ToDouble(this.txtbFirstPer.Text)) <= 29)
                    players.RemoveAll(item => item.FirstPer >= 30);
            }

            // Att
            if (cbAtt.Checked)
            {
                if ((Convert.ToDouble(this.txtbAtt.Text)) >= 100)
                    players.RemoveAll(item => item.Att <= 99);
                else if ((Convert.ToDouble(this.txtbAtt.Text)) <= 99)
                    players.RemoveAll(item => item.Att >= 100);
            }

            // Yds/G
            if (cbYdsG.Checked)
            {
                if ((Convert.ToDouble(this.txtbYdsG.Text)) >= 40)
                    players.RemoveAll(item => item.YdsG <= 39);
                else if ((Convert.ToDouble(this.txtbYdsG.Text)) <= 39)
                    players.RemoveAll(item => item.YdsG >= 40);
            }

            // Avg
            if (cbAvg.Checked)
            {
                if ((Convert.ToDouble(this.txtbAvg.Text)) >= 4)
                    players.RemoveAll(item => item.Avg <= 3);
                else if ((Convert.ToDouble(this.txtbAvg.Text)) <= 3)
                    players.RemoveAll(item => item.Avg >= 4);
            }

            // Lng
            if (cbLng.Checked)
            {
                if ((Convert.ToDouble(this.txtbLng.Text)) >= 40)
                    players.RemoveAll(item => item.Lng <= 39);
                else if ((Convert.ToDouble(this.txtbLng.Text)) <= 39)
                    players.RemoveAll(item => item.Lng >= 40);
            }

            ShowGrid();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            pbCalculateTree.Step = -120;
            pbCalculate.Step = -120;

            pbCalculate.PerformStep();
            pbCalculateTree.PerformStep();

            // Uncheck everything        
            this.cbAtt.Checked = false;
            this.cbAttG.Checked = false;
            this.cbYds.Checked = false;
            this.cbAvg.Checked = false;
            this.cbYdsG.Checked = false;
            this.cbWeight.Checked = false;
            this.cbLng.Checked = false;
            this.cbFirst.Checked = false;
            this.cbFirstPer.Checked = false;
            this.cbHeight.Checked = false;
            this.cbDispertion.Checked = false;

            canSearch = 0;

            // Set init values -> Minimum
            this.tbAtt.Value = oAtt.MinInt();
            this.tbAttG.Value = oAttG.MinInt();
            this.tbYds.Value = oYds.MinInt();
            this.tbAvg.Value = oAvg.MinInt();
            this.tbYdsG.Value = oYdsG.MinInt();
            this.tbWeight.Value = oWeight.MinInt() / 2;
            this.tbLng.Value = oLng.MinInt();
            this.tbFirst.Value = oFirst.MinInt();
            this.tbFirstPer.Value = oFirstPer.MinInt();
            this.tbHeight.Value = oHeight.MinInt() / 2;
            this.tbDispertion.Value = MIN_DISPERTION;

            // Set init values -> Median
            //this.tbAtt.Value = oAtt.MedianInt();
            //this.tbAttG.Value = oAttG.MedianInt();
            //this.tbYds.Value = oYds.MedianInt();
            //this.tbAvg.Value = oAvg.MedianInt();
            //this.tbYdsG.Value = oYdsG.MedianInt();
            //this.tbWeight.Value = oWeight.MedianInt() / 2;
            //this.tbLng.Value = oLng.MedianInt();
            //this.tbFirst.Value = oFirst.MedianInt();
            //this.tbFirstPer.Value = oFirstPer.MedianInt();
            //this.tbHeight.Value = oHeight.MedianInt() / 2;
            //this.tbDispertion.Value = 50;
        }

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }        

        private void grdDifuse_MouseClick(object sender, MouseEventArgs e)
        {
            pnlSingle.Visible = true;
            while (this.Height < 640)
            {
                this.Height += 20;
                Application.DoEvents();
            }

            try
            {
                pImage = Image.FromFile((sPlayers[grdDifuse.CurrentCell.RowIndex].ImageUrl));
                pImage = resizeImage(pImage, new Size(100, 100));
                pbPlayerDetail.Image = pImage;
            }
            catch (ArgumentOutOfRangeException exc)
            {
                return;
            }

            lblPLayerName.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Name;
            lblPlayerAtt.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Att.ToString();
            lblPlayerAttG.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].AttG.ToString();
            lblPlayerYds.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Yds.ToString();
            lblPlayerAvg.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Avg.ToString();
            lblPlayerYdsG.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].YdsG.ToString();
            lblPlayerWeight.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Weight.ToString();
            lblPlayerLng.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Lng.ToString();
            lblPlayerFirst.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].First.ToString();
            lblPlayerFirstPer.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].FirstPer.ToString();
            lblPlayerHeight.Text = sPlayers[grdDifuse.CurrentCell.RowIndex].Height.ToString();
        }

        private void grdTree_MouseClick(object sender, MouseEventArgs e)
        {
            pnlT.Visible = true;

            while (this.Height < 640)
            {
                this.Height += 20;
                Application.DoEvents();
            }

            grdTree.Height = 430;

            try
            {
                tImage = Image.FromFile((tPlayers[grdTree.CurrentCell.RowIndex].ImageUrl));
                tImage = resizeImage(tImage, new Size(100, 100));
                pbTree.Image = tImage;
            }
            catch (ArgumentOutOfRangeException exc)
            {
                return;
            }
            catch (NullReferenceException nullex) { return; }

            lblTName.Text = tPlayers[grdTree.CurrentCell.RowIndex].Name;
            lblTAtt.Text = tPlayers[grdTree.CurrentCell.RowIndex].Att.ToString();
            lblTAttG.Text = tPlayers[grdTree.CurrentCell.RowIndex].AttG.ToString();
            lblTYds.Text = tPlayers[grdTree.CurrentCell.RowIndex].Yds.ToString();
            lblTAvg.Text = tPlayers[grdTree.CurrentCell.RowIndex].Avg.ToString();
            lblTYdsG.Text = tPlayers[grdTree.CurrentCell.RowIndex].YdsG.ToString();
            lblTWeight.Text = tPlayers[grdTree.CurrentCell.RowIndex].Weight.ToString();
            lblTLng.Text = tPlayers[grdTree.CurrentCell.RowIndex].Lng.ToString();
            lblTFirst.Text = tPlayers[grdTree.CurrentCell.RowIndex].First.ToString();
            lblTFirstPer.Text = tPlayers[grdTree.CurrentCell.RowIndex].FirstPer.ToString();
            lblTHeight.Text = tPlayers[grdTree.CurrentCell.RowIndex].Height.ToString();
        }        
    }
}