using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class PlayCardEvent : PcEvent
    {
        public PlayerBase Player { get; set; }
        public Card Card { get; set; }
    }

    public abstract class ControllerBase
    {
        protected ControllerBase() { }

        public virtual void Update() { }
    }
}
