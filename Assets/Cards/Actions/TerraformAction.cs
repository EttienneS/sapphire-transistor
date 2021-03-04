using Assets.Map;

namespace Assets.Cards.Actions
{

    public class TerraformAction : ICardAction
    {
        private IMapManager _mapManager;
        private ITerrain _terrain;

        public TerraformAction(IMapManager mapManager, ITerrain terrain)
        {
            _mapManager = mapManager;
            _terrain = terrain;
        }

        public bool CanPlay(Coord coord)
        {
            return _mapManager.TryGetCellAtCoord(coord, out Cell _);
        }

        public void ClearPreview()
        {
        }

        public void Play(Coord coord)
        {
            if (_mapManager.TryGetCellAtCoord(coord, out Cell cell))
            {
                _mapManager.ChangeCellTerrain(cell, _terrain);
            }
        }

        public void Preview(Coord coord)
        {
        }
    }
}