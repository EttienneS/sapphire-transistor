using System;
using System.Collections;

namespace Assets.ServiceLocator
{
    public interface IServiceLocator
    {
        T Find<T>() where T : class;

        IEnumerator ProcessServiceList();

        void Register<TService>(IGameService service) where TService : class;

        bool ServicesReady(params Type[] types);
    }
}