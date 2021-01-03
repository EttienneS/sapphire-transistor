using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuElement : MonoBehaviour
    {
        public event RadialMenuDelegates.MenuItemClicked MenuItemClicked;

        public void OnClick()
        {
            MenuItemClicked?.Invoke();
        }

        public void Load(string text, RadialMenuDelegates.MenuItemClicked menuItemClicked)
        {
            GetComponentInChildren<TMPro.TMP_Text>().text = text;
            MenuItemClicked += menuItemClicked;
        }
    }
}