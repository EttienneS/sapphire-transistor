using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.InputManager
{
    public class MoveCameraCommand : InputCommand
    {
        private Vector3 _amount;
        private ICameraController _camera;

        public MoveCameraCommand(ICameraController camera, Vector3 amount)
        {
            _amount = amount;
            _camera = camera;
        }

        public override void Execute()
        {
            _camera.SetNewPosition(_camera.GetNewPosition() + _amount);
        }
    }
}