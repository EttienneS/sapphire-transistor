using Assets.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.StrategyCamera
{
    public class CameraController : LocatableMonoBehaviorBase, ICameraController
    {
        public Camera Camera;
        public float maxZoom;
        public float minZoom;
        public float movementSpeed;
        public float movementTime;

        public float normalSpeed;

        public float rotationAmount;

        public Vector3 zoomAmount;

        private readonly Queue<CameraCommand> _commands = new Queue<CameraCommand>();

        private CameraInputHandler _cameraInputHandler;

        private int _maxX;

        private int _maxZ;

        private int _minX;

        private int _minZ;

        private Vector3 _newPosition;

        private Quaternion _newRotation;

        private Vector3 _newZoom;

        private bool _ready;

        public void AddCameraCommand(CameraCommand command)
        {
            _commands.Enqueue(command);
        }

        public void ConfigureBounds(int minx, int maxx, int minz, int maxz)
        {
            _minX = minx;
            _maxX = maxx;
            _minZ = minz;
            _maxZ = maxz;
        }

        public Camera GetCamera()
        {
            return Camera;
        }

        public override void Initialize()
        {
            ResetDeltas();

            // https://gameprogrammingpatterns.com/command.html
            // using the command pattern we can easily change the handler to work diffirently when on a phone
#if (UNITY_IPHONE || UNITY_ANDROID)
            //_cameraInputHandler = new TouchScreenHandler(this);
            _cameraInputHandler = new MouseAndKeyboardInputHandler(this);
#else
            _cameraInputHandler = new MouseAndKeyboardInputHandler(this);
#endif
            _ready = true;
        }

        public void MoveToPosition(Vector3 position)
        {
            Debug.Log($"Move to: {position}");
            transform.position = position;

            _newPosition = transform.position;
            _newZoom = new Vector3(0, minZoom, -minZoom);

            CameraEventManager.CameraPositionChanged(new Vector3(_newPosition.x, _newZoom.y, _newPosition.z));
        }

        public void Update()
        {
            if (_ready)
            {
                _cameraInputHandler.HandleInput();

                while (_commands.Count > 0)
                {
                    _commands.Dequeue().Execute(this);
                    CameraEventManager.CameraPositionChanged(new Vector3(_newPosition.x, _newZoom.y, _newPosition.z));
                }
                UpdateCameraAndEnsureBounds();
            }
        }

        public Vector3 GetNewPosition()
        {
            return _newPosition;
        }

        public Quaternion GetNewRotation()
        {
            return _newRotation;
        }

        public Vector3 GetNewZoom()
        {
            return _newZoom;
        }

        public float GetPerpendicularRotation()
        {
            return 90 + transform.rotation.eulerAngles.y;
        }

        public void SetNewPosition(Vector3 vector3)
        {
            _newPosition = vector3;
            CameraEventManager.CameraPositionChanged(new Vector3(_newPosition.x, _newZoom.y, _newPosition.z));
        }

        public void SetNewRotation(Quaternion value)
        {
            _newRotation = value;
        }

        public void SetNewZoom(Vector3 value)
        {
            _newZoom = value;
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            return new Vector3(Mathf.Clamp(position.x, _minX, _maxX),
                               position.y,
                               Mathf.Clamp(position.z, _minZ, _maxZ));
        }

        private Vector3 ClampZoom(Vector3 zoom)
        {
            return new Vector3(zoom.x,
                               Mathf.Clamp(zoom.y, minZoom, maxZoom),
                               Mathf.Clamp(zoom.z, -maxZoom, -minZoom));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.75f);
            Gizmos.DrawCube(transform.position, new Vector3(0.1f, 2f, 0.1f));
        }

        private void ResetDeltas()
        {
            _newPosition = transform.position;
            _newRotation = transform.rotation;
            _newZoom = Camera.transform.localPosition;
        }

        private void UpdateCameraAndEnsureBounds()
        {
            transform.position = ClampPosition(Vector3.Lerp(transform.position, _newPosition, Time.deltaTime * movementTime));
            transform.rotation = Quaternion.Lerp(transform.rotation, _newRotation, Time.deltaTime * movementTime);
            Camera.transform.localPosition = ClampZoom(Vector3.Lerp(Camera.transform.localPosition, _newZoom, Time.deltaTime * movementTime));
        }

        public (float minZoom, float maxZoom) GetMinMaxZoom()
        {
            return (minZoom, maxZoom);
        }
    }
}