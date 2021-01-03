using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuController : MonoBehaviour
    {
        public bool CloseOnSelect = true;
        public GameObject ElementContainer;

        public RadialMenuElement RadialMenuElementPrefab;

        private float Radius;
        private Vector2 _origin = Vector2.zero; // in case we want the circle center to move

        public void Awake()
        {
            var containerRect = GetComponent<RectTransform>().sizeDelta;
            var buttonRect = RadialMenuElementPrefab.GetComponent<RectTransform>().sizeDelta;
            var offset = Mathf.Max(buttonRect.x, buttonRect.y) / 2;
            Radius = (Mathf.Max(containerRect.x, containerRect.y) / 2) + offset;
        }

        public void AddButton(string text, RadialMenuDelegates.MenuItemClicked onItemClicked)
        {
            var menuButton = Instantiate(RadialMenuElementPrefab, ElementContainer.transform);
            menuButton.Load(text, onItemClicked);
#if DEBUG
            menuButton.MenuItemClicked += () => Debug.Log($"{text} Clicked!");
#endif
            if (CloseOnSelect)
            {
                menuButton.MenuItemClicked += () => CloseMenu();
            }
            UpdatePositionOfElements();
        }

        public void CloseMenu()
        {
            Destroy(gameObject);
        }

        private Vector2 CalculatePositionOnCircle(Vector2 origin, float theta)
        {
            var x = origin.x + (Radius * Mathf.Cos(theta));
            var y = origin.y + (Radius * Mathf.Sin(theta));
            return new Vector2(x, y);
        }

        private static float GetRadian(int elementCount)
        {
            return (Mathf.PI * 2) / elementCount;
        }

        private RadialMenuElement[] GetMenuElements()
        {
            return ElementContainer.GetComponentsInChildren<RadialMenuElement>();
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
    }
}