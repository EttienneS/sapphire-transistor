using Assets.Factions;
using Assets.ServiceLocator;
using UnityEngine;

namespace Assets.UI
{
    public class EndTurnButton : MonoBehaviour
    {
        private PlayerFaction _playerFaction;
        private IFactionManager _factionManager;

        private void Start()
        {
            _factionManager = Locator.Instance.Get<FactionManager>();
            _playerFaction = _factionManager.GetPlayerFaction() as PlayerFaction;
        }

        public void EndTurn()
        {
            _playerFaction.EndTurn();
        }
    }
}