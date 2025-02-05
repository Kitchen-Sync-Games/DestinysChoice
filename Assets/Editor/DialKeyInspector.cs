using Destiny.LocksAndKeys;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(DialKey))]
public class DialKeyInspector : Editor
{
	const string START_NUM_LABEL = "Start Number";
	const string END_NUM_LABEL = "End Number";
	const string START_CHAR_LABEL = "Start Character";
	const string END_CHAR_LABEL = "End Character";
	const string USING_CHAR_LABEL = "Using Characters";


	SerializedProperty startNumProp;
	int startNum;
	string startString;

	SerializedProperty endNumProp;
	int endNum;
	string endString;

	SerializedProperty usingCharsProp;
	bool usingChars;
	bool previousCharsState;

	SerializedProperty useCharsDirtyProp;

	private void OnEnable()
	{
		startNumProp = serializedObject.FindProperty("startNum");
		startNum = startNumProp.intValue;

		endNumProp = serializedObject.FindProperty("endNum");
		endNum = endNumProp.intValue;

		usingCharsProp = serializedObject.FindProperty("UsingChars");
		usingChars = previousCharsState = usingCharsProp.boolValue;

		useCharsDirtyProp = serializedObject.FindProperty("charsIsDirty");

		startString = "";
		endString = "";
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		bool startChanged = false;
		bool endChanged = false;

		usingChars = EditorGUILayout.Toggle(USING_CHAR_LABEL, usingChars);
		if (usingChars)
		{
			if (previousCharsState != usingChars)
			{
				if (startNum < 'A')
				{
					startNum = 'A';
				}
				else if (startNum > 'Z')
				{
					startNum = 'Z';
				}
				if (endNum > 'Z')
				{
					endNum = 'Z';
				}
				else if (endNum < 'A')
				{
					endNum = 'A';
				}
				startString = ((char)startNum).ToString();
				endString = ((char)endNum).ToString();
			}

			string newStartString = EditorGUILayout.TextField(START_CHAR_LABEL, startString).ToUpper();
			startChanged = !newStartString.Equals(startString);
			string newEndString = EditorGUILayout.TextField(END_CHAR_LABEL, endString).ToUpper();\
			endChanged = !newEndString.Equals(endString);

			if (startString.Length > 1)
			{
				startString = startString[0].ToString();
			}
			if (endString.Length > 1)
			{
				endString = endString[0].ToString();
			}

			if (startString.Length > 0)
			{
				startNum = startString[0];
			}
			else
			{
				startNum = endNum;
			}
			if (endString.Length > 0)
			{
				endNum = endString[0];
			}
			else
			{
				endNum = startNum;
			}

			if (startNum < 'A')
			{
				startNum = 'A';
			}
			if (endNum > 'Z')
			{
				endNum = 'Z';
			}
		}
		else
		{
			if (previousCharsState != usingChars)
			{
				startNum = 0;
				endNum = endString[0] - startString[0];
			}
			startNum = EditorGUILayout.IntField(START_NUM_LABEL, startNum);
			endNum = EditorGUILayout.IntField(END_NUM_LABEL, endNum);
		}
		usingCharsProp.boolValue = usingChars;


		if (startNum > endNum)
		{
			startNum = endNum;
		}
		startNumProp.intValue = startNum;

		
		if (endNum < startNum)
		{
			endNum = startNum;
		}
		endNumProp.intValue = endNum;

		if (previousCharsState != usingChars
			|| startChanged
			|| endChanged)
		{
			useCharsDirtyProp.boolValue = previousCharsState != usingChars;
			((DialKey)target).UpdateNotches();
		}

		previousCharsState = usingChars;
	}
}
