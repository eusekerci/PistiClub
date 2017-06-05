using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class Player : PlayerBase
    {
        public Player(int playerID, Transform root)
        {
            Hand = new List<Card>();
            PlayerID = playerID;
            HandRoot = root;
        }

        protected override void OnTurnStart()
        {
            base.OnTurnStart();

        }

        protected override void OnPlayCard()
        {
            base.OnPlayCard();

        }

        public override void TakeCard(Card newCard)
        {
            base.TakeCard(newCard);
            Hand.Add(newCard);
        }

        public void ReorderHand()
        {

        }

        public override void Update()
        {
            base.Update();

        }
    }
}