using Assets.Factions;
using Assets.Resources;
using Assets.ServiceLocator;
using System;
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

            foreach (var resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                SpawnResourceLabel((ResourceType)resourceType, 0);
            }

            _playerFaction.OnResourcesUpdated += OnPlayerResoucesUpdated;
        }

        private bool _updateResouces;

        private void OnPlayerResoucesUpdated(ResourceType resourceType, int newValue)
        {
            _updateResouces = true;
        }

        public void Update()
        {
            if (_updateResouces)
            {
                _updateResouces = false;

                foreach (var res in _playerFaction.GetResources())
                {
                    _labelLookup[res.Key].text = $"{res.Key}: {res.Value}";
                }
            }
        }

        private void SpawnResourceLabel(ResourceType resourceType, int newValue)
        {
            _spawnManager.SpawnUIElement("ResourceLabelPrefab", transform, (obj) =>
            {
                var label = obj.GetComponent<TMP_Text>();
                label.text = $"{resourceType}: {newValue}";
                _labelLookup.Add(resourceType, label);
            });
        }
    }
}