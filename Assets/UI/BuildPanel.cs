using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.UI
{
    public class BuildPanel : MonoBehaviour
    {
        private IFaction _playerFaction;
        private ISpawnManager _spawnManager;

        private void Start()
        {
            CellEventManager.OnCellClicked += CellClicked;

            _playerFaction = Locator.Instance.Find<IFactionManager>().GetPlayerFaction();
            _spawnManager = Locator.Instance.Find<ISpawnManager>();

            foreach (var structure in _playerFaction.StructureManager.GetBuildableStructures())
            {
                _spawnManager.SpawnUIElement("StructureButtonPrefab", transform, (obj) =>
                {
                    var btn = obj.GetComponent<Button>();
                    obj.GetComponentInChildren<Text>().text = structure.Name;
                    btn.onClick.AddListener(() => _selectedFacade = structure);
                });
            }
        }

        private IStructureFacade _selectedFacade;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _selectedFacade = null;
            }
        }

        public void CellClicked(Cell cell)
        {
            if (MouseOverUi())
            {
                return;
            }

            if (_selectedFacade != null)
            {
                _playerFaction.StructureManager.AddStructure(_selectedFacade, cell.Coord);
            }
        }

        private bool MouseOverUi()
        {
            if (EventSystem.current == null)
            {
                // event system not on yet
                return false;
            }

            return EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null;
        }
    }
}