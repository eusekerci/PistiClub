using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public abstract class PcEvent { }

    public static class MessageBus
    {
        public static void Publish<T>(T evnt) where T : PcEvent
        {
            UniRx.MessageBroker.Default.Publish(evnt);
        }

        public static UniRx.IObservable<T> OnEvent<T>() where T : PcEvent
        {
            return UniRx.MessageBroker.Default.Receive<T>();
        }
    }
}