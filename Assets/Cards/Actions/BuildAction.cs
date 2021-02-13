using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using UnityEngine;

namespace Assets.Cards.Actions
{
    public class BuildAction : ICardAction
    {
        private readonly IFaction _owner;
        private readonly StructureType _structureType;

        private GameObject _previewObject;
        private ISpawnManager _spawnManager;

        public BuildAction(StructureType structureType, IFaction owner)
        {
            _structureType = structureType;
            _owner = owner;

            _spawnManager = Locator.Instance.Find<ISpawnManager>();
        }

        public bool CanPlay(ICoord coord)
        {
            return _owner.StructureManager.PlacementValidator.CanPlace(coord, _structureType).CanPlace;
        }

        public void ClearPreview()
        {
            if (_previewObject != null)
            {
                _spawnManager.RecyleItem(_structureType, _previewObject);
            }
        }

        public void Play(ICoord coord)
        {
            _owner.StructureManager.AddStructure(_structureType, coord);
        }

        public void Preview(ICoord coord)
        {
            _spawnManager.SpawnPreviewModel(_structureType, coord.ToAdjustedVector3(), CanPlay(coord), (obj) => _previewObject = obj);
        }

        public override string ToString()
        {
            return _structureType.ToString().Substring(0, 1);
        }
    }
}