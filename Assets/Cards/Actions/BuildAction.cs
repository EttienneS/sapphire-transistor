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
        private readonly StructureDefinition.StructureType _structureType;

        private GameObject _previewObject;
        private ISpawnManager _spawnManager;
        private IStructureDefinitionManager _structureDefinitionManager;

        public BuildAction(StructureDefinition.StructureType structureType, IFaction owner)
        {
            _structureType = structureType;
            _owner = owner;

            _spawnManager = Locator.Instance.Find<ISpawnManager>();
            _structureDefinitionManager = Locator.Instance.Find<IStructureDefinitionManager>();
        }

        public bool CanPlay(Coord coord)
        {
            return _structureDefinitionManager.CanPlace(coord, _structureType).CanPlace;
        }

        public void ClearPreview()
        {
            if (_previewObject != null)
            {
                _spawnManager.RecyleItem(_structureType, _previewObject);
            }
        }

        public void Play(Coord coord)
        {
            _owner.StructureManager.AddStructure(_structureType, coord);
        }

        public void Preview(Coord coord)
        {
            _spawnManager.SpawnPreviewModel(_structureType, coord.ToAdjustedVector3(), CanPlay(coord), (obj) => _previewObject = obj);
        }

        public override string ToString()
        {
            return _structureType.ToString().Substring(0, 1);
        }
    }
}