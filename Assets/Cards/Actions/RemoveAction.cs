using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using UnityEngine;

namespace Assets.Cards.Actions
{
    public class RemoveAction : ICardAction
    {
        public const string AssetName = "RemoveMarker";
        private IFactionManager _factionManager;
        private GameObject _previewObject;
        private ISpawnManager _spawnManager;

        public RemoveAction()
        {
            _factionManager = Locator.Instance.Find<IFactionManager>();
            _spawnManager = Locator.Instance.Find<ISpawnManager>();
        }

        public bool CanPlay(Coord coord)
        {
            return true;
        }

        public void ClearPreview()
        {
            if (_previewObject != null)
            {
                _spawnManager.RecyleItem(AssetName, _previewObject);
            }
        }

        public void Play(Coord coord)
        {
            if (_factionManager.TryGetStructureAtCoord(coord, out IStructure structure))
            {
                _factionManager.GetOwnerOfStructure(structure)
                               .StructureManager.RemoveStructure(structure);
            }

            ClearPreview();
        }

        public void Preview(Coord coord)
        {
            _spawnManager.SpawnPreviewModel(AssetName, coord.ToAdjustedVector3(), CanPlay(coord), (obj) => _previewObject = obj);
        }

        public override string ToString()
        {
            return "X";
        }
    }
}