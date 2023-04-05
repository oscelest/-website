namespace api.noxy.io.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public string Type { get; private set; }
        public string Property { get; private set; }
        public object Value { get; private set; }

        public EntityAlreadyExistsException(string type, string property, object value)
        {
            Type = type;
            Property = property;
            Value = value;
        }
    }
}
