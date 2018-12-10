using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;

namespace IndoorFootballStrategySimulator.Simulation {
    [DataContract]
    class Strategy {

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public List<Player> Players { get; set; }

        public Strategy(string name, string description, List<Player> players) {
            Name = name;
            Description = description;
            Players = players;
        }

    }
}
