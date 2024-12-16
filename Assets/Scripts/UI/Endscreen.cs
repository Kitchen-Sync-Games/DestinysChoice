using TMPro;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    [SerializeField] private TMP_Text clues;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clues.text = Global.currentClues+"/"+Global.maxClues+" Clues Found";  
    }

}
