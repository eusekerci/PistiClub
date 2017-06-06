using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PistiClub
{
    public class PlayCardCommand : PcEvent
    {
        public Card Card { get; set; }
        public PlayerBase Player { get; set; }
    }

    public abstract class ControllerBase
    {
        protected PlayerBase _myPlayer;
        protected bool _isPlayable;

        protected ControllerBase()
        {
        }

        public virtual void Update() { }
    }
}
