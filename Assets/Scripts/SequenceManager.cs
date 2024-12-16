using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SequenceManager : MonoBehaviour
{
    [SerializeField] private NarrationManager narrationManager; // Reference to the NarrationManager
    [SerializeField] private SoundManager soundManager;         // Reference to the SoundManager
    
    [SerializeField] private List<NarrationSequence> sequences;
    [SerializeField] private GameObject player;

    
    private Vector3 worldPosition;
    
    public void PlaySequence(string sequenceID, Vector3 position)
    {
        // Load the ScriptableObject based on the sequenceID
        NarrationSequence sequence = sequences.Find(x => x.narrationTitle == sequenceID);
            //Resources.Load<NarrationSequence>("Sequences/" + sequenceID);
        worldPosition = position;
        if (sequence != null)
        {
            StartCoroutine(PlayNarrationSequence(sequence));
        }
        else
        {
            Debug.LogWarning($"Sequence with ID '{sequenceID}' not found.");
        }
    }

    private IEnumerator PlayNarrationSequence(NarrationSequence sequence)
    {
        float lastTime = 0f;
        NarrationStep lastStep = sequence.narrationSteps.Last();
        foreach (NarrationStep step in sequence.narrationSteps)
        {
            // Wait for the specified delay before showing the narration and playing the audio
            yield return new WaitForSeconds(step.delay + lastTime);
            lastTime = step.lifeTime;
            // Show the narration text at a desired position (e.g., Vector3.zero)
            Vector3 position = Vector3.zero; // Adjust as necessary for each narration position
            narrationManager.ShowNarration(step.narrationText, position, step.lifeTime);

            // Play the audio clip
            if (step.audioClip != null)
            {
                soundManager.PlaySoundAtPosition(step.audioClip, worldPosition);
            }
            if (step.isPrompt)
            {
                yield return new WaitForSeconds(step.lifeTime);
                narrationManager.ShowChoices(step.choice1, step.choice2);
                yield return new WaitUntil(() => narrationManager.playerchoice != 0);
                if (narrationManager.playerchoice == 1)
                {
                    PlaySequence(step.choice1ResultingSequence, player.transform.position);
                }
                else if (narrationManager.playerchoice == 2)
                {
                    PlaySequence(step.choice2ResultingSequence, player.transform.position);
                }
            }
            else if (step == lastStep)
            {
                player.GetComponent<Player>().cutScene = false;
            }
        }
    }
}