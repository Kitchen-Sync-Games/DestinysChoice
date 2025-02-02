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

		public List<string> LockIds { get { return lockIds; } }
		[SerializeField]
		protected List<string> lockIds;


		public bool CheckIdMatch(string lockId)
		{
			return lockIds.Contains(lockId);
		}

		public bool UseKey(string lockId)
		{
			bool res = !(isUsed && !allowMultipleUses) && CheckIdMatch(lockId);
			if (res)
			{
				isUsed = true;
			}
			return res;
		}
    }
}