using System;
using System.Linq;
using System.Collections.Generic;
using SFGL.Utils;

namespace SFGL.Time
{
	public class Timer : IUpdateable
	{
		#region Variables

		class TimerInfo
		{
			public RepeatingTimerEvent Event;
			public GameTime StartTime;
			public GameTime Time;
		}

		public delegate bool RepeatingTimerEvent();
		public delegate void TimerEvent();
		private readonly Dictionary<int, TimerInfo> Timers = new Dictionary<int, TimerInfo>();
		private readonly Random Random = new Random();

		#endregion

		#region General functions

		public int NextFrame(TimerEvent callback)
		{
			return Add(new TimerInfo
				{
					Event = () => { callback(); return true; }
				});
		}

		public int EveryFrame(RepeatingTimerEvent callback)
		{
			return Add(new TimerInfo{ Event = callback });
		}

		public int After(GameTime time, TimerEvent callback)
		{
			return Add(new TimerInfo
				{
					Event = () => { callback(); return true; },
					Time = time
				});
		}

		public int Every(GameTime time, RepeatingTimerEvent callback)
		{
			return Add(new TimerInfo
				{
					Event = callback,
					StartTime = time,
					Time = time
				});
		}

		public void Cancel(int id)
		{
			lock (Timers)
				Timers.Remove(id);
		}

		public bool Exists(int id)
		{
			lock (Timers)
				return Timers.ContainsKey(id);
		}

		public void Update(GameTime gameTime)
		{
			lock (Timers)
			{
				var readonlyTimers = Timers.Values.ToList();
				var toRemove = new List<TimerInfo>();

				foreach (var timer in readonlyTimers)
				{
					timer.Time -= gameTime;

					if (timer.Time.Ticks > 0)
						continue;

					if (timer.Event())
						toRemove.Add(timer);
					else
						timer.Time = timer.StartTime;
				}

				Timers.RemoveAll(kv => toRemove.Contains(kv.Value));
			}
		}

		#endregion

		#region Helpers

		private int Add(TimerInfo info)
		{
			lock (Timers)
			{
				var id = GenerateId();
				Timers.Add(id, info);
				return id;
			}
		}

		private int GenerateId()
		{
			var res = 0;
			do res = Random.Next();
			while (Timers.ContainsKey(res));
			return res;
		}

		#endregion
	}
}