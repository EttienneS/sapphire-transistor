using Assets.Resources;
using Assets.ServiceLocator;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public abstract class FactionBase : IFaction
    {
        private readonly Dictionary<ResourceType, int> _resources;


        public FactionBase(string name, IServiceLocator serviceLocator)
        {
            _resources = new Dictionary<ResourceType, int>();

            Name = name;

            StructureManager = new StructureManager(serviceLocator.Find<ISpawnManager>(), 
                                                    serviceLocator.Find<IStructureFactory>(), 
                                                    serviceLocator.Find<IStructurePlacementValidator>());
        }

        public event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        public event FactionDelegates.OnTurnEnded TurnEnded;

        public event FactionDelegates.OnTurnStarted TurnStarted;

        public string Name { get; }

        public IStructureManager StructureManager { get; }

        public void DoTurnEndActions()
        {
            StructureManager.DoTurnEndActions();
        }

        public void DoTurnStartActions()
        {
            StructureManager.DoTurnStartActions();
            AddResources(StructureManager.GetCombinedYield());
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

        private void AddResources(List<(ResourceType resouceType, int amount)> resouces)
        {
            foreach (var resouce in resouces)
            {
                ModifyResource(resouce.resouceType, resouce.amount);
            }
        }
    }
}