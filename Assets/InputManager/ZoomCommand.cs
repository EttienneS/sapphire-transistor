using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.InputManager
{
    public class ZoomCommand : InputCommand
    {
        private Vector3 _amount;

        private ICameraController _camera;

        public ZoomCommand(ICameraController camera, Vector3 amount)
        {
            _amount = amount;
            _camera = camera;
        }

        public override void Execute()
        {
            var zoom = _camera.GetNewZoom();
            var (minZoom, maxZoom) = _camera.GetMinMaxZoom();

            zoom += _amount;
            zoom = new Vector3(zoom.x,
                                         Mathf.Clamp(zoom.y, minZoom, maxZoom),
                                         Mathf.Clamp(zoom.z, -maxZoom, -minZoom));

            _camera.SetNewZoom(zoom);
        }
    }
}