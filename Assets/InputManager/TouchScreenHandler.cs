using Assets.StrategyCamera;
using System;
using UnityEngine;

namespace Assets.InputManager
{
    public class TouchScreenHandler : IInputHandler
    {
        private Vector3 _dragCurrentPosition;
        private Vector3 _dragStartPosition;
        private Vector3 _rotateCurrentPosition;
        private Vector3 _rotateStartPosition;

        private CameraController _cameraController;

        public TouchScreenHandler(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public void HandleInput()
        {
            throw new NotImplementedException();
            if (Input.touchCount >= 2)
            {
                Vector2 touch0, touch1;
                float distance;
                touch0 = Input.GetTouch(0).position;
                touch1 = Input.GetTouch(1).position;
                distance = Vector2.Distance(touch0, touch1);

                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController, _cameraController.zoomAmount * distance));
            }
        }
    }
}