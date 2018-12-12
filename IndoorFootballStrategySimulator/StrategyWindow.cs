using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Forms.Controls;
using IndoorFootballStrategySimulator.Simulation;

namespace IndoorFootballStrategySimulator {
    abstract class StrategyWindow : ExtendedUpdateWindow {

        protected Field field;
        protected List<Goal> goals;
        protected Strategy strategy;
        protected TeamState teamState;

        public virtual Strategy Strategy { get; protected set; }
        public TeamState TeamState {
            get { return teamState; }
            set => SetTeamState(value);
        }

        protected override void Initialize() {
            base.Initialize();
        }

        private void SetTeamState(TeamState teamState) {
            this.teamState = teamState;

            if (Strategy == null)
                return;

            foreach (var player in Strategy.Players) {
                if (player is FieldPlayer fieldPlayer)
                    fieldPlayer.Position = fieldPlayer.GetHomeArea(field, teamState).Center;
            }
        }

        public abstract void LoadStrategyFromFile(string fileName);

        public void ClearStrategy() {
            Strategy = null;
        }

    }
}
