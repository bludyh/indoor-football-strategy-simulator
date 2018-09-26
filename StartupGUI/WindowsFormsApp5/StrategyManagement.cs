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
	public partial class StrategyManagement : Form
	{
		Thread th;
		public StrategyManagement()
		{
			InitializeComponent();
		}

		private void Simulation_btn_Click(object sender, EventArgs e)
		{
			this.Close();
			th = new Thread(OpenStartSimulation);
			th.SetApartmentState(ApartmentState.STA);
			th.Start();
		}
		public void OpenStartSimulation(object obj)
		{
			Application.Run(new StartSimulation());

		}
	}
}
