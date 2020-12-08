namespace Assets.ServiceLocator
{
    public interface IGameService
    {
        void BindServiceLocator(Locator locator);

        void Initialize();
    }
}