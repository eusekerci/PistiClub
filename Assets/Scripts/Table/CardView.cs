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

        public void LoadData(Card data, bool isSecret = false)
        {
            Data = data;
            _renderer = GetComponent<SpriteRenderer>();
            if (!isSecret)
            {
                _renderer.sprite = PcResources.GetCardSprite(Data.Shape, Data.Value);
            }
            else
            {
                _renderer.sprite = PcResources.GetCardBackSprite();
            }
        }

        public void LoadData(CardShape shape, CardValue value, bool isSecret = false)
        {
            var data = new Card(shape, value);
            LoadData(data, isSecret);
        }

        public void LoadCardBack()
        {
            Data = null;
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.sprite = PcResources.GetCardBackSprite();
        }
    }

}