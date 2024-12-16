using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private SequenceManager sequenceManage;
    [SerializeField] private string seqId;
    [SerializeField] private bool multiuse = false;
    private bool used;

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player") && (!used || multiuse))
        {
            sequenceManage.PlaySequence(seqId, hit.transform.position);
            used = true;
        }
    }
}
