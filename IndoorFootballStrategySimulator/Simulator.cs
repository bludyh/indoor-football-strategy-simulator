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
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Simulation;

namespace IndoorFootballStrategySimulator {
    public partial class Simulator : Form {
		public static bool Pause { get; private set; }

        static Simulator() {
            Pause = true;
        }

        public Simulator() {
            InitializeComponent();

            strategyPreviewWindowHome.Initialized += StrategyWindow_Initialized;
            strategyPreviewWindowAway.Initialized += StrategyWindow_Initialized;
            strategyEditingWindow.Initialized += StrategyWindow_Initialized;

            Directory.CreateDirectory(@"Data\Strategies");
        }

        private void PopulateStrategyList(DataGridView dgv) {
            dgv.Rows.Clear();

            var files = Directory.GetFiles(@"Data\Strategies");

            foreach (var file in files) {
                var fileInfo = new FileInfo(file);
                switch (dgv.Name) {
                    case "dgvStrategies":
                        dgv.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), fileInfo.CreationTime.ToShortDateString(), fileInfo.LastWriteTime.ToShortDateString());
                        break;
                    default:
                        dgv.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
                        break;
                }
            }
        }

        private void StrategyWindow_Initialized(object sender, EventArgs e) {
            var strategyWindow = (StrategyWindow)sender;
            switch (strategyWindow.Name) {
                case "strategyPreviewWindowHome":
                    PopulateStrategyList(dgvHomeStrategies);
                    break;
                case "strategyPreviewWindowAway":
                    PopulateStrategyList(dgvAwayStrategies);
                    break;
                case "strategyEditingWindow":
                    PopulateStrategyList(dgvStrategies);
                    break;
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
            var radioButton = (RadioButton)sender;
            var checkedButton = radioButton.Parent.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked);
            var strategyWindow = radioButton.Parent.Controls.OfType<StrategyWindow>().FirstOrDefault();

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

                strategyEditingWindow.Strategy.Name = tbStrategyName.Text;
                strategyEditingWindow.Strategy.Description = rtbStrategyDescription.Text;

                strategyEditingWindow.SaveStrategyToFile($@"Data\Strategies\{ strategyEditingWindow.Strategy.Name }.xml");

                PopulateStrategyList(dgvStrategies);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void DgvStrategies_SelectionChanged(object sender, EventArgs e) {
            try {
                if (dgvStrategies.SelectedRows.Count > 0) {
                    strategyEditingWindow.LoadStrategyFromFile($@"Data\Strategies\{ dgvStrategies.CurrentRow.Cells["StrategyName"].Value }.xml");

                    pnStrategy.Enabled = true;
                    rbOffensive.Checked = true;
                    tbStrategyName.Text = strategyEditingWindow.Strategy.Name;
                    rtbStrategyDescription.Text = strategyEditingWindow.Strategy.Description;
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
            strategyEditingWindow.CreateNewStrategy();

            pnStrategy.Enabled = true;
            rbOffensive.Checked = true;
            tbStrategyName.Text = strategyEditingWindow.Strategy.Name;
            rtbStrategyDescription.Text = strategyEditingWindow.Strategy.Description;
            dgvStrategies.ClearSelection();
        }

        private void BtnDeleteStrategy_Click(object sender, EventArgs e) {
            if (dgvStrategies.SelectedRows.Count > 0) {
                strategyEditingWindow.ClearStrategy();
                File.Delete($@"Data\Strategies\{ dgvStrategies.CurrentRow.Cells["StrategyName"].Value }.xml");

                pnStrategy.Enabled = false;
                rbOffensive.Checked = true;
                tbStrategyName.Text = string.Empty;
                rtbStrategyDescription.Text = string.Empty;
                PopulateStrategyList(dgvStrategies);
            }
        }

        private void DgvHomeStrategies_SelectionChanged(object sender, EventArgs e) {
            try {
                if (dgvHomeStrategies.SelectedCells.Count > 0) {
                    strategyPreviewWindowHome.LoadStrategyFromFile($@"Data\Strategies\{ dgvHomeStrategies.CurrentCell.Value }.xml");

                    pnHomeTeam.Enabled = true;
                    rbHomeOffensive.Checked = true;
                    lbHomeStrategyName.Text = strategyPreviewWindowHome.Strategy.Name;
                    lbHomeStrategyDescription.Text = strategyPreviewWindowHome.Strategy.Description;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void DgvAwayStrategies_SelectionChanged(object sender, EventArgs e) {
            try {
                if (dgvAwayStrategies.SelectedCells.Count > 0) {
                    strategyPreviewWindowAway.LoadStrategyFromFile($@"Data\Strategies\{ dgvAwayStrategies.CurrentCell.Value }.xml");

                    pnAwayTeam.Enabled = true;
                    rbAwayOffensive.Checked = true;
                    lbAwayStrategyName.Text = strategyPreviewWindowAway.Strategy.Name;
                    lbAwayStrategyDescription.Text = strategyPreviewWindowAway.Strategy.Description;
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }
    }
}
