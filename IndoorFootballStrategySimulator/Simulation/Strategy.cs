using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation {
    class Strategy {

        public string Name { get; set; }
        public string Description { get; set; }
        public List<Player> Players { get; set; }

        public Strategy(string name, string description, List<Player> players) {
            Name = name;
            Description = description;
            Players = players;
        }

    }
}
