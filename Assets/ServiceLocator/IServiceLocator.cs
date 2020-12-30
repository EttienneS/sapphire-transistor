namespace Assets.ServiceLocator
{
    public interface IServiceLocator
    {
        T Find<T>() where T : class;
        void InitializeServices();
        void Register<TService>(IGameService service) where TService : class;
    }
}