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

        List<Player> players = new List<Player>();
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

                // Set players objects in order to do the tree search
                SetPlayersArray();
                SetTreeSearch(step);

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
                
            }
            finally 
            {
                // Finally close it
                excelBook.Close(0);
                excelApp.Quit();
            }
        }

        private void SetPlayersArray()
        {
            for (int i = 0; i < 300; i++)
            {
                players.Add(new Player() { 
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
                    Height = oHeight.Values[i]
                });
            }
        }

        private void SetTreeSearch(int step)
        {
            lblAsk.Text = tree.LeafQuestion(step);
            btnLeft.Text = tree.LeafLeft(step);
            btnRight.Text = tree.LeafRight(step);
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
        private void tbHeigh_ValueChanged(object sender, EventArgs e)
        {
            this.txtbHeight.Text = (this.tbHeight.Value * 2).ToString();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (canSearch < 3)
            {
                MetroFramework.MetroMessageBox.Show(this, "Selecciona al menos un atributo", "Error");
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

        private void CalculatePlayer()
        {
            pbCalculate.PerformStep();
            pbCalculate.Step = 1;
            double[] playerSum = new double[300];
            double tempSum = 0;
            for (int i = 0; i < 300; i++)
            {
                pbCalculate.PerformStep();
                tempSum = (oAtt.Values[i] * sAtt) + (oAttG.Values[i] * sAttG) + (oYds.Values[i] * sYds) + (oAvg.Values[i] * sAvg) + (oYdsG.Values[i] * sYdsG) + (oWeight.Values[i] * sWeight) + (oLng.Values[i] * sLng) + (oFirst.Values[i] * sFirst) + (oFirstPer.Values[i] * sFirstPer) + (oHeight.Values[i] * sHeight);
                playerSum[i] = tempSum;
            }

            pbCalculate.Step = 5;
            pbCalculate.PerformStep();

            tempSum = (Convert.ToDouble(this.txtbAtt.Text) * sAtt)
            + (Convert.ToDouble(this.txtbAttG.Text) * sAttG)
            + (Convert.ToDouble(this.txtbYds.Text) * sYds)
            + (Convert.ToDouble(this.txtbAvg.Text) * sAvg)
            + (Convert.ToDouble(this.txtbYdsG.Text) * sYdsG)
            + ((Convert.ToDouble(this.txtbWeight.Text) * 2) * sWeight)
            + (Convert.ToDouble(this.txtbLng.Text) * sLng)
            + (Convert.ToDouble(this.txtbFirst.Text) * sFirst)
            + (Convert.ToDouble(this.txtbFirstPer.Text) * sFirstPer)
            + ((Convert.ToDouble(this.txtbHeight.Text) * 2) * sHeight);

            //var nearest = playerSum.OrderBy(x => Math.Abs((long)x - tempSum)).First();
            int nearestIndex = Array.IndexOf(playerSum, playerSum.OrderBy(x => Math.Abs((long)x - tempSum)).First()); 
            pbCalculate.PerformStep();
            pbCalculate.PerformStep();

            ShowPlayerFound(nearestIndex);
        }

        private void ShowPlayerFound(int index)
        {
            pnlSingle.Visible = true;

            this.lblPLayerName.Text = oPlayer.Values[index];
            this.lblPlayerAttVal.Text = oAtt.Values[index].ToString();
            this.lblPlayerAttGVal.Text = oAttG.Values[index].ToString();
            this.lblPlayerYdsVal.Text = oYds.Values[index].ToString();
            this.lblPlayerAvgVal.Text = oAvg.Values[index].ToString();
            this.lblPlayerYdsGVal.Text = oYdsG.Values[index].ToString();
            this.lblPlayerWeightVal.Text = oWeight.Values[index].ToString();
            this.lblPlayerLngVal.Text = oLng.Values[index].ToString();
            this.lblPlayerFirstVal.Text = oFirst.Values[index].ToString();
            this.lblPlayerFirstPerVal.Text = oFirstPer.Values[index].ToString();
            this.lblPlayerHeightVal.Text = oHeight.Values[index].ToString();

            while (this.Height < 470)
            {
                this.Height += 10;
                Application.DoEvents();
            }

            if (grdTree.Visible == true)
            {
                grdTree.Height = 394;
            }
        }

        private void txtbAtt_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAtt.Text))
                txtbAtt.Text = "0";

            //tbAtt.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAtt.Text))));
        }

        private void txtbAttG_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAttG.Text))
                txtbAttG.Text = "0";

            //tbAttG.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAttG.Text))));
        }

        private void txtbYds_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbYds.Text))
                txtbYds.Text = "0";

            //tbYds.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbYds.Text))));
        }

        private void txtbAvg_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbAvg.Text))
                txtbAvg.Text = "0";

            //tbAvg.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbAvg.Text))));
        }

        private void txtbYdsG_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbYdsG.Text))
                txtbYdsG.Text = "0";

            //tbYdsG.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbYds.Text))));
        }

        private void txtbWeight_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbWeight.Text))
                txtbWeight.Text = "0";

            //tbWeight.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbWeight.Text))));
        }

        private void txtbLng_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbLng.Text))
                txtbLng.Text = "0";

            //tbLng.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbLng.Text))));
        }

        private void txtbFirst_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbFirst.Text))
                txtbFirst.Text = "0";

            //tbFirst.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbFirst.Text))));
        }

        private void txtbFirstPer_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbFirstPer.Text))
                txtbFirstPer.Text = "0";

            //tbFirstPer.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbFirstPer.Text))));
        }

        private void txtbHeight_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtbHeight.Text))
                txtbHeight.Text = "0";

            //tbHeight.Value = Convert.ToInt32((Math.Round(Convert.ToDouble(txtbHeight.Text))));
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            // Remove players from list
            if (step == 0)
            {
                leftOne = true;

                // Height
                players.RemoveAll(item => item.Height >= 180);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 1)
            {
                leftTwo = true;

                // Weight
                players.RemoveAll(item => item.Weight >= 220);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 2)
            {
                leftThree = true;

                // 1st %
                players.RemoveAll(item => item.FirstPer >= 30);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 3)
            {
                leftFour = true;

                // Att
                players.RemoveAll(item => item.Att >= 100);

                if (leftOne)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && leftTwo)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && rightTwo && leftThree)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && rightTwo && rightThree && leftFour)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }

            }

            if (step == 4)
            {
                leftFive = true;

                // Yds/G
                players.RemoveAll(item => item.Att >= 40);

                if (rightOne && rightTwo && rightThree && rightFour && leftFive)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }
            }

            if (step == 5)
            {
                leftSix = true;

                // Avg
                players.RemoveAll(item => item.Att >= 4);

                if (rightOne && rightTwo && rightThree && rightFour && rightFive && leftSix)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }
            }

            if (step == 6)
            {
                // Lng
                players.RemoveAll(item => item.Att >= 40);
                btnRight.Enabled = false;
                btnLeft.Enabled = false;

                ShowGrid();
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            // Remove players from list
            if (step == 0)
            {
                rightOne = true;

                // Height
                players.RemoveAll(item => item.Height <= 179);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 1)
            {
                rightTwo = true;

                // Weight
                players.RemoveAll(item => item.Weight <= 219);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 2)
            {
                rightThree = true;

                // 1st %
                players.RemoveAll(item => item.FirstPer <= 29);
                step++;
                SetTreeSearch(step);
                return;
            }

            if (step == 3)
            {
                rightFour = true;

                // Att
                players.RemoveAll(item => item.Att <= 99);

                if (leftOne)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && leftTwo)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && rightTwo && leftThree)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else if (rightOne && rightTwo && rightThree && leftFour)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }
            }

            if (step == 4)
            {
                rightFive = true;

                // Yds/G
                players.RemoveAll(item => item.Att <= 30);

                if (rightOne && rightTwo && rightThree && rightFour && leftFive)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }
            }

            if (step == 5)
            {
                rightSix = true;

                // Avg
                players.RemoveAll(item => item.Att <= 3);

                if (rightOne && rightTwo && rightThree && rightFour && rightFive && leftSix)
                {
                    btnRight.Enabled = false;
                    btnLeft.Enabled = false;

                    ShowGrid();
                }
                else { step++; SetTreeSearch(step); return; }
            }

            if (step == 6)
            {
                // Lng
                players.RemoveAll(item => item.Att <= 39);

                btnRight.Enabled = false;
                btnLeft.Enabled = false;

                ShowGrid();
            }

            step++;
            
        }

        private void ShowGrid()
        {
            grdTree.Visible = true;

            if (pnlSingle.Visible == true)
            {
                grdTree.Height = 394;
            }

            // Resize
            while (this.Width < 1230)
            {
                this.Width += 40;
                Application.DoEvents();
            }

            // Add rows
            foreach(Player player in players)
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
        }

        private void btnResetTree_Click(object sender, EventArgs e)
        {
            step = 0;
            
            leftOne = false;
            leftTwo = false;
            leftThree = false;
            leftFour = false;
            leftFive = false;
            leftSix = false;
            rightOne = false;
            rightTwo = false;
            rightThree = false;
            rightFour = false;
            rightFive = false;
            rightSix = false;

            grdTree.Rows.Clear();

            SetPlayersArray();

            btnRight.Enabled = true;
            btnLeft.Enabled = true;
            
            SetTreeSearch(step);
        }

    }
}
