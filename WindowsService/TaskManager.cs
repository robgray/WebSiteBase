using System;
using System.ServiceProcess;
using System.Threading;
using Castle.Core.Logging;
using WindowsService.Tasks;

namespace WindowsService
{
    [System.ComponentModel.DesignerCategory("")]
    public class TaskManager : ServiceBase
    {
        private Thread _thread;
        private Timer _scheduledTaskTimer;
        private static object lockObject = new object();

        private readonly ITaskDefinition[] _tasks;
        public ILogger Logger { get; set; }

        public TaskManager(TaskDefinitionFactory factory)
        {
            _tasks = factory.GetAll();
            
            ServiceName = "Task Execution Service";
        }

        protected override void OnStart(string[] args)
        {
            LoadTaskProcessor();

            _thread = new Thread(RequestLoop);
            _thread.Start();
        }

        protected override void OnStop()
        {
            if (_scheduledTaskTimer != null)
                _scheduledTaskTimer.Dispose();
        }

        /// <summary>
        /// Load the permitted scheduled tasks to be run, depending on configuration settings.
        /// </summary>
        private void LoadTaskProcessor()
        {
            var taskNames = new string[_tasks.Length];            
            for (var i = 0; i < taskNames.Length; i++)
                taskNames[i] = _tasks[i].Name;

            Logger.Info("Loaded Tasks: " + String.Join(", ", taskNames));
        }

        void RequestLoop()
        {
            try
            {
                StartScheduledTaskTimer();
            }
            catch (Exception ex)
            {                
                Logger.Error("Exception not caught in request loop", ex);
            }
        }

        private void StartScheduledTaskTimer()
        {            
            _scheduledTaskTimer =
            new Timer(o =>
            {
                if (Monitor.TryEnter(lockObject))
                {
                    try
                    {
                        new TaskProcessor(_tasks).CheckScheduledTask();
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            Logger.Error("Exception starting scheduled task timer ", e);
                        }
                        catch
                        {
                            // don't quit if logging fails
                        }
                    }
                    finally
                    {
                        Monitor.Exit(lockObject);
                    }
                }
            }, null, 10000, 10000);
        }
    }

    public class TaskInfo
    {
        public readonly string TaskName;
        public readonly ITaskDefinition TaskDefinition;
        public readonly int TaskId;
        public readonly Guid Guid;
        public readonly Thread Thread;
        public readonly DateTime StartTime;
        public readonly TimeSpan ExpireSpan;

        public TaskInfo(string taskName, ITaskDefinition definition, int taskId, Guid guid, Thread thread, DateTime startTime, TimeSpan expireSpan)
        {
            TaskName = taskName;
            TaskDefinition = definition;
            TaskId = taskId;
            Guid = guid;
            Thread = thread;
            StartTime = startTime;
            ExpireSpan = expireSpan;
        }
    }
}
