using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class PlayCardEvent : PcEvent
    {
        public Card Card { get; set; }
    }

    public abstract class ControllerBase
    {
        public virtual void Update() { }
    }
}
