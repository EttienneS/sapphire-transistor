using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI
{
    public class UIManager : LocatableMonoBehaviorBase, IUIManager
    {
        private IFaction _activePlayer;
        private CurrentPlayerLabel _currentPlayerLabel;
        private EndTurnButton _endTurnButton;

        private IFactionManager _factionManager;
        private List<GameObject> _activeHighlights = new List<GameObject>();
        private IFaction _playerFaction;
        private ISpawnManager _spawnManager;
        public RadialMenuManager RadialMenuManager { get; set; }
        public MessageManager MessageManager { get; set; }

        public void DisableHighlights()
        {
            if (_activeHighlights.Count != 0)
            {
                foreach (var highlight in _activeHighlights)
                {
                    _spawnManager.AddItemToDestroy(highlight);
                }
                _activeHighlights.Clear();
            }
        }

        public void HighlightCells(ICoord[] coords, Color color)
        {
            DisableHighlights();
            foreach (var coord in coords)
            {
                _spawnManager.SpawnModel("Highlight", coord.ToAdjustedVector3(), (obj) =>
                {
                    _activeHighlights.Add(obj);
                    obj.GetComponent<MeshRenderer>().material.color = color;
                });
            }
        }

        public override void Initialize()
        {
            _factionManager = Locate<IFactionManager>();
            _playerFaction = _factionManager.GetPlayerFaction();
            _factionManager.OnTurnStarted += FactionManager_OnTurnStarted;
            _spawnManager = Locate<ISpawnManager>();

            _currentPlayerLabel = GetComponentInChildren<CurrentPlayerLabel>();
            _endTurnButton = GetComponentInChildren<EndTurnButton>();

            _currentPlayerLabel.Hide();

            RadialMenuManager = new RadialMenuManager(_spawnManager, transform);
            MessageManager = new MessageManager(_spawnManager, transform);
        }

        private void FactionManager_OnTurnStarted(IFaction faction)
        {
            _activePlayer = faction;
        }

        private void ShowOrHideActivePlayerMessage()
        {
            if (_activePlayer == null)
            {
                return;
            }

            if (_activePlayer == _playerFaction)
            {
                _currentPlayerLabel.Hide();
                _endTurnButton.gameObject.SetActive(true);
            }
            else
            {
                _currentPlayerLabel.Show(_activePlayer);
                _endTurnButton.gameObject.SetActive(false);
            }
        }

        private bool _ready;
        private bool? _showDrawView;

        private void Update()
        {
            if (!_ready)
            {
                _ready = GetLocator().ServicesReady(typeof(IFactionManager));
            }
            else
            {
                ShowOrHideActivePlayerMessage();
            }

            if (_showDrawView.HasValue)
            {
                if (_showDrawView.Value)
                {
                    _spawnManager.SpawnUIElement("DrawView", transform, (obj) =>
                    {
                        _drawView = obj.GetComponent<DrawView>();
                    });
                }
                else
                {
                    HideDrawView();
                }

                _showDrawView = null;
            }
        }

        private DrawView _drawView;

        public void ShowDrawView()
        {
            _showDrawView = true;
        }

        public void HideDrawView()
        {
            _showDrawView = false;
        }

        public void DestroyDrawView()
        {
            if (_drawView != null)
            {
                _spawnManager.AddItemToDestroy(_drawView.gameObject);
            }
            _drawView = null;
        }
    }
}