using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using UnityEngine;

namespace Assets.UI
{
    public class UIManager : LocatableMonoBehaviorBase, IUIManager
    {
        private IFaction _activePlayer;
        private CurrentPlayerLabel _currentPlayerLabel;
        private EndTurnButton _endTurnButton;

        private IFactionManager _factionManager;
        private GameObject _highlight;
        private IFaction _playerFaction;
        private ISpawnManager _spawnManager;
        public RadialMenuManager RadialMenuManager { get; set; }

        public void DisableHighlight()
        {
            if (_highlight != null)
            {
                _spawnManager.AddItemToDestroy(_highlight);
                _highlight = null;
            }
        }

        public void HighlightCell(Cell cell, Color color)
        {
            DisableHighlight();
            _spawnManager.SpawnAddressable("Highlight", cell.Coord.ToAdjustedVector3(), (obj) => _highlight = obj);
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

        private void Update()
        {
            ShowOrHideActivePlayerMessage();
        }
    }
}