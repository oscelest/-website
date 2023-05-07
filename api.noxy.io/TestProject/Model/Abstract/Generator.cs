namespace Test.Model.Abstract
{
    public abstract class Generator<T>
    {
        public int Counter { get; protected set; } = 0;
        public List<T> Generated { get; private set; } = new List<T>();

        public T Current { get => Generated.Last(); }
        public T this[int index] => Generated[index];

    }
}
