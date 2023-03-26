namespace api.noxy.io.Utilities
{
    public static class RNG
    {
        private static readonly Random random = new();

        public static int IntBetweenFactors(float value, float fmin = 0.9f, float fmax = 1.1f)
        {
            return IntBetweenFactors((int)value, fmin, fmax);
        }

        public static int IntBetweenFactors(int value, float fmin, float fmax)
        {
            float minValue = value * fmin;
            float maxValue = value * fmax;
            return (int)(random.NextDouble() * (maxValue - minValue) + minValue);
        }

        public static bool Boolean()
        {
            return random.Next(2) == 0;
        }

        public static T GetRandomElement<T>(List<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static IEnumerable<T> GetRandomElementList<T>(IEnumerable<T> list, int count)
        {
            return list.OrderBy(x => random.Next()).Take(count);
        }

    }
}
