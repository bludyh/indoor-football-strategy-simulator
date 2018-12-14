using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Support: State<FieldPlayer>
    {
        private static readonly Support instance = new Support();
        static Support(){}
        private Support(){}
        public static Support Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            player.Steering.StartArrival(player.Steering.Target);

            player.Steering.Target = SupportCalculate.GetBestSupportingSpot();
        }
        public override void Handle(FieldPlayer player)
        {
            //if his team loses control go back home
            if (!player.Team.InControl())
            {
                player.GetFSM().ChangeState(ReturnToHomeArea.Instance());
                return;
            }

            //if the best supporting spot changes, change the steering target
            if (SupportCalculate.GetBestSupportingSpot() != player.Steering.Target)
            {
                player.Steering.Target = SupportCalculate.GetBestSupportingSpot();

                player.Steering.StartArrival(player.Steering.Target);
            }

            //if this player has a shot at the goal AND the attacker can pass
            //the ball to him the attacker should pass the ball to this player
            if (player.Team.CanShoot(player.Position, 1.2f))
            {
                player.Team.RequestPass(player);
            }


            //if this player is located at the support spot and his team still have
            //possession, he should remain still and turn to face the ball
            if (player.AtTarget())
            {
                player.Steering.StopArrival();

                //the player should keep his eyes on the ball!
                player.TrackBall();

                player.Velocity = new Vector2(0, 0);

                //if not threatened by another player request a pass
                if (!player.IsThreatened())
                {
                    player.Team.RequestPass(player);
                }
            }
        }
        public override void OnExit(FieldPlayer player)
        {
            // set supporting player to null so that the team knows it has to
            //determine a new one.
            player.Team.SupportingPlayer = null;

            player.Steering.StopArrival();
        }

        public override bool OnMessage(FieldPlayer owner, Telegram telegram)
        {
            return false;
        }
    }
}
