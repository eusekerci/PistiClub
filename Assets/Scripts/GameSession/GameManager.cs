﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PistiClub
{
    public class RoundStartEvent : PcEvent { }

    public class TurnStartEvent : PcEvent
    {
        public PlayerBase Player { get; set; }
        public Card TopCard { get; set; }
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject CardPrefab;

        [SerializeField]
        private Transform _p1Root;
        [SerializeField]
        private Transform _p2Root;
        [SerializeField]
        private Transform _midRoot;
        [SerializeField]
        private Transform _deckRoot;

        private Deck _deck;
        private List<ControllerBase> _controllers;
        private List<PlayerBase> _players;

        private List<Card> _cardsOnMid;

        private int _playerCount;

        private int _turnCounter;
        private int _maxTurn;
        private PlayerBase _yourTurn;

        void Start()
        {
            _deck = new Deck();
            _players = new List<PlayerBase>();
            _controllers = new List<ControllerBase>();
            _cardsOnMid = new List<Card>();

            _playerCount = 2;
            _turnCounter = 0;

            _players.Add(new Player(0, _p1Root));
            _controllers.Add(new PlayerController(_players[0]));

            _players.Add(new Player(1, _p2Root));
            _controllers.Add(new AIController(_players[1]));

            MessageBus.OnEvent<PlayCardEvent>().Subscribe(evnt => {
                if (evnt.Player.PlayerID == _yourTurn.PlayerID)
                {
                    _players[_turnCounter % _playerCount] = evnt.Player;
                    OnCardPlayed(evnt.Card);
                }
            });

            OnGameStart();
        }

        void Update()
        {
            for (int i = 0; i < _controllers.Count; i++)
            {
                _controllers[i].Update();
            }

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Update();
            }
        }

        private void OnGameStart()
        {
            for(int i=0; i<4; i++)
            {
                AddNewCardOnMid(_deck.Draw());
            }
            if (_deck.RemainingCount() > 0 && _deckRoot.childCount < 1)
            {
                GameObject newCard = ObjectPool.Instance.PopFromPool(CardPrefab, false, true);
                newCard.GetComponent<CardView>().LoadCardBack();
                newCard.transform.SetParent(_deckRoot);
                newCard.transform.localPosition = Vector3.zero;
            }
            else
            {
                GameObject lastCard = _deckRoot.GetChild(0).gameObject;
                ObjectPool.Instance.PushToPool(ref lastCard);
            }

            _yourTurn = _players[0];

            OnAllHandsAreEmpty();
        }

        private void OnAllHandsAreEmpty()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < _playerCount; j++)
                {
                    _players[j].TakeCard(_deck.Draw());
                }
            }

            MessageBus.Publish(new RoundStartEvent());
            MessageBus.Publish(new TurnStartEvent() { Player = _yourTurn, TopCard = _cardsOnMid[_cardsOnMid.Count - 1] });
        }

        protected void OnCardPlayed(Card newCard)
        {
            Debug.Log("GameManager: " + _yourTurn.PlayerID + " played " + newCard.ID);
            _turnCounter++;
            _yourTurn = _players[_turnCounter % _playerCount];
            AddNewCardOnMid(newCard);
            if (_turnCounter % (_playerCount * 4) == 0)
            {
                OnAllHandsAreEmpty();
            }
            else
            {
                MessageBus.Publish(new TurnStartEvent() { Player = _yourTurn, TopCard = _cardsOnMid[_cardsOnMid.Count - 1] });
            }
        }

        private void AddNewCardOnMid(Card card)
        {
            _cardsOnMid.Add(card);
            if (_midRoot.childCount > 0)
            {
                _midRoot.GetChild(0).GetComponent<CardView>().LoadData(card);
            }
            else
            {
                GameObject newCard = ObjectPool.Instance.PopFromPool(CardPrefab, false, true);
                newCard.GetComponent<CardView>().LoadData(card);
                newCard.transform.SetParent(_midRoot);
                newCard.transform.localPosition = Vector3.zero;
            }
        }
    }
}
