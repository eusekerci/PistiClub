using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PistiClub
{
    public class CardView : MonoBehaviour, IPoolObject
    {
        public Card Data;
        private SpriteRenderer _renderer;

        public GameObject Prefab
        {
            get { return PcResources.Load<GameObject>(PcResourceType.Card); }
            set { }
        }

        public void Init()
        {
        }

        void Start()
        {
            Init();
        }

        public void LoadData(Card data)
        {
            Data = data;
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = PcResources.GetCardSprite(Data.Shape, Data.Value);
        }

        public void LoadData(CardShape shape, CardValue value)
        {
            var data = new Card(shape, value);
            LoadData(data);
        }
    }

}