using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
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

            Directory.CreateDirectory(@"Data\Strategies");

            GetStrategyFiles();
        }

        private void GetStrategyFiles() {
            dgvStrategies.Rows.Clear();

            var files = Directory.GetFiles(@"Data\Strategies");

            foreach (var file in files) {
                var fileInfo = new FileInfo(file);
                dgvStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), fileInfo.CreationTime.ToShortDateString(), fileInfo.LastWriteTime.ToShortDateString());
            }
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

        private void BtnSaveStrategy_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrWhiteSpace(tbStrategyName.Text))
                    throw new Exception("Please provide a name for the strategy");

                strategyWindow.Strategy.Name = tbStrategyName.Text;
                strategyWindow.Strategy.Description = rtbStrategyDescription.Text;

                strategyWindow.SaveStrategyToFile($@"Data\Strategies\{ strategyWindow.Strategy.Name }.xml");

                GetStrategyFiles();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void DgvStrategies_SelectionChanged(object sender, EventArgs e) {
            try {
                if (dgvStrategies.SelectedRows.Count > 0) {
                    strategyWindow.LoadStrategyFromFile($@"Data\Strategies\{ dgvStrategies.CurrentRow.Cells["StrategyName"].Value }.xml");

                    pnStrategy.Enabled = true;
                    rbOffensive.Checked = true;
                    tbStrategyName.Text = strategyWindow.Strategy.Name;
                    rtbStrategyDescription.Text = strategyWindow.Strategy.Description;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void BtnDiscardChanges_Click(object sender, EventArgs e) {
            DgvStrategies_SelectionChanged(sender, e);
        }

        private void BtnNewStrategy_Click(object sender, EventArgs e) {
            strategyWindow.CreateNewStrategy();

            pnStrategy.Enabled = true;
            rbOffensive.Checked = true;
            tbStrategyName.Text = strategyWindow.Strategy.Name;
            rtbStrategyDescription.Text = strategyWindow.Strategy.Description;
            dgvStrategies.ClearSelection();
        }

        private void BtnDeleteStrategy_Click(object sender, EventArgs e) {
            if (dgvStrategies.SelectedRows.Count > 0) {
                strategyWindow.DeleteStrategy();
                File.Delete($@"Data\Strategies\{ dgvStrategies.CurrentRow.Cells["StrategyName"].Value }.xml");

                pnStrategy.Enabled = false;
                rbOffensive.Checked = true;
                tbStrategyName.Text = string.Empty;
                rtbStrategyDescription.Text = string.Empty;
                GetStrategyFiles();
            }
        }

    }
}
