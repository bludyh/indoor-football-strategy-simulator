using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class GlobalGoalKeeperState : State<GoalKeeper>
    {
        private static readonly GlobalGoalKeeperState instance = new GlobalGoalKeeperState();
        static GlobalGoalKeeperState() { }
        private GlobalGoalKeeperState() { }
        public static GlobalGoalKeeperState Instance()
        {
            return instance;
        }
        public override void Handle(GoalKeeper owner)
        {

        }

        public override void OnEnter(GoalKeeper owner)
        {

        }

        public override void OnExit(GoalKeeper owner)
        {

        }

        public override bool OnMessage(GoalKeeper owner, Telegram telegram)
        {
            switch (telegram.Message)
            {
                case MessageTypes.Msg_GoHome:
                    {
                        owner.GetHomeArea(SimulationWindow.EntityManager.Field,owner.Team.State);
                        owner.GetFSM().ChangeState(ReturnHome.Instance());
                    }

                    break;

                case MessageTypes.Msg_ReceiveBall:
                    {
                        owner.GetFSM().ChangeState(InterceptBall.Instance());
                    }

                    break;

            }//end switch

            return false;
        }
    }
}
