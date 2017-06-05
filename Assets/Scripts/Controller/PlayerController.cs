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
            //Player can nothing when it's not his turn.
            //Implemented for base gameplay
            //TODO just apply this for card selection
            if (!_myPlayer.IsMyTurn)
                return;

            base.Update();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, LayerMask.NameToLayer("Cards")))
            {
                var hitCard = hit.transform.GetComponent<CardView>().Data;
                MessageBus.Publish(new PlayCardEvent()
                {
                    Player = _myPlayer,
                    Card = hitCard,
                });
            }
        }
    }

}
