using System;
using System.Threading.Tasks;

namespace ProfilerLite.AureliaNpmSupport
{
    internal static class TaskTimeoutExtensions
    {
        public static async Task WithTimeout(this Task task, TimeSpan timeoutDelay, string message)
        {
            object obj = (object) task;
            Task[] taskArray = new Task[2]
            {
                task,
                Task.Delay(timeoutDelay)
            };
            if (obj != await Task.WhenAny(taskArray))
                throw new TimeoutException(message);
            obj = (object) null;
            task.Wait();
        }

        public static async Task<T> WithTimeout<T>(
            this Task<T> task,
            TimeSpan timeoutDelay,
            string message)
        {
            object obj = (object) task;
            Task[] taskArray = new Task[2]
            {
                (Task) task,
                Task.Delay(timeoutDelay)
            };
            if (obj != await Task.WhenAny(taskArray))
                throw new TimeoutException(message);
            obj = (object) null;
            return task.Result;
        }
    }
}