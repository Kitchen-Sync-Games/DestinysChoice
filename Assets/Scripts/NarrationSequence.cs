using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NarrationStep
{
    [TextArea]
    public string narrationText;  // Text to be displayed for this step
    public AudioClip audioClip;   // Audio clip to be played in this step
    public float delay;           // Delay in seconds before this step is triggered
    public float lifeTime;
    public bool isPrompt;
    public string choice1;
    public string choice1ResultingSequence;
    public string choice2;
    public string choice2ResultingSequence;
}

[CreateAssetMenu(fileName = "NarrationSequence", menuName = "Game/NarrationSequence")]
public class NarrationSequence : ScriptableObject
{
    public string narrationTitle;
    public List<NarrationStep> narrationSteps;  // List of narration steps in this sequence
}