using UnityEngine;

namespace Assets.StrategyCamera
{
    public interface ICameraController
    {
        void AddCameraCommand(CameraCommand command);
        void ConfigureBounds(int minx, int maxx, int minz, int maxz);
        Camera GetCamera();
        void MoveToPosition(Vector3 vector3);
        Vector3 GetNewPosition();
        Quaternion GetNewRotation();
        Vector3 GetNewZoom();
        (float minZoom, float maxZoom) GetMinMaxZoom();
        float GetPerpendicularRotation();
        void SetNewPosition(Vector3 vector3);
        void SetNewRotation(Quaternion value);
        void SetNewZoom(Vector3 value);
    }
}