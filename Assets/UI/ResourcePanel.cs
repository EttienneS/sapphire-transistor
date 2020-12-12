using Assets.Factions;
using Assets.Resources;
using Assets.ServiceLocator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        private IFaction _playerFaction;
        private SpawnManager _spawnManager;

        private Dictionary<ResourceType, TMP_Text> _labelLookup;

        private void Start()
        {
            _labelLookup = new Dictionary<ResourceType, TMP_Text>();

            _playerFaction = Locator.Instance.Get<FactionManager>().GetPlayerFaction();
            _spawnManager = Locator.Instance.Get<SpawnManager>();

            

            _playerFaction.OnResourcesUpdated += OnPlayerResoucesUpdated;


        }

        private void OnPlayerResoucesUpdated(ResourceType resourceType, int newValue)
        {
            if (!_labelLookup.ContainsKey(resourceType))
            {
                _spawnManager.SpawnUIElement("ResourceLabelPrefab", transform, (obj) =>
                {
                    var label = obj.GetComponent<TMP_Text>();
                    label.text = $"{resourceType}: {newValue}";
                    _labelLookup.Add(resourceType, label);
                });
            }
            else
            {
                _labelLookup[resourceType].text = $"{resourceType}: {newValue}";
            }
        }
    }
}