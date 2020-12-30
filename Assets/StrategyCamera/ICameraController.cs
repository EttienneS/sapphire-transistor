using UnityEngine;

namespace Assets.StrategyCamera
{
    public interface ICameraController
    {
        void AddCameraCommand(CameraCommand command);
        void ConfigureBounds(int minx, int maxx, int minz, int maxz);
        Camera GetCamera();
        void MoveToPosition(Vector3 vector3);
    }
}