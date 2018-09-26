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
	public partial class SimulationProcess : Form
	{
		Thread th;
		public SimulationProcess()
		{
			InitializeComponent();
		}

		private void button8_Click(object sender, EventArgs e)
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
