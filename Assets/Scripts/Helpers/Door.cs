using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private NarrationManager narrationManager;
    [SerializeField] private SequenceManager sequenceManager;
    public bool isLocked;
    private ToolUIManager ToolUI;
    private Lockpick lockUI;
    private Animator doorAnimator;
    private bool isDoorOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (narrationManager == null)
        {
            narrationManager = GameObject.Find("NarrationManager").GetComponent<NarrationManager>();
        }
        if (sequenceManager == null)
        {
            sequenceManager = GameObject.Find("SequenceController").GetComponent<SequenceManager>();

        }
        ToolUI = GameObject.Find("ToolUIPanel").GetComponent<ToolUIManager>();
        lockUI = GameObject.Find("UI").transform.GetChild(2).GetComponent<Lockpick>();
        doorAnimator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        if (isLocked)
        {
            if(ToolUI.m_is_lockpick_unlocked)
            {
                lockUI.StartGame(gameObject);
            }
            else
            {
                sequenceManager.PlaySequence("NoLockpick", transform.position);
                StartCoroutine(WaitforInt());
            }
        }
        else
        {
            isDoorOpen = !isDoorOpen;
            doorAnimator.SetBool("IsOpen", isDoorOpen);
            StartCoroutine(WaitforInt());
        }
    }
    IEnumerator WaitforInt()
    {
        yield return new WaitForSeconds(1f);
        //GameObject.Find("Player").GetComponent<Player>().inpu = false;
    }
}
