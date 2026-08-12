namespace _Project.Scripts.EventSystem
{
    public interface IEvent 
    { 
    }

    public interface IEvent<T> : IEvent
    {
        T Value { get; }
    }
}