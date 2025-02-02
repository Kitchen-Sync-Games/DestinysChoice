using System;
using UnityEngine;
using UnityEngine.Events;

namespace Destiny.LocksAndKeys
{
	/// <summary>
	/// Abstract parent class for any locks.
	/// </summary>
	public abstract class LockBase : MonoBehaviour, ILock
	{
		public string Id { get { return id; } }
		[SerializeField, Header("Basic Lock"), Space(5)]
		protected string id;

		public bool IsLocked { get { return isLocked; } }
		[SerializeField]
		protected bool isLocked;

		public Action<string, bool> OnLockStateChanged { get; set; }
		[SerializeField,
			Tooltip("Invoked when IsLocked changes. Passes lock ID and the new state of the lock."),
			Space(15)]
		protected UnityEvent<string, bool> onLockStateChangedUnityEvent;

		public Action<string> OnUnlock { get; set; }
		[SerializeField, Tooltip("Invoked when IsLocked changes and is set to false.")]
		protected UnityEvent<string> onUnlockUnityEvent;

		public Action<string> OnLock { get; set; }
		[SerializeField, Tooltip("Invoked when IsLocked changes and is set to true.")]
		protected UnityEvent<string> onLockUnityEvent;

		public void Lock()
		{
			bool lockChanged = !isLocked;
			isLocked = true;
			if (lockChanged)
			{
				fireLockingEvent();
			}
		}

		public void Unlock()
		{
			bool lockChanged = isLocked;
			isLocked = false;
			if (lockChanged)
			{
				fireLockingEvent();
			}
		}

		public virtual bool TryUnlock(IKey key)
		{
			if (!isLocked)
			{
				return true;
			}

			bool res = key.UseKey(Id);
			if (res)
			{
				Unlock();
			}
			return res;
		}

		private void fireLockingEvent()
		{
			if (isLocked)
			{
				OnLock?.Invoke(Id);
				onLockUnityEvent?.Invoke(Id);
			}
			else
			{
				OnUnlock?.Invoke(Id);
				onUnlockUnityEvent?.Invoke(Id);
			}

			OnLockStateChanged?.Invoke(Id, isLocked);
			onLockStateChangedUnityEvent?.Invoke(Id, isLocked);
		}
	}
}