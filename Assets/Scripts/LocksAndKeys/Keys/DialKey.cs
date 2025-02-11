using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Destiny.LocksAndKeys
{
	public class DialKey : KeyBase, IPointerEnterHandler, IPointerExitHandler
    {
        // TODO: May want to do some research on localization and avoid
        // some of the magic values in this class. Especially characters.

        private const float DEMO_RADIUS = 350;

        public Action<string[]> OnCodeInputed;
        public Action<string> OnCurrentValueChanged;
        public Action<string> OnValueAddedToCode;

        public int NotchCount { 
            get
            {
                return endNum - startNum + 1;
            }
        }

        public bool UsingChars { get { return usingChars; } }
        [SerializeField]
        protected bool usingChars;
        [SerializeField]
        protected bool charsIsDirty = false;

        public int CodeLength = 3;

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
        [SerializeField]
        protected float notchWidth = 20;

        [SerializeField]
        protected RectTransform arrow;
        protected Transform arrowDetector;

        private bool mouseOver = false;

        protected DialNotch curNotch;
        protected string[] currentCode;

        private int curDir; // -1 is counterclockwise; 1 is clockwise; 0 is initial state
        private int initDir = 1;

        private void Update()
        {
            if (mouseOver && Input.GetMouseButton(0))
            {
                rotateByMouse();
            }
            else if (Input.mouseScrollDelta.y != 0)
            {
                rotateByScroll();
            }
            else if (Input.GetKey(KeyCode.D)
                || Input.GetKey(KeyCode.RightArrow))
            {
                rotateByKey(true);
            }
            else if (Input.GetKey(KeyCode.A)
                || Input.GetKey(KeyCode.LeftArrow))
            {
                rotateByKey(false);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                attemptUnlock();
            }
		}

        public void Setup(string lockId, int codeLength)
        {
            AddLockId(lockId);
            CodeLength = codeLength;
            currentCode = new string[CodeLength];
            curDir = 0;
            arrowDetector = arrow.transform.GetChild(0).GetComponent<RectTransform>();
            Vector2 centerArrowVec = arrow.transform.position - transform.position;
            arrowDetector.position = ((Vector2)transform.position) + (centerArrowVec.normalized * DEMO_RADIUS);
            (arrowDetector as RectTransform).sizeDelta = arrow.sizeDelta;
            UpdateNotches();
        }

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
                for (int i = NotchCount; i < notches.Count; i++)
                {
                    Destroy(notches[i].gameObject);
                }
                notches.RemoveRange(NotchCount, notches.Count - NotchCount);
            }
            else if (notches.Count < NotchCount)
            {
                int notchListCount = notches.Count;
                for (int i = 0; i < NotchCount - notchListCount; i++)
                {
                    DialNotch newNotch = Instantiate(notchPrefab, notchParent).GetComponent<DialNotch>();
                    int notchNum = startNum + i + notchListCount;
					newNotch.Setup(DEMO_RADIUS, 20, usingChars ? ((char)notchNum).ToString() : notchNum.ToString());
					notches.Add(newNotch);
                }
            }

            for (int i = 0; i < notches.Count; i++)
            {
                DialNotch notch = notches[i];
                notch.SetPositionAndRotation(i, notches.Count);
            }
			updateCurNotch();
		}

		private bool notchUpdateRequired()
        {
            return notches.Count != NotchCount || charsIsDirty;
		}

        private int convertLowerCharToUpper(int character)
        {
            return character + 'Z' - 'A' + 1;

		}
        
        // WARNING! Lots of magic numbers below here.

        private void rotateByMouse()
        {
            int modX = Input.mousePosition.y >= transform.position.y ? -1 : 1;
            rotateMechanism(modX * Input.mousePositionDelta.x / Screen.width * 360);
        }

        private void rotateByScroll()
        {
            rotateMechanism(Input.mouseScrollDelta.y * 3);
        }

        private void rotateByKey(bool clockwise)
        {
            int mod = clockwise ? -1 : 1;
            rotateMechanism(0.25f * mod);
        }

        private void rotateMechanism(float degrees)
        {
            transform.Rotate(0, 0, degrees);
            if (degrees != 0)
            {
				if (curDir == 0)
				{
                    int dir = (int)(-degrees / Mathf.Abs(degrees));
                    if(dir == initDir)
					    curDir = dir;
				}
                else
                {
                    bool sameDir = curDir / Mathf.Abs(curDir) == (int)(-degrees / Mathf.Abs(degrees));
                    if (!sameDir)
                    {
                        curDir *= -1;
                        addToCode();
                    }
				}
			}
            updateCurNotch();
        }

        private void addToCode()
        {
            int i = 0;
            for (; i < currentCode.Length; i++)
            {
                if (currentCode[i] == null)
                {
                    currentCode[i] = curNotch.Val;
                    OnValueAddedToCode?.Invoke(currentCode[i]);
                    break;
                }
            }
            if (currentCode.Length == i + 1)
                inputCode();
        }

        protected void updateCurNotch()
        {
            int start = 0;
            int count = 0;
            if (curNotch)
            {
                start = notches.IndexOf(curNotch);
            }

            if (isNotchAlignedWithArrow(notches[start]))
            {
                if (!curNotch)
                {
					setCurrentNotch(notches[start]);
				}
                return;
            }
            DialNotch nextNotch = null;
            count++;

            for (int i = 1; count < notches.Count; i++)
            {
                int leftIndex = start - i < 0 ? notches.Count + (start - i) : start - i;
                int rightIndex = start + i >= notches.Count ? (start + i) - notches.Count : start + i;
                if (isNotchAlignedWithArrow(notches[leftIndex]))
                {
					nextNotch = notches[leftIndex];
                    break;
                }
                count++;
                if (isNotchAlignedWithArrow(notches[rightIndex]))
                {
                    nextNotch = notches[rightIndex];
                    break;
                }
                count++;
            }

            setCurrentNotch(nextNotch);
        }

        private bool isNotchAlignedWithArrow(DialNotch notch)
        {
            float notchArea = (notch.NotchSatellite as RectTransform).rect.size.x * (notch.NotchSatellite as RectTransform).rect.size.y;
            float detectorArea = (arrowDetector as RectTransform).rect.size.x * (arrowDetector as RectTransform).rect.size.y;

            RectTransform small = Mathf.Min(notchArea, detectorArea) == notchArea ? (notch.NotchSatellite as RectTransform) : (arrowDetector as RectTransform);
            RectTransform large = small.Equals(notch.NotchSatellite as RectTransform) ? (arrowDetector as RectTransform) : (notch.NotchSatellite as RectTransform);

            Vector2[] smallCoordinates = new Vector2[4] { 
                small.rect.min + (Vector2)small.position,
                small.rect.max + (Vector2)small.position,
                new Vector2(small.rect.xMax, small.rect.yMin) + (Vector2)small.position,
                new Vector2(small.rect.xMin, small.rect.yMax) + (Vector2)small.position };

            foreach (Vector2 point in smallCoordinates)
            {
                if (large.rect.xMax + large.position.x >= point.x && large.rect.xMin + large.position.x <= point.x
                    && large.rect.yMax + large.position.y >= point.y && large.rect.yMin + large.position.y <= point.y)
                {
                    return true;
                }
            }

			return false;
        }

        private void attemptUnlock()
        {
            if (currentCode[currentCode.Length - 2] != null)
                addToCode();
        }

        private void inputCode()
        {
            OnCodeInputed?.Invoke(currentCode);
            ResetDial();
        }

		public void ResetDial()
		{
            curDir = 0;
            currentCode = new string[CodeLength];
            updateCurNotch();
		}

		private void setCurrentNotch(DialNotch notch)
        {
            if (notch)
                curNotch = notch;
			OnCurrentValueChanged?.Invoke(notch ? curNotch.Val : "");
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
            mouseOver = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
            mouseOver = false;
		}
	}
}