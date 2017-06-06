using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace PistiClub
{
    public class AIController : ControllerBase
    {
        private Card _lastPlayedCard;
        public AIController(PlayerBase player)
        {
            _isPlayable = false;
            _myPlayer = player;

            MessageBus.OnEvent<TurnStartEvent>().Where(evnt => evnt.Player.PlayerID == _myPlayer.PlayerID).Subscribe(evnt =>
            {
                _myPlayer = evnt.Player;
                _lastPlayedCard = evnt.TopCard;
                _isPlayable = true;
            });
        }

        public override void Update()
        {
            if (!_isPlayable)
            {
                return;
            }
            base.Update();
            if (_lastPlayedCard == null)
            {
                _isPlayable = false;
                AIPlayRandomCommand();
            }
            else
            {
                for (int i = 0; i < _myPlayer.Hand.Count && _isPlayable; i++)
                {
                    if (_myPlayer.Hand[i].Value == _lastPlayedCard.Value)
                    {
                        _isPlayable = false;
                        AIPlayScoreCommand(i);
                        break;
                    }
                }
                if (_isPlayable && _myPlayer.Hand.Count > 0)
                {
                    _isPlayable = false;
                    AIPlayRandomCommand();
                }
            }
        }

        private void AIPlayRandomCommand()
        {
            IDisposable d = null;
            d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
            {
                MessageBus.Publish(new PlayCardCommand()
                {
                    Player = _myPlayer,
                    Card = _myPlayer.Hand[UnityEngine.Random.Range(0, _myPlayer.Hand.Count)],
                });
                d.Dispose();
            });
        }

        private void AIPlayScoreCommand(int i)
        {
            IDisposable d = null;
            d = Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(x =>
            {
                MessageBus.Publish(new PlayCardCommand()
                {
                    Player = _myPlayer,
                    Card = _myPlayer.Hand[i],
                });
                d.Dispose();
            });
        }
    }

}