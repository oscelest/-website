namespace api.noxy.io.Exceptions
{
    public class NotEnoughCurrencyException : Exception
    {
        public int Currency { get; private set; }

        public NotEnoughCurrencyException(int currency)
        {
            Currency = currency;
        }
    }
}
