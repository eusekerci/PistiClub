﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class PlayerController : ControllerBase
    {
        public override void Update()
        {
            base.Update();

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

            }
        }
    }

}