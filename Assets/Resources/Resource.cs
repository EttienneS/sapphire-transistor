namespace Assets.Resources
{
    public class Resource : IResource
    {
        public Resource(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}