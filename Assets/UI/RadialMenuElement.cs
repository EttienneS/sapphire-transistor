using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuElement : MonoBehaviour
    {
        private RadialMenuDelegates.MenuItemClicked _menuItemClicked;

        public void OnClick()
        {
            _menuItemClicked?.Invoke();
        }

        public void Load(string text, RadialMenuDelegates.MenuItemClicked menuItemClicked)
        {
            GetComponentInChildren<TMPro.TMP_Text>().text = text;
            _menuItemClicked = menuItemClicked;
        }
    }
}