using Assets.Factions;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        private Dictionary<ResourceType, TMP_Text> _labelLookup;
        private IFaction _playerFaction;
        private bool _ready;
        private ISpawnManager _spawnManager;

        public void Update()
        {
            if (_ready)
            {
                foreach (var res in _playerFaction.GetResources())
                {
                    if (_labelLookup.ContainsKey(res.Key))
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

            _ready = true;
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