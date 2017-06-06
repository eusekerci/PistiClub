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
        private int _playerScore;
        private int _aiScore;

        void Start()
        {
            _playerScore = 0;
            _aiScore = 0;

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
    
        }

        void Update()
        {
            PlayerScoreText.text = "Player Score\n" + _playerScore;
            AIScoreText.text = "AI Score\n" + _aiScore;
        }
    }

}
