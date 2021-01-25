using Assets.Factions;
using Assets.Map;

namespace Assets.Structures.Cards
{
    public interface ICard
    {
        (int x, int z) GetAnchorPoint();

        StructureType?[,] GetStructures();

        bool CanPlay(ICoord coord);

        void RotateLeft();

        void RotateRight();
    }
}