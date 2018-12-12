using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class ReturnToHomeRegion : State<FieldPlayer>
    {
        private static readonly ReturnToHomeRegion instance = new ReturnToHomeRegion();
        static ReturnToHomeRegion() { }
        private ReturnToHomeRegion() { }
        public static ReturnToHomeRegion Instance()
        {
            return instance;
        }
        public override void Handle(FieldPlayer owner)
        {
            var field = SimulationWindow.EntityManager.Field;

            if (Simulator.isGameOn)
            {
                //if the ball is nearer this player than any other team member  &&
                //there is not an assigned receiver && the goalkeeper does not gave
                //the ball, go chase it

                if (owner.IsClosestTeamMemberToBall()
                        && (owner.Team.ReceivingPlayer == null)
                        && !field.GoalKeeperHasBall)
                {
                    owner.GetFSM().ChangeState(ChaseBall.Instance());
                    return;
                }
            }

            //if game is on and close enough to home, change state to wait and set the 
            //player target to his current position.(so that if he gets jostled out of 
            //position he can move back to it)
            if (Simulator.isGameOn && owner.GetHomeArea(field).Inside(owner.Position, Area.AreaModifer.HalfSize))
            {
                owner.Steering.Target = owner.Position;
                owner.GetFSM().ChangeState(Idle.Instance());
            } //if game is not on the player must return much closer to the center of his
              //home region
            else if (!Simulator.isGameOn && owner.AtTarget())
            {
                owner.GetFSM().ChangeState(Idle.Instance());
            }
        }

        public override void OnEnter(FieldPlayer owner)
        {
            var field = SimulationWindow.EntityManager.Field;

            owner.Steering.StartArrival(owner.Steering.Target);

            if (!owner.GetHomeArea(field).Inside(owner.Steering.Target, Area.AreaModifer.HalfSize))
            {
                owner.Steering.Target = owner.GetHomeArea(field).Center;
            }
        }

        public override void OnExit(FieldPlayer owner)
        {
            owner.Steering.StopArrival();
        }
    }
}
