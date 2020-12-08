using Assets.Map;

namespace Assets.Actors
{
    public interface IStructure
    {
        void GetName();

        ICell GetLocation();
    }
}