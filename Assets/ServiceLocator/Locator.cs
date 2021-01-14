using Assets.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ServiceLocator
{
    public sealed class Locator : IServiceLocator
    {
        private readonly Dictionary<Type, IGameService> _services;

        private List<Type> _readyServices = new List<Type>();

        private Locator()
        {
            _services = new Dictionary<Type, IGameService>();
        }

        public static IServiceLocator Instance { get; set; }

        public static IServiceLocator Create()
        {
            Instance = new Locator();

            return Instance;
        }

        public T Find<T>() where T : class
        {
            var key = typeof(T);
            if (!_services.ContainsKey(key))
            {
                Debug.LogError($"{key} not registered with {GetType().Name}");
                throw new InvalidOperationException();
            }

            return (T)_services[key];
        }

        public IEnumerator ProcessServiceList()
        {
            foreach (var item in _services)
            {
                using (Instrumenter.Start(item.Key.Name))
                {
                    item.Value.Initialize();
                    _readyServices.Add(item.Key);
                    yield return null;
                }
            }

            LogServices();
        }

        public void Register<TService>(IGameService service) where TService : class
        {
            var serviceType = typeof(TService);
            if (_services.ContainsKey(serviceType))
            {
                Debug.LogError($"Attempted to register service of type {serviceType} which is already registered with the {GetType().Name}.");
                return;
            }
            service.BindServiceLocator(this);

            _services.Add(serviceType, service);
        }

        public bool ServicesReady(params Type[] types)
        {
            foreach (var type in types)
            {
                if (!_readyServices.Contains(type))
                {
                    return false;
                }
            }
            return true;
        }
        internal void LogServices()
        {
            var msg = "Loaded services:\n";
            foreach (var service in _services)
            {
                msg += $"- {service.Key} >> {service.Value}\n";
            }
            Debug.Log(msg);
        }
    }
}