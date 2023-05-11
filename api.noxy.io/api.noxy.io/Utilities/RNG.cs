namespace Database.Utilities
{
    public static class RNG
    {
        private static readonly Random random = new();


        public static Stack<int> SplitIntRandomly(int value, int count)
        {
            Stack<int> stack = new();
            for (int i = 0; i < count; i++)
            {
                int next = i < count - 1 ? random.Next(value) : value;
                stack.Push(next);
                value -= next;
            }
            return stack;
        }

        public static int NextInt(int max) => NextInt(0, max, true);
        public static int NextInt(int max, bool inclusive) => NextInt(0, max, inclusive);
        public static int NextInt(int min, int max) => NextInt(min, max, true);
        public static int NextInt(int min, int max, bool inclusive)
        {
            return inclusive ? random.Next(min, max + 1) : random.Next(min, max);
        }

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
