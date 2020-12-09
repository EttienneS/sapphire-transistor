using System.Collections.Generic;

namespace Assets.Resources
{
    public interface IResourceManager
    {
        List<IResource> GetAllResouces();

        IResource GetResouceByName(string name);
    }
}