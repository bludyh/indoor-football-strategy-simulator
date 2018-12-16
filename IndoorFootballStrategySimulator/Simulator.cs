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
        public static int NumberofSimulations { get; private set; }
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
            SetFirstTarget();
        }

        private void Start_btn_Click(object sender, EventArgs e)
		{
            try {
                if (strategyPreviewWindowHome.Strategy == null || strategyPreviewWindowAway.Strategy == null)
                    throw new Exception("Please select strategies for Home and Away team!");

                if (simulationWindow.IsInitialized) {
                    SimulationWindow.EntityManager.BlueTeam.Strategy = strategyPreviewWindowHome.Strategy;
                    SimulationWindow.EntityManager.RedTeam.Strategy = strategyPreviewWindowAway.Strategy;
					
                }
                NumberofSimulations = Convert.ToInt32(tbNrofSimulations.Text);
                if (NumberofSimulations == 0)
                {
                    throw new Exception("Invalid number of simulations");
                }
                tab.Controls.Add(tabSimulation);
                tab.Controls.Remove(tabHome);
                tab.Controls.Remove(tabStrategies);
                tab.Controls.Remove(tabResults);
                tab.SelectTab(tabSimulation);
                isGameOn = true;
                Pause = false;
                timer1.Start();
                timer2.Start();
                timer2.Tick += Timer2_Tick;
                timer1.Tick += Timer1_Tick;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message, "Warning");
            }
		}

        private void Timer2_Tick(object sender, EventArgs e)
        {
            if (isGameOn && Math.Round(SimulationWindow.MatchTime.TotalMinutes) == 46)
            {
                timer1.Tick += Timer1_Tick;
            }
            if (isGameOn && Math.Round(SimulationWindow.MatchTime.TotalMinutes) == 0)
            {
                timer1.Tick += Timer1_Tick;
            }
            if (NumberofSimulations == 0)
            {
                timer2.Tick -= Timer2_Tick;
                DialogResult dialog = MessageBox.Show("END SIMULATION", "Futsal Simulation",MessageBoxButtons.OK);
                if (dialog == DialogResult.OK)
                {
                    tab.Controls.Add(tabHome);
                    tab.Controls.Add(tabStrategies);
                    tab.Controls.Add(tabResults);
                    tab.Controls.Remove(tabSimulation);
                    tab.SelectTab(tabResults);
                    RefreshStrategyLists();
                    tbNrofSimulations.Text = "";
                }
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Math.Round(SimulationWindow.MatchTime.TotalMinutes) == 45)
            {
                timer1.Tick -= Timer1_Tick;
                isGameOn = false;
                Team BlueTeam = SimulationWindow.EntityManager.BlueTeam;
                Team RedTeam = SimulationWindow.EntityManager.RedTeam;
                BlueTeam.GetFSM().ChangeState(PrepareForKickOff.Instance());
                RedTeam.GetFSM().ChangeState(PrepareForKickOff.Instance());
                SimulationWindow.MatchTime = new TimeSpan(0,45,0);
            }
            if (Math.Round(SimulationWindow.MatchTime.TotalMinutes) >= 90)
            {
                timer1.Tick -= Timer1_Tick;
                SimulationWindow.EntityManager.BlueTeam.Goal.ResetScore();
                SimulationWindow.EntityManager.RedTeam.Goal.ResetScore();
                isGameOn = false;
                NumberofSimulations--;
                Team BlueTeam = SimulationWindow.EntityManager.BlueTeam;
                Team RedTeam = SimulationWindow.EntityManager.RedTeam;
                BlueTeam.GetFSM().ChangeState(PrepareForKickOff.Instance());
                RedTeam.GetFSM().ChangeState(PrepareForKickOff.Instance());
                SimulationWindow.MatchTime = new TimeSpan(); 
            }
            matchTime.Text = Math.Round(SimulationWindow.MatchTime.TotalMinutes).ToString() + "\'";
            redTeamScore.Text = SimulationWindow.EntityManager.BlueTeam.Goal.Score.ToString();
            blueTeamScore.Text = SimulationWindow.EntityManager.RedTeam.Goal.Score.ToString();
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
                tbNrofSimulations.Text = "";
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
        private void SetFirstTarget()
        {
            foreach (Player player in SimulationWindow.EntityManager.BlueTeam.Strategy.Players)
            {
                player.Steering.Target = player.GetHomeArea(SimulationWindow.EntityManager.Field, player.Team.State).Center;
            }
            foreach (Player player in SimulationWindow.EntityManager.RedTeam.Strategy.Players)
            {
                player.Steering.Target = player.GetHomeArea(SimulationWindow.EntityManager.Field, player.Team.State).Center;
            }
        }
    }
}
