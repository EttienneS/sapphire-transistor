using UnityEngine;

namespace Assets.StrategyCamera
{
    public class MoveCommand : CameraCommand
    {
        private Vector3 _amount;

        public MoveCommand(Vector3 amount)
        {
            _amount = amount;
        }

        public override void Execute(ICameraController camera)
        {
            camera.SetNewPosition(camera.GetNewPosition() + _amount);
        }
    }
}