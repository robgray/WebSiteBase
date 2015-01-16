using Castle.Core.Logging;
using Infrastructure.Common;
using WindowsService.Tasks;

namespace WindowsService
{
    public class TaskProcessor
    {
        public ILogger Logger { get; set; }
        private ITaskDefinition[] _tasks;

        public TaskProcessor(ITaskDefinition[] tasks)
        {
            _tasks = tasks;
        }

        public void CheckScheduledTask()
        {
            foreach (var task in _tasks)
            {
                if (task.AllowExecutionAt(DateTimeHelper.Now))
                {
                    task.Run();
                }
            }
        }
    }
}
