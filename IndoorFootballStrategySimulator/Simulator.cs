using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IndoorFootballStrategySimulator.Simulation;

namespace IndoorFootballStrategySimulator {
    public partial class Simulator : Form {
		public static bool pause = false;
		public static bool abort = false;

        private List<Formation> listStrategy = new List<Formation>();
        private List<Formation> listStrategyAdded = new List<Formation>();
        private Formation formation;
        public static Random Random { get; private set; }

        public Simulator() {
            InitializeComponent();
			
         Random = new Random();

            formation = new Formation("", "2-1-1");
            listStrategy.Add(formation);
            formation = new Formation("", "3-0-1");
            listStrategy.Add(formation);
            formation = new Formation("", "2-0-2");
            listStrategy.Add(formation);
            formation = new Formation("", "2-2-0");
            listStrategy.Add(formation);
            formation = new Formation("", "1-2-1");
            listStrategy.Add(formation);

            pictureBox2.Image = Properties.Resources.field;

            //Show list of Formation in the combobox
            ShowListOfFormation();

            dataGridView1.Columns.Add("Home Strategy", "Home Strategy");
            dataGridView1.Columns.Add("Selection", "Name");

            //Set full screen
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

        }

        //Show List of Formation in the combobox
        private void ShowListOfFormation()
        {

            foreach (Formation f in listStrategy)
            {
                cbx_formation.Items.Add(f.Strategy);
            }
        }

        private void Pause_btn_Click(object sender, EventArgs e)
		{
			if (pause == false)
			{
				pause = true;
				Pause_btn.BackColor = System.Drawing.Color.Yellow;
			}
			else
			{
				pause = false;
				Pause_btn.BackColor = System.Drawing.Color.Orange;
			}

		}

		private void Start_btn_Click(object sender, EventArgs e)
		{
			tab_ctrl.SelectTab(Simulation_tb);
			Control homeTab = Home_tab;
            Control strategyTab = Strategies_tab;
            homeTab.Dispose();
            strategyTab.Dispose();
		}

	

		private void Abort_btn_Click(object sender, EventArgs e)
		{
			Simulator.abort = true;
			tab_ctrl.SelectTab(result_tab);
			
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\1.jpg");
            pictureBox1.Image = Properties.Resources._211;
        }

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\2.jpg");
            pictureBox1.Image = Properties.Resources._301;
        }

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\3.jpg");
            pictureBox1.Image = Properties.Resources._202;
        }

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\4.jpg");
            pictureBox1.Image = Properties.Resources._220;
        }

		private void radioButton8_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\5.jpg");
            pictureBox1.Image = Properties.Resources._301;
        }

		private void radioButton7_CheckedChanged(object sender, EventArgs e)
		{
            //pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\5.jpg");
            pictureBox1.Image = Properties.Resources._121;
        }

        private void btn_saveStrategy_Click(object sender, EventArgs e)
        {

            if (cbx_formation.SelectedIndex > -1 && txt_formationName.Text != "")
            {
                string str = listStrategy[cbx_formation.SelectedIndex].Strategy;
                formation = new Formation(str, txt_formationName.Text);
                listStrategyAdded.Add(formation);
                dataGridView1.Rows.Add(str, txt_formationName.Text);

                cbx_formation.Text = "";
                txt_formationName.Text = "";
            }
            else
            {
                MessageBox.Show("Select formation and input Name!");
            }
        }

        private void btn_discardChange_Click(object sender, EventArgs e)
        {
            cbx_formation.Text = txt_formationName.Text = "";
            pictureBox2.Image = Properties.Resources.field;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            pictureBox2.Image = Properties.Resources.field;
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(this.dataGridView1.SelectedRows[0].Index);
                pictureBox2.Image = Properties.Resources.field;
            }
        }

        private void cbx_formation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = listStrategy[cbx_formation.SelectedIndex].Strategy;
            if (str == "2-1-1")
            {
                pictureBox2.Image = Properties.Resources._211;
            }
            if (str == "1-2-1")
            {
                pictureBox2.Image = Properties.Resources._121;
            }
            
            if (str == "3-0-1")
            {
                pictureBox2.Image = Properties.Resources._103;
            }
            if (str == "2-2-0")
            {
                pictureBox2.Image = Properties.Resources._220;
            }
            if (str == "2-0-2")
            {
                pictureBox2.Image = Properties.Resources._202;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells[0].Value.ToString() == "2-1-1")
            {
                pictureBox2.Image = Properties.Resources._211;
            }
            if (dataGridView1.SelectedCells[0].Value.ToString() == "1-2-1")
            {
                pictureBox2.Image = Properties.Resources._121;
            }
            if (dataGridView1.SelectedCells[0].Value.ToString() == "3-0-1")
            {
                pictureBox2.Image = Properties.Resources._301;
            }
            if (dataGridView1.SelectedCells[0].Value.ToString() == "2-2-0")
            {
                pictureBox2.Image = Properties.Resources._220;
            }
            if (dataGridView1.SelectedCells[0].Value.ToString() == "2-0-2")
            {
                pictureBox2.Image = Properties.Resources._202;
            }
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
