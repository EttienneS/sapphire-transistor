using Assets.Map;
using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Structures
{
    public class StructureDefinitionManager : GameServiceBase, IStructureDefinitionManager
    {
        private Lazy<IPlacementValidator> _validator;
        public List<StructureDefinition> StructureDefinitions { get; internal set; }

        public IPlacementResult CanPlace(ICoord coord, StructureDefinition.StructureType type)
        {
            return GetDefinitionForType(type).CanPlace(coord);
        }

        public string GetAssetNameForStructureType(StructureDefinition.StructureType type)
        {
            return GetDefinitionForType(type).Asset;
        }

        public StructureDefinition GetDefinitionForType(StructureDefinition.StructureType type)
        {
            var def = StructureDefinitions.FirstOrDefault(s => s.Type == type);

            if (def == null)
            {
                Debug.LogError($"Type not found {type}");
                throw new KeyNotFoundException(type.ToString());
            }
            return def;
        }

        public override void Initialize()
        {
            _validator = new Lazy<IPlacementValidator>(() => Locate<IPlacementValidator>());
            StructureDefinitions = new List<StructureDefinition>
            {
                new StructureDefinition("Empty", "Empty", 5, StructureDefinition.StructureType.Empty, "NoBehavior", false, 1,1, (coord) => _validator.Value.CellEmpty(coord) ),
                new StructureDefinition("Tree", "Tree", 1000, StructureDefinition.StructureType.Tree, "NoBehavior", false, 1,1, (coord) => _validator.Value.CellEmpty(coord) ),
                new StructureDefinition("Rock", "Rock", 1000, StructureDefinition.StructureType.Rock, "NoBehavior", false, 1,1, (coord) => _validator.Value.CellEmpty(coord) ),
                new StructureDefinition("Core", "BellTower", 10, StructureDefinition.StructureType.Core, "SettlementCore", false, 2,2, (coord) => _validator.Value.CellEmpty(coord) ),
                new StructureDefinition("Road", "Road", 1000, StructureDefinition.StructureType.Road, "NoBehavior", true, 1,1, (coord) => _validator.Value.CellEmptyOrSame(coord, StructureDefinition.StructureType.Road) ),
                new StructureDefinition("House", "House", 500, StructureDefinition.StructureType.House, "HouseBehavior", true, 1,1, (coord) => _validator.Value.CellEmptyOrSame(coord,StructureDefinition.StructureType.House) ),
                new StructureDefinition("Barn", "Barn", 500, StructureDefinition.StructureType.Barn, "FarmBehaviour", true, 2,2, (coord) => _validator.Value.CellEmptyOrSame(coord,StructureDefinition.StructureType.Barn) ),
                new StructureDefinition("Cabin", "Cabin", 500, StructureDefinition.StructureType.Cabin, "CabinBehaviour", true, 2,2, (coord) => _validator.Value.EmptyAndTerrainMatches(coord,StructureDefinition.StructureType.Cabin, TerrainType.Forrest ) ),
                new StructureDefinition("Field", "Field", 500, StructureDefinition.StructureType.Field, "NoBehavior", false, 1,1, (coord) => _validator.Value.EmptyAndTerrainMatches(coord,StructureDefinition.StructureType.Field, TerrainType.Grass) ),
            };
        }
    }
}