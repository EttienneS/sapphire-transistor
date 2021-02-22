using Assets.Factions;
using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Cards
{
    public class CardManager : GameServiceBase, ICardManager
    {
        private CardLoader _cardLoader;
        private List<string> _rawOptions;

        public ICard GetRandomCard(IFaction owner)
        {
            return _cardLoader.Load(_rawOptions[Random.Range(0, _rawOptions.Count)], owner);
        }

        public override void Initialize()
        {
            _cardLoader = new CardLoader();
            _rawOptions = new List<string>();

            foreach (var cardObject in Resources.LoadAll<TextAsset>("Cards"))
            {
                Debug.Log($"Card Loaded: {cardObject.name}");
                _rawOptions.Add(cardObject.text);
            }
        }
    }
}