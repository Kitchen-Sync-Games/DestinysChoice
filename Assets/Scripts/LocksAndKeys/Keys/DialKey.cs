using System.Collections.Generic;
using UnityEngine;

namespace Destiny.LocksAndKeys
{
	public class DialKey : KeyBase
    {
        // TODO: May want to do some research on localization and avoid
        // some of the magic values in this class. Especially characters.

        public int NotchCount { 
            get
            {
                return endNum - startNum + 1;
            }
        }

        public bool UsingChars { get { return usingChars; } }
        protected bool usingChars;
        [SerializeField]
        protected bool charsIsDirty = false;

        public int StartNum { get { return startNum; } }
		[SerializeField]
		protected int startNum = 0;
        public int EndNum { get { return endNum; } }
        [SerializeField]
        protected int endNum = 39;

        [SerializeField]
        protected GameObject notchPrefab;
        [SerializeField]
        protected Transform notchParent;

        protected List<DialNotch> notches = new();

        /// <summary>
        /// Tries to set StartNum to newStartNum. If UsingChars is <em>true</em>, converts
        /// a character a-z to its uppercase
        /// </summary>
        /// <param name="newStartNum"></param>
        /// <returns><em>true</em> if StartNum is set successfully.<br></br>
        /// <em>false</em> if:<br></br>
        /// <list type="bullet">
        /// <item>UsingChars is <em>true</em> and newStartNum is not a character a-Z</item>
        /// <item>newStartNum is greater than EndNum</item>
        /// </list></returns>
        public bool SetStartNum(int newStartNum)
        {
            if (UsingChars)
            {
                if (newStartNum < 'a' || newStartNum > 'Z')
                {
                    return false;
                }
                else if (newStartNum < 'A')
                {
                    newStartNum = convertLowerCharToUpper(newStartNum) ;
                }
            }

            if (newStartNum > endNum)
            {
                return false;
            }

            startNum = newStartNum;
            UpdateNotches();
            return true;
        }

		/// <summary>
		/// Tries to set EndNum to newEndNum. If UsingChars is <em>true</em>, converts
		/// a character a-z to its uppercase
		/// </summary>
		/// <param name="newEndNum"></param>
		/// <returns><em>true</em> if EndNum is set successfully.<br></br>
		/// <em>false</em> if:<br></br>
		/// <list type="bullet">
		/// <item>UsingChars is <em>true</em> and newEndNum is not a character a-Z</item>
		/// <item>newEndNum is less than StartNum</item>
		/// </list></returns>
		public bool SetEndNum(int newEndNum)
		{
			if (UsingChars)
			{
				if (newEndNum < 'a' || newEndNum > 'Z')
				{
					return false;
				}
				else if (newEndNum < 'A')
				{
					newEndNum = convertLowerCharToUpper(newEndNum);
				}
			}

			if (newEndNum < startNum)
			{
				return false;
			}

			endNum = newEndNum;
			UpdateNotches();
			return true;
		}

		public void UpdateNotches()
        {
            if (!notchUpdateRequired())
            {
                return;
            }

            if (notches.Count > NotchCount)
            {
                notches.RemoveRange(NotchCount, notches.Count - NotchCount);
            }
            else if (notches.Count < NotchCount)
            {
                int notchListCount = notches.Count;
                for (int i = 0; i < NotchCount - notchListCount; i++)
                {
                    notches.Add(Instantiate(notchPrefab, notchParent).GetComponent<DialNotch>());
                }
            }

            for (int i = 0; i < notches.Count; i++)
            {
                DialNotch notch = notches[i];
                // Call a notch function to set its rotation. Gotta look at anchors and pivots.
            }
        }

        private bool notchUpdateRequired()
        {
            return notches.Count != NotchCount || charsIsDirty;
		}

        private int convertLowerCharToUpper(int character)
        {
            return character + 'Z' - 'A' + 1;

		}
    }
}