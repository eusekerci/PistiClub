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

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            ReorderHand();
        }

        protected override void OnPlayCard(Card playedCard)
        {
            base.OnPlayCard(playedCard);
            Hand.Remove(playedCard);
            ReorderHand();
        }

        protected override void OnTurnStart()
        {
            base.OnTurnStart();
        }

        public override void TakeCard(Card newCard)
        {
            base.TakeCard(newCard);
            Hand.Add(newCard);
        }

        public void ReorderHand()
        {
            for (int i = 0; i < HandRoot.childCount; i++)
            {
                GameObject oldCard = HandRoot.GetChild(0).gameObject;
                ObjectPool.Instance.PushToPool(ref oldCard);
            }

            for (int i = 0; i < Hand.Count; i++)
            {
                GameObject newCard = ObjectPool.Instance.PopFromPool(PcResources.Load<GameObject>(PcResourceType.Card), false, true);
                newCard.GetComponent<CardView>().LoadData(Hand[i]);
                newCard.transform.position = HandRoot.transform.position + new Vector3(i * 2f, 0f, 0f);
                newCard.transform.SetParent(HandRoot);
            }
        }

        public override void Update()
        {
            base.Update();

        }
    }
}