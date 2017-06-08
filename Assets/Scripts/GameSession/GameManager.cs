using System.Collections;
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

    public class PlayerGotScoreEvent : PcEvent
    {
        public PlayerBase Player { get; set; }
        public int Score { get; set; }
    }

    public class GameEndEvent : PcEvent { }

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
        private bool _isGameRunning = true;
        private int _roundScore;

        void Start()
        {
            _deck = new Deck();
            _players = new List<PlayerBase>();
            _controllers = new List<ControllerBase>();
            _cardsOnMid = new List<Card>();

            _playerCount = 2;
            _turnCounter = 0;
            _roundScore = 0;

            _players.Add(new Player(0, _p1Root));
            _controllers.Add(new PlayerController(_players[0]));

            _players.Add(new Player(1, _p2Root, true));
            _controllers.Add(new AIController(_players[1]));

            MessageBus.OnEvent<PlayCardEvent>().Where(evnt => evnt.Player.PlayerID == _yourTurn.PlayerID).Subscribe(evnt => 
            {
                    _players[_turnCounter % _playerCount] = evnt.Player;
                    OnCardPlayed(evnt.Card);
            });

            MessageBus.OnEvent<GameEndEvent>().Subscribe(evnt =>
            {
                _isGameRunning = false;
            });

            OnGameStart();
        }

        void Update()
        {
            if(!_isGameRunning)
            {
                return;
            }
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
                PutCardOnMid(_deck.Draw());
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
                    Card newCard = _deck.Draw();
                    if(newCard == null)
                    {
                        MessageBus.Publish(new GameEndEvent());
                        return;
                    }
                    _players[j].TakeCard(newCard);
                }
            }

            MessageBus.Publish(new RoundStartEvent());
            MessageBus.Publish(new TurnStartEvent() { Player = _yourTurn, TopCard = _cardsOnMid.Count > 0 ? _cardsOnMid[_cardsOnMid.Count - 1] : null });
        }

        private void OnCardPlayed(Card newCard)
        {
            Debug.Log("GameManager: " + _yourTurn.PlayerID + " played " + newCard.ID);
            PutCardOnMid(newCard);
            CheckForScore();
            _turnCounter++;
            _yourTurn = _players[_turnCounter % _playerCount];
            if (_turnCounter % (_playerCount * 4) == 0)
            {
                OnAllHandsAreEmpty();
            }
            else
            {
                MessageBus.Publish(new TurnStartEvent() { Player = _yourTurn, TopCard = _cardsOnMid.Count > 0 ? _cardsOnMid[_cardsOnMid.Count - 1] : null });
            }
        }

        private void PutCardOnMid(Card card)
        {
            if(card == null)
            {
                MessageBus.Publish(new GameEndEvent());
                return;
            }
            _cardsOnMid.Add(card);
            if (_midRoot.childCount > 0)
            {
                _midRoot.GetChild(0).GetComponent<CardView>().LoadData(card);
            }
            else
            {
                GameObject newCard = ObjectPool.Instance.PopFromPool(PcResources.CardPrefab, false, true);
                newCard.GetComponent<CardView>().LoadData(card);
                newCard.transform.SetParent(_midRoot);
                newCard.transform.localPosition = Vector3.zero;
            }
        }

        #region Score Calculations
        private void CheckForScore()
        {
            var newCard = _cardsOnMid[_cardsOnMid.Count - 1];

            if (newCard.Value == CardValue.Two && newCard.Shape == CardShape.Clubs)
            {
                _roundScore += 2;
            }
            else if (newCard.Value == CardValue.Ten && newCard.Shape == CardShape.Diamonds)
            {
                _roundScore += 3;
            }
            else if (newCard.Value == CardValue.Ace || newCard.Value == CardValue.Jack)
            {
                _roundScore += 1;
            }

            if (_cardsOnMid.Count > 1)
            {
                var oldCard = _cardsOnMid[_cardsOnMid.Count - 2];

                if (newCard.Value == oldCard.Value || newCard.Value == CardValue.Jack)
                {
                    Debug.Log("!!SCORE!!");

                    if (_cardsOnMid.Count == 2)
                    {
                        Debug.Log("!!PISTI!!");
                        if (newCard.Value == CardValue.Jack)
                        {
                            if (oldCard.Value == CardValue.Jack)
                            {
                                Debug.Log("!!PISTI!!");
                                MessageBus.Publish(new PlayerGotScoreEvent() { Player = _yourTurn, Score = 20 });
                                Debug.Log("Player " + _yourTurn.PlayerID + " got 20");
                            }
                            else
                            {
                                MessageBus.Publish(new PlayerGotScoreEvent() { Player = _yourTurn, Score = _roundScore });
                                Debug.Log("Player " + _yourTurn.PlayerID + " got " + _roundScore);
                            }
                        }
                        else
                        {
                            Debug.Log("!!PISTI!!");
                            MessageBus.Publish(new PlayerGotScoreEvent() { Player = _yourTurn, Score = 10 });
                            Debug.Log("Player " + _yourTurn.PlayerID + " got 10");
                        }
                    }
                    else
                    {
                        MessageBus.Publish(new PlayerGotScoreEvent() { Player = _yourTurn, Score = _roundScore });
                        Debug.Log("Player " + _yourTurn.PlayerID + " got " + _roundScore);
                    }

                    _roundScore = 0;
                    _cardsOnMid.Clear();

                    foreach (Transform child in _midRoot)
                    {
                        GameObject.Destroy(child.gameObject);
                    }

                    Debug.Log("All childs are destroyed: " + _midRoot.childCount);
                }
            }

        }
        #endregion
    }
}
