using System.Collections.Generic;
using UnityEngine;

namespace Destiny.LocksAndKeys
{
    public abstract class KeyBase : MonoBehaviour, IKey
    {
		public bool IsUsed { get { return isUsed; } }
		[SerializeField, Header("Basic Key"), Space(5)]
		protected bool isUsed;

		public bool AllowMultipleUses { get { return allowMultipleUses; } set { allowMultipleUses = value; } }
		[SerializeField]
		protected bool allowMultipleUses;

		public IReadOnlyList<string> LockIds => lockIds.AsReadOnly();
		[SerializeField]
		protected List<string> lockIds = new();

		private void Start()
		{
			lockIds = lockIds.RemoveDuplicates();
		}


		public bool CheckIdMatch(string lockId)
		{
			return lockIds.Contains(lockId);
		}

		public virtual bool UseKey(string lockId)
		{
			bool res = !(isUsed && !allowMultipleUses) && CheckIdMatch(lockId);
			if (res)
			{
				isUsed = true;
			}
			return res;
		}

		public bool AddLockId(string lockId)
		{
			if (lockId.IsNullOrEmpty())
				return false;

			if (CheckIdMatch(lockId))
				return false;

			lockIds.Add(lockId);
			return true;
		}

		public bool RemoveLockId(string lockId)
		{
			if (lockId.IsNullOrEmpty())
				return false;

			return lockIds.Remove(lockId);
		}
    }
}