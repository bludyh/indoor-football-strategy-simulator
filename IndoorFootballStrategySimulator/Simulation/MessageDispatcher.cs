using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndoorFootballStrategySimulator.Simulation
{
    public class MessageDispatcher
    {
        public const long SEND_MESSAGE_IMMEDIATELY = 0;

        private readonly HashSet<Telegram> priorityQueue;

        private MessageDispatcher()
        {
            priorityQueue = new HashSet<Telegram>();
        }
        private static MessageDispatcher instance;

        public static MessageDispatcher Instance()
        {
            if (instance == null)
            {
                instance = new MessageDispatcher();
            }
            return instance;
        }
        public void DispatchMessage(
           long delay,
           MovingEntity sender,
           MovingEntity receiver,
           MessageTypes message,
           object extraInfo)
        {
            if (receiver == null)
            {
                Console.WriteLine(string.Format("Warning! No receiver with Id of {0} found", receiver));
                return;
            }

            var telegram = new Telegram(0, sender, receiver, message, extraInfo);
            if (delay > 0)
            {
                var currentTime = DateTime.UtcNow.Ticks;
                telegram.DispatchTime = currentTime + delay;
                priorityQueue.Add(telegram);
            }
            else
            {
                Discharge(receiver, telegram);
            }
        }
        public void DispatchDelayedMessages()
        {
            var currentTime = DateTime.UtcNow.Ticks;

            while (priorityQueue.Count > 0 &&
                (priorityQueue.First().DispatchTime < currentTime) &&
                (priorityQueue.First().DispatchTime > 0))
            {
                var telegram = priorityQueue.First();
                var receiver = telegram.Receiver;
                priorityQueue.Remove(telegram);
            }
        }
        public void Discharge(MovingEntity receiver, Telegram telegram)
        {
            receiver.HandleMessage(telegram);
            if (!receiver.HandleMessage(telegram))
            {
                Console.WriteLine("Message not handled");
            }
        }
    }
}
