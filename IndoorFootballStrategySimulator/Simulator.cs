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
        public static bool isGameOn { get; set; }

        private string[] StrategyFiles {
            get {
                return Directory.GetFiles(@"Data\Strategies");
            }
        }

        static Simulator() {
            Pause = true;
            isGameOn = false;
        }

        public Simulator() {
            InitializeComponent();

            tab.Controls.Remove(tabSimulation);
            strategyPreviewWindowHome.Initialized += StrategyPreviewWindowHome_Initialized;
            strategyPreviewWindowAway.Initialized += StrategyPreviewWindowAway_Initialized;
            strategyEditingWindow.Initialized += StrategyEditingWindow_Initialized;
            simulationWindow.Initialized += SimulationWindow_Initialized;

            Directory.CreateDirectory(@"Data\Strategies");
        }

        private void RefreshStrategyLists() {
            dgvHomeStrategies.Rows.Clear();
            dgvAwayStrategies.Rows.Clear();
            dgvStrategies.Rows.Clear();

            foreach (var file in StrategyFiles) {
                var fileInfo = new FileInfo(file);
                dgvHomeStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
                dgvAwayStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
                dgvStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), fileInfo.CreationTime.ToShortDateString(), fileInfo.LastWriteTime.ToShortDateString());
            }
        }

        private void StrategyPreviewWindowHome_Initialized(object sender, EventArgs e) {
            dgvHomeStrategies.Rows.Clear();

            foreach (var file in StrategyFiles) {
                var fileInfo = new FileInfo(file);
                dgvHomeStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
            }
        }

        private void StrategyPreviewWindowAway_Initialized(object sender, EventArgs e) {
            dgvAwayStrategies.Rows.Clear();

            foreach (var file in StrategyFiles) {
                var fileInfo = new FileInfo(file);
                dgvAwayStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name));
            }
        }

        private void StrategyEditingWindow_Initialized(object sender, EventArgs e) {
            dgvStrategies.Rows.Clear();

            foreach (var file in StrategyFiles) {
                var fileInfo = new FileInfo(file);
                dgvStrategies.Rows.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), fileInfo.CreationTime.ToShortDateString(), fileInfo.LastWriteTime.ToShortDateString());
            }
        }

        private void SimulationWindow_Initialized(object sender, EventArgs e) {
            SimulationWindow.EntityManager.BlueTeam.Strategy = strategyPreviewWindowHome.Strategy;
            SimulationWindow.EntityManager.RedTeam.Strategy = strategyPreviewWindowAway.Strategy;
        }

        private void Start_btn_Click(object sender, EventArgs e)
		{
            try {
                if (strategyPreviewWindowHome.Strategy == null || strategyPreviewWindowAway.Strategy == null)
                    throw new Exception("Please select strategies for Home and Away team!");

                tab.Controls.Add(tabSimulation);
                tab.Controls.Remove(tabHome);
                tab.Controls.Remove(tabStrategies);
                tab.Controls.Remove(tabResults);
                tab.SelectTab(tabSimulation);

                Pause = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
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

		private void Abort_btn_Click(object sender, EventArgs e)
		{
            DialogResult result = MessageBox.Show("Do you want to abort simulation?","Confirmation", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                tab.Controls.Add(tabHome);
                tab.Controls.Add(tabStrategies);
                tab.Controls.Add(tabResults);
                tab.Controls.Remove(tabSimulation);
                tab.SelectTab(tabResults);
                RefreshStrategyLists();
            }
		}

        private void DgvHomeStrategies_SelectionChanged(object sender, EventArgs e) {
            //try {
                if (dgvHomeStrategies.SelectedCells.Count > 0) {
                    strategyPreviewWindowHome.LoadStrategyFromFile($@"Data\Strategies\{ dgvHomeStrategies.CurrentCell.Value }.xml");

                    pnHomeTeam.Enabled = true;
                    rbHomeOffensive.Checked = true;
                    lbHomeStrategyName.Text = strategyPreviewWindowHome.Strategy.Name;
                    lbHomeStrategyDescription.Text = strategyPreviewWindowHome.Strategy.Description;
                }
            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.Message, "Warning");
            //}
        }

        private void DgvAwayStrategies_SelectionChanged(object sender, EventArgs e) {
            //try {
                if (dgvAwayStrategies.SelectedCells.Count > 0) {
                    strategyPreviewWindowAway.LoadStrategyFromFile($@"Data\Strategies\{ dgvAwayStrategies.CurrentCell.Value }.xml");

                    pnAwayTeam.Enabled = true;
                    rbAwayOffensive.Checked = true;
                    lbAwayStrategyName.Text = strategyPreviewWindowAway.Strategy.Name;
                    lbAwayStrategyDescription.Text = strategyPreviewWindowAway.Strategy.Description;
                }
            //}
            //catch (Exception ex) {
            //    MessageBox.Show(ex.Message, "Warning");
            //}
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
                RefreshStrategyLists();
            }
        }

        private void BtnSaveStrategy_Click(object sender, EventArgs e) {
            try {
                if (string.IsNullOrWhiteSpace(tbStrategyName.Text))
                    throw new Exception("Please provide a name for the strategy");

                strategyEditingWindow.Strategy.Name = tbStrategyName.Text;
                strategyEditingWindow.Strategy.Description = rtbStrategyDescription.Text;

                strategyEditingWindow.SaveStrategyToFile($@"Data\Strategies\{ strategyEditingWindow.Strategy.Name }.xml");

                RefreshStrategyLists();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
        }

        private void BtnDiscardChanges_Click(object sender, EventArgs e) {
            DgvStrategies_SelectionChanged(sender, e);
        }

    }
}
