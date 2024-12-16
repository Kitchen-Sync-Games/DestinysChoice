using UnityEngine;

public class Global : MonoBehaviour
{
    public static int maxClues;
    public static int currentClues;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentClues = 0;
        maxClues = 0;
        DontDestroyOnLoad(gameObject);        
    }

}
