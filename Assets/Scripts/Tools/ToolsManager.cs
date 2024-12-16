using System;
using UnityEngine;

public class ToolsManager : MonoBehaviour
{
    public enum UsableTools
    {
        None, 
        Flashlight,
        Lockpick,
    }

    [SerializeField] private GameObject mFlashlightObject;
    [SerializeField] private GameObject mLockpickObject;
    
    
    private UsableTools mActiveTool  = UsableTools.None;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ToolUIManager.OnNewToolSelected += NewToolSelected;
    }

    private void OnDisable()
    {
        ToolUIManager.OnNewToolSelected -= NewToolSelected;
    }


    private void NewToolSelected(string tool)
    {
        switch (tool)
        {
            case "Flashlight":
                mActiveTool = UsableTools.Flashlight;
                mFlashlightObject.gameObject.SetActive(true);
                mLockpickObject.gameObject.SetActive(false);
                break;
            case "Lockpick":
                mActiveTool = UsableTools.Lockpick;
                mFlashlightObject.gameObject.SetActive(false);
                mLockpickObject.gameObject.SetActive(true);
                break;
            default:
                mActiveTool = UsableTools.None;
                mFlashlightObject.gameObject.SetActive(false);
                mLockpickObject.gameObject.SetActive(false);
                break;
        }
    }


}
