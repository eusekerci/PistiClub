using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class TurnStartEvent : PcEvent { }

    public class GameManager : MonoBehaviour
    {
        private Deck _deck;

        void Start()
        {
            _deck = new Deck();
            MessageBus.Publish(new TurnStartEvent());
            MessageBus.Publish(new PlayCardEvent());
        }

        void Update()
        {

        }
    }
}
