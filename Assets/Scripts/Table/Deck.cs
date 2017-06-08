using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class DrawCardEvent : PcEvent
    {
        public int Remaining;
    }

    public class Deck
    {
        private List<Card> _cardPool;
        private List<Card> _usedCards;

        public Deck(int numberOfDeck = 1)
        {
            _cardPool = new List<Card>();
            _usedCards = new List<Card>();

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
            if (RemainingCount() == 0)
            {
                return null;
            }

            int r = Random.Range(0, RemainingCount());

            return Draw(_cardPool[r].Shape, _cardPool[r].Value);
        }

        public Card Draw(CardShape shape, CardValue value)
        {
            Card c = new Card(shape, value);

            foreach (Card card in _cardPool)
            {
                if (card.Equals(c))
                {
                    _usedCards.Add(card);
                    _cardPool.Remove(card);
                    MessageBus.Publish(new DrawCardEvent() { Remaining = _cardPool.Count });
                    return c;
                }
            }

            return null;
        }

        public int RemainingCount()
        {
            return _cardPool.Count;
        }
    }
}
