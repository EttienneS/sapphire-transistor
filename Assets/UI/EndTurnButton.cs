using Assets.Factions;
using Assets.ServiceLocator;
using UnityEngine;

namespace Assets.UI
{
    public class EndTurnButton : MonoBehaviour
    {
        private PlayerFaction _playerFaction;

        public void EndTurn()
        {
            if (_playerFaction == null)
            {
                _playerFaction = Locator.Instance.Find<IFactionManager>().GetPlayerFaction() as PlayerFaction;
            }
            _playerFaction.EndTurn();
        }
    }
}