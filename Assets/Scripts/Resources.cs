using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public static class PcResources
    {
        private static readonly Sprite[] CardSprites;
        static PcResources()
        {
            CardSprites = Resources.LoadAll <Sprite> ("Sprites/CardSprites");
        }

        public static Sprite GetCardSprite(CardShape shape, CardValue value)
        {
            return CardSprites[(int)shape + (((int)value - 1) * 4)];
        }
    }
}
