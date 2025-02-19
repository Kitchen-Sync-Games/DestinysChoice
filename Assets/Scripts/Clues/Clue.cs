using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class Clue : MonoBehaviour, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Interact(){
        dissapear();
    }

    private void dissapear(){
        gameObject.SetActive(false);
    }

}
