using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
	public static class TaskExtensions
	{
		public static Task ContinueWithOnMainThread<T>(this Task<T> task, Action<Task<T>> action)
		{
			return task.ContinueWith(action, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
