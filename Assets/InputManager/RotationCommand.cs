using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.InputManager
{
    public class RotationCommand : InputCommand
    {
        private Vector3 _amount;
        private ICameraController _camera;

        public RotationCommand(ICameraController camera, Vector3 amount)
        {
            _amount = amount;
            _camera = camera;
        }

        public override void Execute()
        {
            _camera.SetNewRotation(_camera.GetNewRotation() * Quaternion.Euler(Vector3.up * (-_amount.x / 5)));
        }
    }
}