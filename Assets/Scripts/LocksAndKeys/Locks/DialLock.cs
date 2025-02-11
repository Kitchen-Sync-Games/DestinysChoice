using PixelCrushers.DialogueSystem.Articy.Articy_4_0;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace Destiny.LocksAndKeys
{
	public class DialLock : LockBase
	{
		[SerializeField, Header("Dial Lock"), Space(5)]
		private DialKey dial;
		[SerializeField]
		private string[] correctCode;
		[SerializeField]
		private TextMeshProUGUI curValText;

		private void Start()
		{
			Setup();
		}

		public override void Setup()
		{
			bind();
			dial.Setup(Id, correctCode.Length);
		}

		public void ResetDialLock()
		{
			dial.ResetDial();
		}

		private void bind()
		{
			unbind();
			dial.OnCodeInputed += handleCodeInputed;
			dial.OnCurrentValueChanged += handleCurrentValueChanged;
		}

		private void unbind()
		{
			dial.OnCodeInputed -= handleCodeInputed;
			dial.OnCurrentValueChanged -= handleCurrentValueChanged;
		}

		private void handleCodeInputed(string[] code)
		{
			if (code.Matches(correctCode))
			{
				TryUnlock(dial);
			}
		}

		private void handleCurrentValueChanged(string newVal)
		{
			curValText.text = newVal;
		}
	}
}