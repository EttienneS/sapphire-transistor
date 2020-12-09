using Assets.ServiceLocator;
using System;
using System.Collections.Generic;

namespace Assets.Resources
{
    public class ResourceManager : GameServiceBase, IResourceManager
    {
        private List<IResource> _allResources;

        public List<IResource> GetAllResouces()
        {
            return _allResources;
        }

        public IResource GetResouceByName(string name)
        {
            return _allResources.Find(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public override void Initialize()
        {
            _allResources = new List<IResource>
            {
                new Resource("Stone"),
                new Resource("Wood"),
                new Resource("Food")
            };
        }
    }
}