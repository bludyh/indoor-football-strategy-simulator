using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Dribble: State<FieldPlayer>
    {
        private static readonly Dribble instance = new Dribble();
        static Dribble(){}
        private Dribble(){}
        public static Dribble Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            player.Team.SetControllingPlayer(player);
        }
        public override void Handle(FieldPlayer player)
        {
            var ball = SimulationWindow.EntityManager.Ball;

            float dot = Vector2.Dot(player.Team.Goal.Facing, player.Heading);
            //if the ball is between the player and the home goal, it needs to swivel
            // the ball around by doing multiple small kicks and turns until the player 
            //is facing in the correct direction
            if (dot < 0)
            {
                float angle = MathHelper.PiOver4 * -1
                        * SupportCalculate.Sign(player.Team.Goal.Facing,player.Heading);
                Matrix rotationMatrix = Matrix.CreateRotationZ(angle);
                SupportCalculate.TransformVector2(rotationMatrix, player.Heading);
                //this value works well whjen the player is attempting to control the
                //ball and turn at the same time
                const float KickingForce = 0.8f;
                ball.Kick(player.Heading, KickingForce);
            } //kick the ball down the field
            else
            {
                ball.Kick(player.Team.Goal.Facing,1.5f);
            }
            //the player has kicked the ball so he must now change state to follow it
            player.GetFSM().ChangeState(ChaseBall.Instance());
            return;
        }
        public override void OnExit(FieldPlayer player)
        {
            
        }

        public override bool OnMessage(FieldPlayer owner, Telegram telegram)
        {
            return false;
        }
    }
}
