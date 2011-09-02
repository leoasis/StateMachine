using System;
using System.Linq;

namespace StateMachine
{
    public class StateMachine<TStateType>
    {
        public static class For<TEntity>
        {
            public static StateMachine<TStateType> Create(Action<IStateMachineConfiguration<TStateType, TEntity>> configuration)
            {
                return new StateMachine<TStateType>();
            }
        }

        public static implicit operator TStateType(StateMachine<TStateType> stateMachine)
        {
            return default(TStateType);
        }

        public void Fire(string @event)
        {
            throw new NotImplementedException();
        }
    }

    public interface IStateMachineConfiguration<TStateType, TEntity>
    {
        TStateType Initial { set; }
        ITransitionHookBuilder<TStateType, TEntity> BeforeTransition { get; }
        ITransitionHookBuilder<TStateType, TEntity> AfterTransition { get; }
        IEventBuilder<TStateType, TEntity> OnEvent(string @event);
    }

    public interface IEventBuilder<TStateType, TEntity>
    {
        IEventTransition<TStateType, TEntity> Transition { get; }
    }

    public interface IEventTransition<TStateType, TEntity>
    {
        IEventTransitionFrom<TStateType, TEntity> From(params TStateType[] states);
    }

    public interface IEventTransitionFrom<TStateType, TEntity>
    {
        IEventTransitionTo<TStateType, TEntity> To(TStateType state);
        IEventTransitionTo<TStateType, TEntity> ToSame();
    }

    public interface IEventTransitionTo<TStateType, TEntity>
    {
        IEventTransition<TStateType, TEntity> Transition { get; }
        IEventBuilder<TStateType, TEntity> If(Func<TEntity, bool> condition);
        IEventBuilder<TStateType, TEntity> Unless(Func<TEntity, bool> condition);        
    }

    public interface ITransitionHookBuilder<TStateType, TEntity>
    {
        ITransitionHookFrom<TStateType, TEntity> From(params TStateType[] states);
        ITransitionHookDo<TStateType, TEntity> On(string eventName);
    }

    public interface ITransitionHookFrom<TStateType, TEntity>
    {
        ITransitionHookDo<TStateType, TEntity> To(params TStateType[] states);
    }

    public interface ITransitionHookDo<TStateType, TEntity>
    {
        void Do(Action action);
        void Do(Action<TEntity> action);
    }


    public class States<T>
    {
        private static readonly States<T> _any = new States<T>();
        private readonly T[] _states;

        public static States<T> Any { get { return _any; } }
        public static States<T> All { get { return Any; } }

        public static T[] operator -(States<T> state, T value)
        {
            return state - new[] { value };
        }

        public static T[] operator -(States<T> state, T[] values)
        {
            return state._states.Except(values).ToArray();
        }

        public static implicit operator T[](States<T> state)
        {
            return state._states;
        }

        public States(params T[] states)
        {
            if (states.Length == 0)
                _states = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            _states = states;
        }
    }
}
