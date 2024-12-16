using System.Collections;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public bool isRinging = false;
    AudioSource ringingSound;
    [SerializeField] private SequenceManager sequenceManager;

    [SerializeField] private string seqID;
    void Start()
    {
        ringingSound = GetComponent<AudioSource>();
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.Find("SequenceController").GetComponent<SequenceManager>();

        }
    }

    public void AnswerPhone()
    {
        isRinging = false;
        AudioSource ringingSound = GetComponent<AudioSource>();
        ringingSound.Stop();
        gameObject.layer = LayerMask.NameToLayer("Default");
        sequenceManager.PlaySequence(seqID, transform.position);
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white * 0);
    }

    public void Ring() {

        isRinging = true;
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        StartCoroutine(PlayRing());
        GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red * 3);
    }
    IEnumerator PlayRing()
    {
        while (isRinging)
        {
            ringingSound.Play();
            yield return new WaitForSeconds(2);
        }
    }
}
