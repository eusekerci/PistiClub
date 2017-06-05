using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class PlayerController : ControllerBase
    {
        private PlayerBase _myPlayer;

        public PlayerController(PlayerBase player)
        {
            _myPlayer = player;
        }

        public override void Update()
        {
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
