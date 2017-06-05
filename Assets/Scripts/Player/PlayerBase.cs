using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace PistiClub
{
    public class PlayCardEvent : PcEvent
    {
        public PlayerBase Player { get; set; }
        public Card Card { get; set; }
    }

    public abstract class PlayerBase
    {
        protected List<Card> Hand;
        public int PlayerID;
        public Transform HandRoot;
        public bool IsMyTurn;

        protected PlayerBase()
        {
            IsMyTurn = false;

            MessageBus.OnEvent<RoundStartEvent>().Subscribe(evnt => OnRoundStart());
            MessageBus.OnEvent<TurnStartEvent>().Subscribe(evnt =>
            {
                if (evnt.Player.PlayerID == PlayerID)
                {
                    IsMyTurn = true;
                    OnTurnStart();
                }
                else
                {
                    IsMyTurn = false;
                }
            });
            MessageBus.OnEvent<PlayCardCommand>().Subscribe(evnt => {
                if (evnt.Player.PlayerID == PlayerID)
                {
                    OnPlayCard(evnt.Card);
                }
            });
        }

        protected virtual void OnRoundStart() { }

        protected virtual void OnTurnStart() { }

        protected virtual void OnPlayCard(Card playedCard){ }

        public virtual void TakeCard(Card newCard) { }

        public virtual void Update() { }
    }
}
