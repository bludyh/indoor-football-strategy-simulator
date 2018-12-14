using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class KickBall:State<FieldPlayer>
    {
        private static readonly KickBall instance = new KickBall();
        static KickBall(){}
        private KickBall(){}

        public static KickBall Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            //let the team know this player is controlling
            player.Team.SetControllingPlayer(player);
            // Kick Limited???
        }
        public override void Handle(FieldPlayer player)
        {
            var field = SimulationWindow.EntityManager.Field;
            var ball = SimulationWindow.EntityManager.Ball;
            //calculate the dot product of the vector pointing to the ball
            //and the player's heading
            Vector2 ToBall = Vector2.Subtract(ball.Position, player.Position);
            float dot = Vector2.Dot(player.Heading, Vector2.Normalize(ToBall));
            //cannot kick the ball if the goalkeeper is in possession or if it is 
            //behind the player or if there is already an assigned receiver. So just
            //continue chasing the ball
            if (player.Team.ReceivingPlayer != null|| field.GoalKeeperHasBall || (dot < 0))
            {
                player.GetFSM().ChangeState(ChaseBall.Instance());
                return;
            }

            /* Attempt a shot at the goal */

            //if a shot is possible, this vector will hold the position along the 
            //opponent's goal line the player should aim for.
            Vector2 BallTarget = new Vector2();
            //the dot product is used to adjust the shooting force. The more
            //directly the ball is ahead, the more forceful the kick
            float power = 3f * dot;
            //if it is determined that the player could score a goal from this position
            //OR if he should just kick the ball anyway, the player will attempt
            //to make the shot
            if (player.Team.CanShoot(ball.Position, power, BallTarget) || (SupportCalculate.RandFloat() < 0.005))
            {
                //add some noise to the kick. We don't want players who are 
                //too accurate!
                BallTarget = Ball.AddNoiseToKick(ball.Position, BallTarget);
                //this is the direction the ball will be kicked in
                Vector2 KickDirection = Vector2.Subtract(BallTarget, ball.Position);
                ball.Kick(KickDirection, power);
                //change state   
                player.GetFSM().ChangeState(Idle.Instance());
                player.FindSupport();
                return;
            }
            /* Attempt a pass to a player */
            //if a receiver is found this will point to it
            Player receiver = null;
            power = 1.5f * dot;
            //test if there are any potential candidates available to receive a pass
            if (player.IsThreatened()
                    && player.Team.FindPass(player, receiver, BallTarget, power, 120f))
            {
                //add some noise to the kick
                BallTarget = Ball.AddNoiseToKick(ball.Position, BallTarget);
                Vector2 KickDirection = Vector2.Subtract(BallTarget, ball.Position);
                ball.Kick(KickDirection, power);
                //let the receiver know a pass is coming 
                MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MESSAGE_IMMEDIATELY,
                        player,
                        receiver,
                        MessageTypes.Msg_ReceiveBall,
                        BallTarget);
                //the player should wait at his current position unless instruced
                //otherwise  
                player.GetFSM().ChangeState(Idle.Instance());
                player.FindSupport();
                return;
            } //cannot shoot or pass, so dribble the ball upfield
            else
            {
                player.FindSupport();
                player.GetFSM().ChangeState(Dribble.Instance());
            }
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
