using UnityEngine;

namespace Assets.Map
{
    public class Terrain : ITerrain
    {
        private string _name;
        private Color _color;

        public Terrain(string name, Color color)
        {
            _name = name;
            _color = color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public string GetName()
        {
            return _name;
        }
    }
}