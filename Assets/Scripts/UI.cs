using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

namespace PistiClub
{
    public class UI : MonoBehaviour
    {

        public Text PlayerScoreText;
        public Text AIScoreText;
        public Text GameState;
        private int _playerScore;
        private int _aiScore;
        private int _remainingCards;
        private bool _isGameRunning;

        void Start()
        {
            _remainingCards = 52;
            _playerScore = 0;
            _aiScore = 0;
            _isGameRunning = true;

            MessageBus.OnEvent<PlayerGotScoreEvent>().Subscribe(evnt =>
            {
                //We somwhow know the player IDs for now
                if(evnt.Player.PlayerID == 0)
                {
                    _playerScore += evnt.Score;
                }
                else
                {
                    _aiScore += evnt.Score;
                }
            });

            MessageBus.OnEvent<DrawCardEvent>().Subscribe(evnt =>
            {
                _remainingCards = evnt.Remaining;
            });

            MessageBus.OnEvent<GameEndEvent>().Subscribe(evnt =>
            {
                _isGameRunning = false;
            });
    
        }

        void Update()
        {
            PlayerScoreText.text = "Player Score\n" + _playerScore;
            AIScoreText.text = "AI Score\n" + _aiScore;
            if(_isGameRunning)
                GameState.text = "Remaining\n" + _remainingCards + " cards";
            else
            {
                string winner = _playerScore > _aiScore ? "Player" : "Computer";
                GameState.text = "Game End\n" + winner + " Wins!";
            }
        }
    }

}
