using UnityEngine;

namespace Assets.StrategyCamera
{
    public static class CameraEventManager
    {
        public delegate void CameraPositionChangedDelegate(Vector3 newPostion);

        public static CameraPositionChangedDelegate OnCameraPositionChanged;

        public static void CameraPositionChanged(Vector3 newPostion)
        {
            //Debug.Log($"New pos: {newPostion}");
            OnCameraPositionChanged?.Invoke(newPostion);
        }
    }
}