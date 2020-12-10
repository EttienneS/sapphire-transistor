namespace Assets.ServiceLocator
{
    public abstract class GameServiceBase : IGameService
    {
        private Locator _locator;

        public void BindServiceLocator(Locator locator)
        {
            _locator = locator;
        }

        public Locator GetLocator()
        {
            return _locator;
        }

        public T Locate<T>() where T : IGameService
        {
            return _locator.Get<T>();
        }

        public abstract void Initialize();
    }
}