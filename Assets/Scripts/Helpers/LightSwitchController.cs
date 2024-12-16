using System.Collections;
using UnityEngine;

public class LightSwitchController : MonoBehaviour
{

    [SerializeField] private Light lightObject;
    private Animator lightSwitchAnimator;
    private bool lightIsOff;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player.OnPlayerInteraction += OnPlayerInteraction;
        lightSwitchAnimator = GetComponent<Animator>();
    }
    
    private void OnPlayerInteraction(string tag)
    {
        if (string.CompareOrdinal(tag, "LightSwitch") == 0)
        {
            Debug.Log("Turning on light");
            lightIsOff = !lightIsOff;    
            lightSwitchAnimator.SetBool("OnOff", lightIsOff);
            StartCoroutine(WaitToToggleLight());
        }

    }

    private IEnumerator WaitToToggleLight()
    {
        yield return new WaitForSeconds(1f);
        lightObject.enabled = lightIsOff;
    }
    
}
