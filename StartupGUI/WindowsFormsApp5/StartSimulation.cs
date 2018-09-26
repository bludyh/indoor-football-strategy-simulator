using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp5
{
	
	public partial class StartSimulation : Form
	{
		Thread th;
		public StartSimulation()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
			th = new Thread(OpenSimulationProcess);
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}
		public void OpenSimulationProcess(object obj)
		{
			Application.Run(new SimulationProcess());

		}

		private void Strategies_btn_Click(object sender, EventArgs e)
		{
			this.Close();
			th = new Thread(OpenStrategyMng);
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}
		public void OpenStrategyMng(object obj)
		{
			Application.Run(new StrategyManagement());

		}
	}
}
