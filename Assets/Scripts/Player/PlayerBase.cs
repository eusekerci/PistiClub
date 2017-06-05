using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PistiClub
{
    public abstract class PlayerBase
    {
        protected List<Card> Hand;
        public int PlayerID;
        public Transform HandRoot;

        protected PlayerBase()
        {
            MessageBus.OnEvent<RoundStartEvent>().Subscribe(evnt => OnRoundStart());
            
            MessageBus.OnEvent<PlayCardEvent>().Subscribe(evnt => OnPlayCard());
        }

        protected virtual void OnRoundStart() { }

        protected virtual void OnTurnStart() { }

        protected virtual void OnPlayCard() { }

        public virtual void TakeCard(Card newCard) { }

        public virtual void Update() { }
    }
}
