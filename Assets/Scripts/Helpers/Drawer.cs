using UnityEngine;

public class Drawer : MonoBehaviour
{
    
    private Animator drawerAnimator;

    private bool isOpen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        drawerAnimator = GetComponent<Animator>();
    }
   
    public void OpenDrawer()
    {
        isOpen = !isOpen;
        drawerAnimator.SetBool("IsOpen", isOpen);
    }
}
