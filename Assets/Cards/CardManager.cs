using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Cards
{
    public class CardManager : GameServiceBase, ICardManager
    {
        private List<string> _rawOptions;

        public string GetRandomRawCard()
        {
            return _rawOptions[Random.Range(0, _rawOptions.Count)];
        }

        public override void Initialize()
        {
            _rawOptions = new List<string>();

            foreach (var cardObject in Resources.LoadAll<TextAsset>("Cards"))
            {
                Debug.Log($"Card Loaded: {cardObject.name}");
                _rawOptions.Add(cardObject.text);
            }
        }
    }
}