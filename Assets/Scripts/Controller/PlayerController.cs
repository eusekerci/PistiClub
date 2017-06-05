using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PistiClub
{
    public class PlayerController : ControllerBase
    {
        private PlayerBase _myPlayer;
        private bool _isPlayable;

        public PlayerController(PlayerBase player)
        {
            _isPlayable = false;
            _myPlayer = player;

            MessageBus.OnEvent<TurnStartEvent>().Subscribe(evnt =>
            {
                if (evnt.Player.PlayerID == _myPlayer.PlayerID)
                {
                    _myPlayer = evnt.Player;
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
            if(!_isPlayable)
            {
                return;
            }
            base.Update();
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1 << LayerMask.NameToLayer("Cards")))
                {
                    Debug.Log("Card clicked by " + _myPlayer.PlayerID);
                    var hitCard = hit.transform.GetComponent<CardView>().Data;
                    MessageBus.Publish(new PlayCardCommand()
                    {
                        Player = _myPlayer,
                        Card = hitCard,
                    });
                }
            }
        }
    }

}
