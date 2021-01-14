using UnityEngine;

namespace Assets.StrategyCamera
{
    public class ZoomCommand : CameraCommand
    {
        private Vector3 _amount;

        public ZoomCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(ICameraController camera)
        {
            var zoom = camera.GetNewZoom();
            var (minZoom, maxZoom) = camera.GetMinMaxZoom();

            zoom += _amount;
            zoom = new Vector3(zoom.x,
                                         Mathf.Clamp(zoom.y, minZoom, maxZoom),
                                         Mathf.Clamp(zoom.z, -maxZoom, -minZoom));

            camera.SetNewZoom(zoom);
        }
    }
}