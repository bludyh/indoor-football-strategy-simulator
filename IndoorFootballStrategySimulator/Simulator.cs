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
		public static Random Random { get; private set; }

        public Simulator() {
            InitializeComponent();
			
         Random = new Random();
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
			Control x = Home_tab;
			x.Dispose();
		}

	

		private void Abort_btn_Click(object sender, EventArgs e)
		{
			Simulator.abort = true;
			tab_ctrl.SelectTab(result_tab);
			
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
		pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\1.jpg");
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
		pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\2.jpg");
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\3.jpg");
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\4.jpg");
		}

		private void radioButton8_CheckedChanged(object sender, EventArgs e)
		{
		pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\5.jpg");
		}

		private void radioButton7_CheckedChanged(object sender, EventArgs e)
		{
		pictureBox1.Image = Image.FromFile(@"E:\1-Year 2\semester 2\ProCp\Git\indoor-football-strategy-simulator\IndoorFootballStrategySimulator\strategy_pictures\5.jpg");
		}
	}
}
