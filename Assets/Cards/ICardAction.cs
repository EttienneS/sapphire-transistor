using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using UnityEngine;

namespace Assets.Cards
{
    public interface ICardAction
    {
        bool CanPlay(ICoord coord);

        void Play(ICoord coord);

        void Preview(ICoord coord);

        void ClearPreview();
    }

    public class BuildAction : ICardAction
    {
        private readonly IFaction _owner;
        private readonly StructureType _structureType;

        private ISpawnManager _spawnManager;
        private IStructureFactory _structureFactory;

        public BuildAction(StructureType structureType, IFaction owner)
        {
            _structureType = structureType;
            _owner = owner;

            _structureFactory = Locator.Instance.Find<IStructureFactory>();
            _spawnManager = Locator.Instance.Find<ISpawnManager>();
        }

        public bool CanPlay(ICoord coord)
        {
            return _owner.StructureManager.PlacementValidator.CanPlace(coord, _structureType).CanPlace;
        }

        public void Play(ICoord coord)
        {
            _owner.StructureManager.AddStructure(_structureType, coord);

            ClearPreview();
        }

        private GameObject _previewObject;

        public void Preview(ICoord coord)
        {
            Debug.Log($"Build {_structureType} at {coord}");
            _spawnManager.SpawnPreviewModel(_structureType, coord.ToAdjustedVector3(), CanPlay(coord), (obj) => _previewObject = obj);
        }

        public override string ToString()
        {
            return _structureType.ToString().Substring(0, 1);
        }

        public void ClearPreview()
        {
            if (_previewObject != null)
            {
                _spawnManager.RecyleItem(_structureType, _previewObject);
            }
        }
    }
}