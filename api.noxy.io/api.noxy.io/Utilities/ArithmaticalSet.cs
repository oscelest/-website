namespace Database.Utilities
{
    public class ArithmaticalSet
    {
        public float Flat { get; set; }
        public float Multipactive { get; set; }
        public float Exponential { get; set; }

        public ArithmaticalSet(float flat, float multiplicative, float exponential)
        {
            Flat = flat;
            Multipactive = multiplicative;
            Exponential = exponential;
        }
    }
}
