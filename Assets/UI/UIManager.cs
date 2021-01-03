using Assets.Factions;
using Assets.ServiceLocator;

namespace Assets.UI
{
    public class UIManager : LocatableMonoBehaviorBase, IUIManager
    {
        private CurrentPlayerLabel _currentPlayerLabel;
        private EndTurnButton _endTurnButton;

        private IFactionManager _factionManager;
        private IFaction _playerFaction;

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

        public override void Initialize()
        {
            _factionManager = Locate<IFactionManager>();
            _playerFaction = _factionManager.GetPlayerFaction();
            _factionManager.OnTurnStarted += FactionManager_OnTurnStarted;

            _currentPlayerLabel = GetComponentInChildren<CurrentPlayerLabel>();
            _endTurnButton = GetComponentInChildren<EndTurnButton>();

            _currentPlayerLabel.Hide();

            RadialMenuManager = new RadialMenuManager(Locate<ISpawnManager>(), transform);
        }

        public RadialMenuManager RadialMenuManager { get;  set; }
    }
}