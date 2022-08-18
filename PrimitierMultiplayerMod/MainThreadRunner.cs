using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class MainThreadRunner
	{
		private static ConcurrentQueue<Action> Tasks = new ConcurrentQueue<Action>();

		public static void RunQueuedTasks()
		{
			while (Tasks.Count > 0)
			{
				
				if(Tasks.TryDequeue(out var task))
				{
					task.Invoke();
				}
				

			}
			

		}

		public static void EnqueueTask(Action action)
		{
			if (action == null)
				return;

			Tasks.Enqueue(action);
		}

	}
}
