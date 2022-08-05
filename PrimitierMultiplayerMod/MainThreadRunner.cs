using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitierMultiplayerMod
{
	public static class MainThreadRunner
	{
		private static Queue<Action> Todo = new Queue<Action>();

		public static void RunQueuedTasks()
		{
			foreach (var task in (Todo as IEnumerable<Action>))
			{
				task.Invoke();
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
