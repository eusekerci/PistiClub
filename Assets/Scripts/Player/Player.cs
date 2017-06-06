using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class Player : PlayerBase
    {
        public Player(int playerID, Transform root, bool isAI = false)
        {
            Hand = new List<Card>();
            PlayerID = playerID;
            HandRoot = root;
            _isAI = isAI;
        }

        protected override void OnRoundStart()
        {
            base.OnRoundStart();
            ReorderHand();
        }

        protected override void OnPlayCard(Card playedCard)
        {
            Debug.Log(PlayerID + " played " + playedCard.ID);
            base.OnPlayCard(playedCard);
            Hand.RemoveAll(c => playedCard.ID == c.ID);
            ReorderHand();
            MessageBus.Publish(new PlayCardEvent() { Player = this, Card = playedCard });
        }

        protected override void OnTurnStart()
        {
            base.OnTurnStart();
            Debug.Log("My Turn " + PlayerID);
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
                GameObject.Destroy(HandRoot.GetChild(i).gameObject);
            }

            for (int i = 0; i < Hand.Count; i++)
            {
                GameObject newCard = ObjectPool.Instance.PopFromPool(PcResources.CardPrefab, false, true);
                newCard.GetComponent<CardView>().LoadData(Hand[i], _isAI);
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