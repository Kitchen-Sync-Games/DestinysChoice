using System;
using UnityEngine;

namespace Destiny.LocksAndKeys
{
    /// <summary>
    /// Interface for all locks in the game. A lock is a
    /// mechanism that somehow restricts the player's
    /// access to something.
    /// </summary>
    public interface ILock
    {
		/// <summary>
		/// A unique identifier. A lock's ID is shared
		/// with the key that unlocks it.
		/// </summary>
		string Id { get; }

		bool IsLocked { get; }


		/// <summary>
		/// Invoked when IsLocked changes. Passes lock ID
		/// and the new state of the lock.
		/// </summary>
		Action<string, bool> OnLockStateChanged { get; set; }

        /// <summary>
        /// Invoked when IsLocked changes and is set to
        /// false.
        /// </summary>
        Action<string> OnUnlock { get; set; }

		/// <summary>
		/// Invoked when IsLocked changes and is set to
		/// true.
		/// </summary>
		Action<string> OnLock { get; set; }

        void Setup();

		void Lock();

		void Unlock();

        /// <summary>
        /// Compares the given key's LockIds to this lock's
        /// ID. If there's a match, the lock is unlocked.
        /// </summary>
        /// <param name="key"></param>
        /// <returns><em>true</em> if the end state is
        /// unlocked.<br></br>Otherwise, <em>false</em>.</returns>
        bool TryUnlock(IKey key);
    }
}