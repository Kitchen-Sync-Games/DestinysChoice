using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class CluesManager : MonoBehaviour
{

    public static event Action OnPlayerFoundClue;
    public static event Action OnPlayerFoundAllClues;
    
    [SerializeField] private TMP_Text mFoundCluesValue;
    private AudioSource clueFound;
    private int mMaxClueCount = 0;
    private int mFoundCluesCount = 0;   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetClueCountValue(GameObject.FindGameObjectsWithTag("Clues").Count());
        Player.OnPlayerInteraction += OnPlayerInteraction;
    }


    private void OnPlayerInteraction(string tag)
    {
        if (string.CompareOrdinal(tag, "Clues") == 0)
        {
            mFoundCluesCount++;
            Global.currentClues++;
            mFoundCluesValue.text = $"{mFoundCluesCount}/{mMaxClueCount}";
        }

    }


    public void SetClueCountValue(int totalClues)
    {
        mMaxClueCount = totalClues;
        mFoundCluesValue.text = $"{mFoundCluesCount}/{totalClues}";
        Global.maxClues += totalClues;
    }
}
