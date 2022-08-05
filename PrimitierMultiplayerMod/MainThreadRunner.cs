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
		private static ConcurrentQueue<Action> Todo = new ConcurrentQueue<Action>();

		public static void RunQueuedTasks()
		{
			while (Todo.Count > 0)
			{
				
				if(Todo.TryDequeue(out var task))
				{
					task.Invoke();
				}
				

			}
			

		}

		public static void EnqueueTask(Action action)
		{
			if (action == null)
				return;

			Todo.Enqueue(action);
		}

	}
}
