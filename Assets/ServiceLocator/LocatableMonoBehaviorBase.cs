using UnityEngine;

namespace Assets.ServiceLocator
{
    public abstract class LocatableMonoBehaviorBase : MonoBehaviour, IGameService
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

        public T Locate<T>() where T : class
        {
            return _locator.Find<T>();
        }

        public abstract void Initialize();
    }
}