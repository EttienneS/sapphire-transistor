using Assets.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ServiceLocator
{
    public sealed class Locator : IServiceLocator
    {
        private readonly Dictionary<Type, IGameService> _services;

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

        public void InitializeServices()
        {
            foreach (var item in _services)
            {
                using (Instrumenter.Start(item.Key.Name))
                {
                    item.Value.Initialize();
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