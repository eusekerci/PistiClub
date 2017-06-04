using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public enum CardShape
    {
        Diamonds,
        Clubs,
        Hearts,
        Spades
    }

    public enum CardValue
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }

    public class Card
    {
        public CardShape Shape { get; private set; }
        public CardValue Value { get; private set; }
        private string _id;
        public string ID
        {
            get
            {
                return _id;
            }
            private set
            {
                _id = value;
            }
        }

        public Card(CardShape shape, CardValue value)
        {
            Shape = shape;
            Value = value;
            ID = CreateId(shape, value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Card c = (Card)obj;
            return (Shape == c.Shape) && (Value == c.Value);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public bool Equals(CardShape shape, CardValue value)
        {
            if (Shape == shape && Value == value)
                return true;
            else
                return false;
        }

        public static string CreateId(CardShape shape, CardValue value)
        {
            return shape.ToString() + value.ToString();
        }
    }
}
