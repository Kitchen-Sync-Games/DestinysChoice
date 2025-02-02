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
		List<string> LockIds { get; }

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
	}
}