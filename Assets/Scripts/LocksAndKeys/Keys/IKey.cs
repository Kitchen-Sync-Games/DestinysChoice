using System.Collections.Generic;
using UnityEngine;

namespace Destiny.LocksAndKeys
{
	/// <summary>
	/// Interface for all keys in the game. A key is a
	/// mechanism that unlocks a lock.
	/// </summary>
	public interface IKey
	{
		bool IsUsed { get; }
		bool AllowMultipleUses { get; set; }

		/// <summary>
		/// A set of unique identifiers. If a lock's ID is
		/// in this set, the key unlocks that lock.
		/// </summary>
		IReadOnlyList<string> LockIds { get; }

		/// <summary>
		/// Checks if lock ID matches LockIds. If it does,
		/// the key is used if it can be.
		/// </summary>
		/// <param name="lockId"></param>
		/// <returns><em>true</em> if kley is used.
		/// <br></br>Otherwise, <em>false</em>.</returns>
		bool UseKey(string lockId);

		/// <summary>
		/// Checks if lock ID matches LockIds.
		/// </summary>
		/// <param name="lockId"></param>
		/// <returns><em>true</em> if ID is in
		/// LockIds list.<br></br>Otherwise, <em>false</em>.</returns>
		bool CheckIdMatch(string lockId);

		/// <summary>
		/// If true is returned, adds the lockId to the LockIds list.
		/// </summary>
		/// <param name="lockId"></param>
		/// <returns><em>false</em> if ID is already in the list or the
		/// given string is null/empty.<br></br>
		/// Otherwise, <em>true</em>.</returns>
		bool AddLockId(string lockId);

		/// <summary>
		/// If true is returned, removes the lockId from the LockIds list.
		/// </summary>
		/// <param name="lockId"></param>
		/// <returns><em>false</em> if ID is not in the list or the
		/// given string is null/empty.<br></br>
		/// Otherwise, <em>true</em>.</returns>
		bool RemoveLockId(string lockId);
	}
}