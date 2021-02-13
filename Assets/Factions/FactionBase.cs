using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<ResourceType, int> _resources;

        protected FactionBase(string name, IServiceLocator serviceLocator)
        {
            _resources = new Dictionary<ResourceType, int>();
            Name = name;

            StructureManager = new StructureManager(serviceLocator.Find<IStructureFactory>(),
                                                    serviceLocator.Find<IFactionManager>(),
                                                    serviceLocator.Find<IMapManager>());
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public string Name { get; }

        public IStructureManager StructureManager { get; }

        public bool CanAfford(Dictionary<ResourceType, int> cost)
        {
            var resources = GetResources();
            foreach (var resource in cost)
            {
                if (!resources.ContainsKey(resource.Key))
                {
                    return false;
                }
                if (resources[resource.Key] < resource.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public void DoTurnEndActions()
        {
            StructureManager.DoTurnEndActions();
        }

        public void DoTurnStartActions()
        {
            StructureManager.DoTurnStartActions();
            TurnStarted?.Invoke(this);
        }

        public void EndTurn()
        {
            TurnEnded?.Invoke(this);
        }

        public Dictionary<ResourceType, int> GetResources()
        {
            return _resources;
        }

        public void ModifyResource(ResourceType resource, int amount)
        {
            if (!_resources.ContainsKey(resource))
            {
                _resources.Add(resource, 0);
            }
            _resources[resource] += amount;

            OnResourcesUpdated?.Invoke(resource, _resources[resource]);
        }

        public abstract void TakeTurn();

        public void ModifyResource(Dictionary<ResourceType, int> cost)
        {
            foreach (var kvp in cost)
            {
                ModifyResource(kvp.Key, kvp.Value);
            }
        }
    }
}