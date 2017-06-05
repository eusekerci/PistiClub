using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public enum PcResourceType
    {
        None,
        Card
    }

    public static class PcResources
    {
        private static readonly Dictionary<PcResourceType, string> ResourcePaths = new Dictionary<PcResourceType, string>();
        
        private static readonly Sprite[] CardSprites;
        static PcResources()
        {
            ResourcePaths.Add(PcResourceType.Card, "Prefabs/Card");

            CardSprites = Resources.LoadAll<Sprite>("Sprites/CardSprites");
            GameObject go = Resources.Load<GameObject>("Prefabs/Card");
            ObjectPool.Instance.AddToPool(go, 10, GameObject.Find("CardPool").transform);
        }

        public static Sprite GetCardSprite(CardShape shape, CardValue value)
        {
            return CardSprites[(int)shape + (((int)value - 1) * 4)];
        }

        public static Sprite GetCardBackSprite()
        {
            return CardSprites[52];
        }

        public static T Load<T>(PcResourceType resType) where T : Object
        {
            if (!ResourcePaths.ContainsKey(resType))
            {
                Debug.LogError("Resource doesn't exist : " + resType);
                return null;
            }

            return Resources.Load<T>(ResourcePaths[resType]);
        }
    }
}
