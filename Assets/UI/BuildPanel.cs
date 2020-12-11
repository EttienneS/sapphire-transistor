using Assets.Factions;
using Assets.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public class BuildPanel : MonoBehaviour
    {
        private IFaction _playerFaction;

        public Button StructureButtonPrefab;

        private void Start()
        {
            _playerFaction = Locator.Instance.Get<FactionManager>().GetPlayerFaction();

            foreach (var structure in _playerFaction.GetBuildableStructures())
            {
                var btn = Instantiate(StructureButtonPrefab, transform);
                btn.GetComponentInChildren<Text>().text = structure.Name;
            }
        }

        private void Update()
        {
        }
    }
}