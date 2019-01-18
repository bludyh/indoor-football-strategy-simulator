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
using Spire.DataExport.TXT;
using Spire.DataExport.Common;

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
        public List<Result> Results { get; set; }


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

            Results = new List<Result>();
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
                    ConcludeResults(Results);
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
                Results.Add(new Result(SimulationWindow.EntityManager.BlueTeam.Strategy, SimulationWindow.EntityManager.RedTeam.Strategy, SimulationWindow.EntityManager.RedTeam.Goal.Score, SimulationWindow.EntityManager.BlueTeam.Goal.Score));
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
                timer1.Stop();
                timer2.Stop();
                ConcludeResults(Results);
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

        // Method that displays the conclusion of a set of results
        // Assuming all results for a particular match up
        private void ConcludeResults(List<Result> results)
        {
            List<int> pointsBlue = new List<int>();
            int wins = 0;
            int draws = 0;
            int losses = 0;

            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    if (result.ScoreHomeTeam > result.ScoreAwayTeam)
                    {
                        pointsBlue.Add(3);
                        wins++;
                    }
                    else if (result.ScoreHomeTeam == result.ScoreAwayTeam)
                    {
                        pointsBlue.Add(1);
                        draws++;
                    }
                    else
                    {
                        pointsBlue.Add(0);
                        losses++;
                    }                    
                }

                string homeStrategy = Results[0].HomeTeamStrategy.Name;
                string awayStrategy = Results[0].AwayTeamStrategy.Name;

                lblHomeStrategy.Text = homeStrategy;
                lblAwayStrategy.Text = awayStrategy;
                lblWins.Text = wins.ToString();
                lblDraws.Text = draws.ToString();
                lblLosses.Text = losses.ToString();

                lblConclusion.Text = "";
                lblConclusion.Text += homeStrategy + " recorded a win rate of " + 100 * ((double)wins) / Results.Count + "% against " + awayStrategy;
            }
        }

        private void btnAllResults_Click(object sender, EventArgs e)
        {
            listViewResults.Items.Clear();
            foreach (var result in Results)
            {
                string[] row = result.ToListViewRow().ToArray();
                var listViewItem = new ListViewItem(row);
                listViewResults.Items.Add(listViewItem);
            }
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            Spire.DataExport.PDF.PDFExport PDFExport = new Spire.DataExport.PDF.PDFExport();
            PDFExport.DataSource = Spire.DataExport.Common.ExportSource.ListView;
            PDFExport.ListView = this.listViewResults;
            PDFExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView;
            PDFExport.SaveToFile("Results.pdf");
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            var txtExport = new Spire.DataExport.TXT.TXTExport();
            
            txtExport.ActionAfterExport = Spire.DataExport.Common.ActionType.OpenView;
            txtExport.DataFormats.CultureName = "en-us";
            txtExport.DataFormats.Currency = "c";
            txtExport.DataFormats.DateTime = "yyyy-M-d H:mm";
            txtExport.DataFormats.Float = "g";
            txtExport.DataFormats.Integer = "g";
            txtExport.DataFormats.Time = "H:mm";
            txtExport.DataEncoding = Spire.DataExport.Common.EncodingType.ASCII;
            txtExport.DataSource = ExportSource.ListView;
            txtExport.ListView = this.listViewResults;
            txtExport.ExportType = TextExportType.CSV;
            txtExport.FileName = "sample.csv";
            txtExport.SaveToFile();
        }
    }
}
