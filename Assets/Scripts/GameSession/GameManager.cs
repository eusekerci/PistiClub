using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class RoundStartEvent : PcEvent { }

    public class TurnStartEvent : PcEvent { }

    public class GameManager : MonoBehaviour
    {
        public GameObject CardPrefab;

        [SerializeField]
        private Transform _p1Root;
        [SerializeField]
        private Transform _p2Root;
        [SerializeField]
        private Transform _midRoot;

        private Deck _deck;
        private List<ControllerBase> _controllers;
        private List<PlayerBase> _players;

        private List<Card> _cardsOnMid;

        private int _playerCount;

        private int _turnCounter;
        private int _maxTurn;

        void Start()
        {
            _deck = new Deck();
            _players = new List<PlayerBase>();
            _controllers = new List<ControllerBase>();
            _cardsOnMid = new List<Card>();

            _playerCount = 2;

            _players.Add(new Player(1, _p1Root));
            _controllers.Add(new PlayerController());

            _players.Add(new Player(2, _p2Root));
            _controllers.Add(new PlayerController());

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
                _cardsOnMid.Add(_deck.Draw());
                if (_midRoot.childCount > 0)
                {
                    _midRoot.GetChild(0).GetComponent<CardView>().LoadData(_cardsOnMid[i]);
                }
                else
                {
                    GameObject newCard = ObjectPool.Instance.PopFromPool(CardPrefab, false, true);
                    newCard.GetComponent<CardView>().LoadData(_cardsOnMid[i]);
                    newCard.transform.SetParent(_midRoot);
                }
            }

            OnAllHandsAreEmpty();
            MessageBus.Publish(new RoundStartEvent());
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
        }
    }
}
