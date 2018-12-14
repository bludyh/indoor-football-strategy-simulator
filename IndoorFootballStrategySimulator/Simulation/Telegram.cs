using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public struct Telegram
    {
        private const double SmallestDelay = 0.25;

        public MovingEntity Sender { get; private set; }
        public MovingEntity Receiver { get; private set; }
        public MessageTypes Message { get; private set; }
        public long DispatchTime { get; set; }
        public object ExtraInfo { get; private set; }

        public Telegram(
            long time,
            MovingEntity sender,
            MovingEntity receiver,
            MessageTypes message,
            object extraInfo)
        {
            Sender = sender;
            Receiver = receiver;
            DispatchTime = time;
            Message = message;
            ExtraInfo = extraInfo;
        }
    }
}

