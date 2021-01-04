using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Helpers
{
    public static class UIHelper
    {
        public static bool MouseOverUi()
        {
            if (EventSystem.current == null)
            {
                // event system not on yet
                return false;
            }

            foreach (Touch touch in Input.touches)
            {
                int id = touch.fingerId;
                if (EventSystem.current.IsPointerOverGameObject(id))
                {
                    return true;
                }
            }

            return EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null;
        }
    }
}