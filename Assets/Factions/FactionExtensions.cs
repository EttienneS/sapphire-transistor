using Assets.Map;
using Assets.Structures;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Factions
{
    public static class FactionExtensions
    {
        public static List<ICoord> GetOpenAnchorPoints(this IFaction faction)
        {
            return faction.StructureManager.GetStructures()
                                           .Where(s => s.Type == StructureType.Anchor)
                                           .Select(s => s.GetOrigin())
                                           .ToList();
        }
    }
}