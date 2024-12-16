using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SequenceManager sequenceManager;
    [SerializeField] private GameObject player;
    [SerializeField] private string startingSequence;
    [SerializeField] private bool startsWithVoice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(startsWithVoice)
            StartCoroutine(WaitForTime());
    }



    private IEnumerator WaitForTime()
    {
        yield return new WaitForSeconds(.5f);
        sequenceManager.PlaySequence(startingSequence, player.transform.position );    
    }
}
