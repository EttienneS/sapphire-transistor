using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Assets.Helpers
{
    public static class RandomExtensions
    {
        // use System.Random instead of Unity random as Unity Random can only run on the main game thread...
        private static Random _rand = new Random();

        public static T GetRandomItem<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(_rand.Next(0, list.Count()));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _rand.Next(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }
    }
}