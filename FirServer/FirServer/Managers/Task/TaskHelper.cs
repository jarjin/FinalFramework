using System;
using System.Threading;

namespace FirServer.Managers
{
    public static class TaskHelper
    {
        public static void AddThread(this ITaskModule module, string taskName, Action<Guid> taskAction)
        {
            var thread = new Thread(() => taskAction(module.ThreadKey)) {Name = taskName};
            module.ThreadList.Add(thread);
        }

        public static void StartThread(this ITaskModule module)
        {
            foreach (var thread in module.ThreadList)
                thread.Start();
        }
    }
}