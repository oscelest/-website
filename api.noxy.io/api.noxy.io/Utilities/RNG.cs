namespace api.noxy.io.Utilities
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

        public static int FromZeroToPositiveIntIncluding(int value)
        {
            return value > 0 ? random.Next(value + 1) : 0;
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
