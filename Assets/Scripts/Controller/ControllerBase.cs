using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class PlayCardCommand : PcEvent
    {
        public Card Card { get; set; }
        public PlayerBase Player { get; set; }
    }

    public abstract class ControllerBase
    {
        protected ControllerBase() { }

        public virtual void Update() { }
    }
}
