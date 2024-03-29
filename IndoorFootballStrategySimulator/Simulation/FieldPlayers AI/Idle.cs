﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public sealed class Idle:State<FieldPlayer>
    {
        private static readonly Idle instance = new Idle();
        static Idle(){}
        private Idle(){}
        public static Idle Instance()
        {
            return instance;
        }
        public override void OnEnter(FieldPlayer player)
        {
            if (!Simulator.isGameOn)
            {
                var field = SimulationWindow.EntityManager.Field;
                player.Steering.Target = player.GetHomeArea(field, player.Team.State).Center;
            }
        }
        public override void Handle(FieldPlayer player)
        {
            //if the player has been jostled out of position, get back in position  
            if (!player.AtTarget())
            {
                player.Steering.StartArrival(player.Steering.Target);
                return;
            }
            else
            {
                player.Steering.StopArrival();
                player.Velocity = new Vector2(0, 0);
                //the player should keep his eyes on the ball!
                player.TrackBall();
            }
            //if this player's team is controlling AND this player is not the attacker
            //AND is further up the field than the attacker he should request a pass.
            if (player.Team.InControl()
                    && (!player.IsControllingPlayer())
                    && player.IsAheadOfAttacker())
            {
                player.Team.RequestPass(player);
                return;
            }
            if (Simulator.isGameOn)
            {
                //if the ball is nearer this player than any other team member  AND
                //there is not an assigned receiver AND neither goalkeeper has
                //the ball, go chase it
                var field = SimulationWindow.EntityManager.Field;

                if (player.IsClosestTeamMemberToBall()
                        && player.Team.ReceivingPlayer == null
                        && !field.GoalKeeperHasBall)
                {
                    player.GetFSM().ChangeState(ChaseBall.Instance());

                    return;
                }
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
