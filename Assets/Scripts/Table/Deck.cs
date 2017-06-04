using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class DrawCardEvent : PcEvent { }

    public class Deck
    {
        private List<Card> _cardPool;
        private List<Card> _usedCards;

        public Deck(int numberOfDeck = 1)
        {
            _cardPool = new List<Card>();
            for (int i = 0; i < numberOfDeck; i++)
            {
                foreach (CardShape shape in System.Enum.GetValues(typeof(CardShape)))
                {
                    foreach (CardValue value in System.Enum.GetValues(typeof(CardValue)))
                    {
                        _cardPool.Add(new Card(shape, value));
                    }
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            _cardPool.Shuffle();
        }

        public Card Draw()
        {
            int r = Random.Range(0, RemainingCount());
            Card c = _cardPool[r];

            return Draw(c.Shape, c.Value);
        }

        public Card Draw(CardShape shape, CardValue value)
        {
            Card c = new Card(shape, value);

            foreach (Card card in _cardPool)
            {
                if (card == c)
                {
                    _usedCards.Add(card);
                    _cardPool.Remove(card);
                    MessageBus.Publish(new DrawCardEvent());
                    return c;
                }
            }

            Debug.Log("Error! The demanding card has already drawed!");
            return null;
        }

        public int RemainingCount()
        {
            return _cardPool.Count;
        }
    }
}
