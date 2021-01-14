using UnityEngine;

namespace Assets.StrategyCamera
{
    public class RotationCommand : CameraCommand
    {
        private Vector3 _amount;

        public RotationCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(ICameraController camera)
        {
            camera.SetNewRotation(camera.GetNewRotation() * Quaternion.Euler(Vector3.up * (-_amount.x / 5)));
        }
    }
}