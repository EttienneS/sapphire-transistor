using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.UI
{
    public static class RadialMenuMemory
    {
        public static string LastClickedOption { get; set; } = "-- Unknown --";
    }

    public class RadialMenuController : MonoBehaviour
    {
        public bool CloseOnSelect = true;
        public Button ConfirmButton;
        public GameObject ElementContainer;
        public RadialMenuElement RadialMenuElementPrefab;

        private RadialMenuDelegates.MenuItemConfirmed _activeItemMethod;
        private (string text, RadialMenuDelegates.MenuItemClicked onClick, RadialMenuDelegates.MenuItemConfirmed onConfirm)? _default;
        private Vector2 _origin = Vector2.zero; // in case we want the circle center to move
        private float Radius;

        public event RadialMenuDelegates.MenuClosed MenuClosed;

        public void AddButton(string text, RadialMenuDelegates.MenuItemClicked onItemClicked, RadialMenuDelegates.MenuItemConfirmed onItemConfirmed,  bool enabled = true)
        {
            var menuButton = Instantiate(RadialMenuElementPrefab, ElementContainer.transform);
            menuButton.Load(text);
            menuButton.MenuItemClicked += () => MenuButton_MenuItemClicked(text, onItemClicked, onItemConfirmed);

            // use first or last remembered selection as default option
            Debug.Log($"{text} - {RadialMenuMemory.LastClickedOption}");
            if (text.Equals(RadialMenuMemory.LastClickedOption, System.StringComparison.OrdinalIgnoreCase) || _default == null)
            {
                _default = (text, onItemClicked, onItemConfirmed);
            }

#if DEBUG
            menuButton.MenuItemClicked += () => Debug.Log($"{text} Clicked!");
#endif

            if (!enabled)
            {
                menuButton.GetComponent<Button>().interactable = false;
            }

            UpdatePositionOfElements();
        }

        public void Awake()
        {
            var containerRect = GetComponent<RectTransform>().sizeDelta;
            var buttonRect = RadialMenuElementPrefab.GetComponent<RectTransform>().sizeDelta;
            var offset = Mathf.Max(buttonRect.x, buttonRect.y) / 2;
            Radius = (Mathf.Max(containerRect.x, containerRect.y) / 2) + offset;
        }
         
        public void CloseMenu()
        {
            MenuClosed?.Invoke();
            Destroy(gameObject);
        }

        public void ConfirmClicked()
        {
            _activeItemMethod.Invoke();

            if (CloseOnSelect)
            {
                CloseMenu();
            }
        }

        internal void SetDefaults()
        {
            MenuButton_MenuItemClicked(_default.Value.text, _default.Value.onClick, _default.Value.onConfirm);
        }

        private static float GetRadian(int elementCount)
        {
            return (Mathf.PI * 2) / elementCount;
        }

        private Vector2 CalculatePositionOnCircle(Vector2 origin, float theta)
        {
            var x = origin.x + (Radius * Mathf.Cos(theta));
            var y = origin.y + (Radius * Mathf.Sin(theta));
            return new Vector2(x, y);
        }

        private RadialMenuElement[] GetMenuElements()
        {
            return ElementContainer.GetComponentsInChildren<RadialMenuElement>();
        }

        private void MenuButton_MenuItemClicked(string text, RadialMenuDelegates.MenuItemClicked menuItemClicked, RadialMenuDelegates.MenuItemConfirmed menuItemConfirmed)
        {
            Debug.Log($"{RadialMenuMemory.LastClickedOption}");

            RadialMenuMemory.LastClickedOption = text;
            ConfirmButton.GetComponentInChildren<TMP_Text>().text = text;
            _activeItemMethod = menuItemConfirmed;

            menuItemClicked.Invoke();
            Debug.Log($"{RadialMenuMemory.LastClickedOption}");
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