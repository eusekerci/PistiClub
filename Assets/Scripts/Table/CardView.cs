using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class CardView : MonoBehaviour
    {
        public Card Data;
        private SpriteRenderer _renderer;

        void Start()
        {
            Data = new Card(CardShape.Diamonds, CardValue.Jack);
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = PcResources.GetCardSprite(Data.Shape, Data.Value);
        }

        void Update()
        {

        }
    }

}