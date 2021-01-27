using Assets.StrategyCamera;
using UnityEngine;

namespace Assets.InputManager
{
    public class MouseAndKeyboardInputHandler : IInputHandler
    {
        private CameraController _cameraController;
        private Vector3 _dragCurrentPosition;
        private Vector3 _dragStartPosition;
        private Vector3 _rotateCurrentPosition;
        private Vector3 _rotateStartPosition;

        public MouseAndKeyboardInputHandler(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        public void HandleInput()
        {
            HandleMouseWheel();

            HandleRightClick();

            HandleMiddleMouse();

            HandleArrowMovement();

            HandleKeyboardZoom();

            HandleRotateCard();
        }

        private void FollowPlaneDrag()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragCurrentPosition = ray.GetPoint(entry);
                _cameraController.SetNewPosition(_cameraController.transform.position + _dragStartPosition - _dragCurrentPosition);
            }
        }

        private void HandleArrowMovement()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                _cameraController.AddCameraCommand(new MoveCameraCommand(_cameraController, _cameraController.transform.forward * _cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _cameraController.AddCameraCommand(new MoveCameraCommand(_cameraController, _cameraController.transform.forward * -_cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                _cameraController.AddCameraCommand(new MoveCameraCommand(_cameraController, _cameraController.transform.right * _cameraController.movementSpeed));
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                _cameraController.AddCameraCommand(new MoveCameraCommand(_cameraController, _cameraController.transform.right * -_cameraController.movementSpeed));
            }
        }

        private void HandleKeyboardZoom()
        {
            if (Input.GetKey(KeyCode.R))
            {
                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController, _cameraController.zoomAmount));
            }
            if (Input.GetKey(KeyCode.F))
            {
                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController, _cameraController.zoomAmount * -1f));
            }
        }

        private void HandleMiddleMouse()
        {
            if (Input.GetMouseButtonDown(2))
            {
                StartMiddleMouseRotate();
            }

            if (Input.GetMouseButton(2))
            {
                RotateWithMiddleMouse();
            }
        }

        private void HandleMouseWheel()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _cameraController.AddCameraCommand(new ZoomCommand(_cameraController, Input.mouseScrollDelta.y * _cameraController.zoomAmount));
            }
        }

        private void HandleRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartPlaneDrag();
            }

            if (Input.GetMouseButton(1))
            {
                FollowPlaneDrag();
            }
        }

        private void HandleRotateCard()
        {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                InputEventManager.RotateCardCCW();
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                InputEventManager.RotateCardCW();
            }
        }

        private void RotateWithMiddleMouse()
        {
            _rotateCurrentPosition = Input.mousePosition;

            var diff = _rotateStartPosition - _rotateCurrentPosition;
            _rotateStartPosition = _rotateCurrentPosition;

            _cameraController.AddCameraCommand(new RotationCommand(_cameraController, diff));
        }

        private void StartMiddleMouseRotate()
        {
            _rotateStartPosition = Input.mousePosition;
        }

        private void StartPlaneDrag()
        {
            var plane = new Plane(Vector3.up, Vector3.zero);
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out float entry))
            {
                _dragStartPosition = ray.GetPoint(entry);
            }
        }
    }
}