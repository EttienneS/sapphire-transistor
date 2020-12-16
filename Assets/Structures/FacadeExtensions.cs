namespace Assets.Structures
{
    public static class FacadeExtensions
    {
        public static int GetTotalBuildTime(this IStructureFacade facade)
        {
            var total = 0;
            foreach (var cost in facade.Cost)
            {
                total += cost.Item2;
            }

            return total;
        }
    }
}