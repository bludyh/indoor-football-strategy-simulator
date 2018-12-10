﻿using Microsoft.Xna.Framework;
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
            //player.Steering.StartArrival();

            player.Steering.Target = player.Team.GetSupportSpot();
        }
        public override void Handle(FieldPlayer player)
        {
            //if his team loses control go back home
            if (!player.Team.InControl())
            {
                player.GetFSM().ChangeState(ReturnToHomeRegion.Instance());
                return;
            }

            //if the best supporting spot changes, change the steering target
            if (player.Team.GetSupportSpot() != player.Steering.Target)
            {
                player.Steering.Target = player.Team.GetSupportSpot();

                //player.Steering.StartArrival();
            }

            //if this player has a shot at the goal AND the attacker can pass
            //the ball to him the attacker should pass the ball to this player
            if (player.Team.CanShoot(player.Position, 60f))
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
                if (!player.isThreatened())
                {
                    player.Team.RequestPass(player);
                }
            }
        }
        public override void OnExit(FieldPlayer player)
        {
        }
    }
}
