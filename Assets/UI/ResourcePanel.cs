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
        private ISpawnManager _spawnManager;

        private Dictionary<ResourceType, TMP_Text> _labelLookup;

        private bool _ready;

        private void LoadPanel()
        {
            if (!Locator.Instance.ServicesReady(typeof(ISpawnManager), typeof(IFactionManager)))
            {
                return;
            }

            _spawnManager = Locator.Instance.Find<ISpawnManager>();
            _playerFaction = Locator.Instance.Find<IFactionManager>().GetPlayerFaction();

            if (_playerFaction == null)
            {
                return;
            }

            _labelLookup = new Dictionary<ResourceType, TMP_Text>();

            foreach (var resourceType in Enum.GetValues(typeof(ResourceType)))
            {
                SpawnResourceLabel((ResourceType)resourceType, 0);
            }

            _playerFaction.OnResourcesUpdated += OnPlayerResoucesUpdated;
            _ready = true;
        }

        private bool _updateResouces;

        private void OnPlayerResoucesUpdated(ResourceType resourceType, int newValue)
        {
            _updateResouces = true;
        }

        public void Update()
        {
            if (_ready)
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
            else
            {
                LoadPanel();
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