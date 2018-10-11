using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator
{
    class Formation
    {
        public string Name { get; set; }
        public string Strategy { get; set; }

        public Formation(string name, string strategy)
        {
            this.Name = name;
            this.Strategy = strategy;
        }
    }
}
