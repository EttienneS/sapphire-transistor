using Assets.Factions;
using Assets.ServiceLocator;
using UnityEngine;

namespace Assets.UI
{
    public class UIManager : MonoBehaviour
    {
        private CurrentPlayerLabel _currentPlayerLabel;
        private EndTurnButton _endTurnButton;

        private IFactionManager _factionManager;
        private IFaction _playerFaction;

        private void Start()
        {
            _factionManager = Locator.Instance.Find<IFactionManager>();
            _playerFaction = _factionManager.GetPlayerFaction();
            _factionManager.OnTurnStarted += FactionManager_OnTurnStarted;

            _currentPlayerLabel = GetComponentInChildren<CurrentPlayerLabel>();
            _endTurnButton = GetComponentInChildren<EndTurnButton>();

            _currentPlayerLabel.Hide();
        }

        private IFaction _activePlayer;

        private void FactionManager_OnTurnStarted(IFaction faction)
        {
            _activePlayer = faction;
        }

        private void Update()
        {
            ShowOrHideActivePlayerMessage();
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
    }
}