using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PistiClub
{
    public abstract class PlayerBase
    {
        protected List<Card> Hand;
        public string PlayerID;

        protected PlayerBase()
        {
            MessageBus.OnEvent<TurnStartEvent>().Subscribe(evnt => OnTurnStart());
            MessageBus.OnEvent<PlayCardEvent>().Subscribe(evnt => OnPlayCard());
        }

        protected virtual void OnTurnStart() { }

        protected virtual void OnPlayCard() { }
    }
}
