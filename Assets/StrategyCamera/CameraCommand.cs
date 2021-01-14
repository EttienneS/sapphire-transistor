namespace Assets.StrategyCamera
{
    public abstract class CameraCommand
    {
        public abstract void Execute(ICameraController camera);
    }
}