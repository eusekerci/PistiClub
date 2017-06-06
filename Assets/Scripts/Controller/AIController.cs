using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PistiClub
{
    public class AIController : ControllerBase
    {
        private Card _lastPlayedCard;
        public AIController(PlayerBase player)
        {
            _isPlayable = false;
            _myPlayer = player;

            MessageBus.OnEvent<TurnStartEvent>().Subscribe(evnt =>
            {
                if (evnt.Player.PlayerID == _myPlayer.PlayerID)
                {
                    _myPlayer = evnt.Player;
                    _lastPlayedCard = evnt.TopCard;
                    _isPlayable = true;
                }
                else
                {
                    _isPlayable = false;
                }
            });
        }

        public override void Update()
        {
            if (!_isPlayable)
            {
                return;
            }
            base.Update();
            for (int i = 0; i < _myPlayer.Hand.Count; i++)
            {
                if (_myPlayer.Hand[i].Value == _lastPlayedCard.Value)
                {
                    MessageBus.Publish(new PlayCardCommand()
                    {
                        Player = _myPlayer,
                        Card = _myPlayer.Hand[i],
                    });
                    _isPlayable = false;
                    break;
                }
            }
            if(_isPlayable && _myPlayer.Hand.Count > 0)
            {
                MessageBus.Publish(new PlayCardCommand()
                {
                    Player = _myPlayer,
                    Card = _myPlayer.Hand[Random.Range(0, _myPlayer.Hand.Count)],
                });
                _isPlayable = false;
            }
        }
    }

}