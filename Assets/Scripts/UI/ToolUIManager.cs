using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ToolUIManager : MonoBehaviour
{
    //TODO update textures on the game tool panel when one is selected set non selected back to normal

    public static Action<string> OnNewToolSelected;
    
    public enum SelectableTools
    {
        None, 
        Flashlight,
        Lockpick    
    }


    [SerializeField] private Button mFlashlightButton;
    [SerializeField] private Button mLockPickButotn;
    [SerializeField] private TMP_Text mFlashlightText;
    [SerializeField] private TMP_Text mLockpickText;
    
    public SelectableTools currentSelectTool = SelectableTools.None;

    private bool m_is_flashlight_unlocked = false;
    public bool m_is_lockpick_unlocked = false;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        Player.OnPlayerInteraction += OnPlayerInteractedWithTool;
    }

    private void OnLockpickButtonClicked()
    {
        if(!m_is_lockpick_unlocked) return;
        
        if(currentSelectTool != SelectableTools.Lockpick)
            UpdateCurrentlySelectedTool(SelectableTools.Lockpick);

    }

    private void OnFlashlightButtonClicked()
    {
        
        if (!m_is_flashlight_unlocked) return;
        if(currentSelectTool != SelectableTools.Flashlight)
            UpdateCurrentlySelectedTool(SelectableTools.Flashlight);
    }

    
    private void UpdateCurrentlySelectedTool(SelectableTools tool)
    {
        currentSelectTool = tool;
        OnNewToolSelected?.Invoke(tool.ToString());
    }


    private void OnPlayerInteractedWithTool(string tool)
    {
        switch (tool)
        {
            case "Flashlight":
                m_is_flashlight_unlocked = true;
                break;
            case "Lockpick":
                m_is_lockpick_unlocked = true;
                break;
            default:
                break;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnFlashlightButtonClicked();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnLockpickButtonClicked();
        }
    }

}
