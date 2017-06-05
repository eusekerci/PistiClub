using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class AIController : ControllerBase
    {
        private PlayerBase _myPlayer;

        public AIController(PlayerBase player)
        {
            _myPlayer = player;
        }

        public override void Update()
        {
            base.Update();
        }
    }

}