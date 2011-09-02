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
    }

    public interface IStateMachineConfiguration<TStateType, TEntity>
    {
        TStateType Initial { set; }
        ITransitionHookBuilder<TStateType, TEntity> BeforeTransition { get; set; }
        ITransitionHookBuilder<TStateType, TEntity> AfterTransition { get; set; }
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
        private static States<T> _any = new States<T>();
        private readonly T[] _states;


        public static States<T> Any { get { return _any; } }

        public static T[] operator -(States<T> state, T value)
        {
            return new[] { value };
        }

        public static implicit operator T[](States<T> state)
        {
            return state._states;
        }

        public States(params T[] states)
        {
            if (states.Length == 0)
                _states = Enum.GetValues(typeof (T)).Cast<T>().ToArray();
            _states = states;
        }
    }
}
