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
		public static bool Pause { get; private set; }

        static Simulator() {
            Pause = true;
        }

        public Simulator() {
            InitializeComponent();

            dataGridView1.Columns.Add("Home Strategy", "Home Strategy");
            dataGridView1.Columns.Add("Selection", "Name");

        }

        private void Pause_btn_Click(object sender, EventArgs e)
		{
			if (Pause == false)
			{
				Pause = true;
				Pause_btn.BackColor = System.Drawing.Color.Yellow;
                Pause_btn.Text = "Resume Simulation";
			}
			else
			{
				Pause = false;
				Pause_btn.BackColor = System.Drawing.Color.Orange;
                Pause_btn.Text = "Pause Simulation";
			}

        }

        private void Start_btn_Click(object sender, EventArgs e)
		{
            Pause = false;
			tab_ctrl.SelectTab(Simulation_tb);
		}

	

		private void Abort_btn_Click(object sender, EventArgs e)
		{
            DialogResult result = MessageBox.Show("Do you want to abort simulation?","Confirmation", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                tab_ctrl.SelectTab(result_tab);
            }
		}

        private void RadioButtons_CheckedChanged(object sender, EventArgs e) {
            var checkedButton = pnStrategy.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            switch (checkedButton.Text) {
                case "Offensive":
                    strategyWindow.TeamState = TeamState.OFFENSIVE;
                    break;
                case "Defensive":
                    strategyWindow.TeamState = TeamState.DEFENSIVE;
                    break;
            }
        }
    }
}
