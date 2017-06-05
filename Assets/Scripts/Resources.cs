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
            CardSprites = Resources.LoadAll<Sprite>("Sprites/CardSprites");
            GameObject go = Resources.Load<GameObject>("Prefabs/Card");
            ObjectPool.Instance.AddToPool(go, 10, GameObject.Find("CardPool").transform);
        }

        public static Sprite GetCardSprite(CardShape shape, CardValue value)
        {
            return CardSprites[(int)shape + (((int)value - 1) * 4)];
        }
    }
}
