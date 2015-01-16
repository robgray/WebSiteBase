using System;
using Infrastructure.Common.Windsor;

namespace WindowsService.Tasks
{
    /// <summary>
    /// Tasks are composed by a ITaskDefinition and a IWorker implemenetation.
    /// Tasks are registered with Windsor as ITaskDefinition, while the workers 
    /// are registered by the actual class, using IWorker as the convention.
    /// 
    /// The constructor for each Task (ITaskDefinition implementation) should take at least
    /// a Lazy<ConcreteIWorker> which will be resolved by Windsor Lazily (only when Run is called).
    /// 
    /// A Scoped lifestyle is used. LifestyleScopeInterceptor is registered in the container and looks
    /// for all ITaskDefinition's with LifestyleScopeAttribute with will then surround the method call with 
    /// using (Container.BeginScope()), to define the scope of dependencies.
    /// 
    /// See WorkoutCreationTask and WorkoutCreationWorker for an example.
    /// </summary>

    public interface ITaskDefinition
    {
        string Name { get; }

        /// <summary>
        /// The amount of time that must pass before this task will run again. For example, if this task 
        /// should run once an hour, then Interval should return a 1 hour <see cref="TimeSpan"/>. This will
        /// only be checked if AllowExecutionAt returns true.
        /// </summary>
        TimeSpan Interval { get; }

        /// <summary>
        /// Whether or not the task can execute at the given time. If it can execute and it has not run within the last Interval then it will run.
        /// </summary>
        /// <param name="time">The time to check</param>
        /// <returns>True if the task can execute, False if it cannot.</returns>
        bool AllowExecutionAt(DateTime time);
        //IUnitOfWork UnitOfWork { set; }

        // The repository is supplied so that the caller (a new thread created by Tier1Service.cs) can ensure the repository connection is closed just prior to the thread shutting down
        [LifestyleScope]
        void Run();
    }

    public interface IWorker
    {
        void Run();
    }
}
