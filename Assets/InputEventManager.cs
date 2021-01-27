using UnityEngine;

namespace Assets
{
    public static class InputDelegates
    {
        public delegate void RotateCardDelegate();
    }

    public static class InputEventManager
    {
        public static event InputDelegates.RotateCardDelegate OnRotateCardCW;
        public static event InputDelegates.RotateCardDelegate OnRotateCardCCW;

        public static void RotateCardCW()
        {
            Debug.Log($"Rotate clockwise");
            OnRotateCardCW?.Invoke();
        }

        public static void RotateCardCCW()
        {
            Debug.Log($"Rotate counter-clockwise");
            OnRotateCardCCW?.Invoke();
        }


    }
}