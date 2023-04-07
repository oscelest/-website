namespace api.noxy.io.Exceptions
{
    public class NotEnoughCurrencyException : Exception
    {
        public NotEnoughCurrencyException()
        {
        }

        public NotEnoughCurrencyException(string message) : base(message)
        {

        }
    }
}
