using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuController : MonoBehaviour
    {
        public GameObject ElementContainer;

        public RadialMenuElement RadialMenuElementPrefab;

        private Vector2 _origin = Vector2.zero; // in case we want the circle center to move
        private const float Radius = 125f; // hardcoded for now to match the circle sprite

        public void AddButton(string text, RadialMenuDelegates.MenuItemClicked onItemClicked)
        {
            var menuButton = Instantiate(RadialMenuElementPrefab, ElementContainer.transform);
            menuButton.Load(text, onItemClicked);
            UpdatePositionOfElements();
        }

        public void Start()
        {
            for (int i = 0; i < 8; i++)
            {
                var name = $"Test {i}";
                AddButton($"{name}", () => Debug.Log($"{name} Clicked!"));
            }
        }

        private void UpdatePositionOfElements()
        {
            var menuElements = GetMenuElements();
            var radian = GetRadian(menuElements.Length);

            for (var i = 0; i < menuElements.Length; i++)
            {
                var theta = i * radian;
                menuElements[i].transform.localPosition = CalculatePositionOnCircle(_origin, theta);
            }
        }

        private RadialMenuElement[] GetMenuElements()
        {
            return ElementContainer.GetComponentsInChildren<RadialMenuElement>();
        }

        private static float GetRadian(int elementCount)
        {
            return (Mathf.PI * 2) / elementCount;
        }

        private static Vector2 CalculatePositionOnCircle(Vector2 origin, float theta)
        {
            var x = origin.x + (Radius * Mathf.Cos(theta));
            var y = origin.y + (Radius * Mathf.Sin(theta));
            return new Vector2(x, y);
        }
    }
}