using System.Collections.Generic;

namespace Assets.Scripts.Extension
{
    public static class Random
    {
        private static System.Random rng = new System.Random();

        public static float SimpleFloat()
        {
            return (float)rng.NextDouble();
        }

        public static int SimpleInt()
        {
            return rng.Next(0,100);
        }
        public static int SimpleInt(int end)
        {
            return rng.Next(0, end);
        }
        public static int SimpleInt<T>(List<T> end)
        {
            return rng.Next(0, end.Count);
        }
    }
}