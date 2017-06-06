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
        public List<Card> Hand;
        public int PlayerID;
        public Transform HandRoot;
        public bool IsMyTurn;
        protected int _score;
        protected bool _isAI;

        protected PlayerBase()
        {
            IsMyTurn = false;

            MessageBus.OnEvent<RoundStartEvent>().Subscribe(evnt => OnRoundStart());
            MessageBus.OnEvent<TurnStartEvent>().Where(evnt=> evnt.Player.PlayerID == PlayerID).Subscribe(evnt =>
            {
                IsMyTurn = true;
                OnTurnStart();
            });
            MessageBus.OnEvent<PlayCardCommand>().Where(evnt => evnt.Player.PlayerID == PlayerID).Subscribe(evnt => {
                OnPlayCard(evnt.Card);
            });
            MessageBus.OnEvent<PlayerGotScoreEvent>().Where(evnt => evnt.Player.PlayerID == PlayerID).Subscribe(evnt =>
            {
                _score = evnt.Score;
            });
        }

        protected virtual void OnRoundStart() { }

        protected virtual void OnTurnStart() { }

        protected virtual void OnPlayCard(Card playedCard){ }

        public virtual void TakeCard(Card newCard) { }

        public virtual void Update() { }
    }
}
