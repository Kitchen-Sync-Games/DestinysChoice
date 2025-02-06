using UnityEngine;

public class CluePickup : MonoBehaviour
{
   [SerializeField] private GameObject ClueHolder;

   void pickupClue(){
        ClueHolder.SetActive(true);
   }

   void putAwayClue(){
        ClueHolder.SetActive(false);
   }

    //rotates clue in x and y directions
   void RotateClue()
{
    float rotX = Input.GetAxis("Mouse X") * 5f;
    float rotY = Input.GetAxis("Mouse Y") * 5f;

    ClueHolder.transform.Rotate(Vector3.up, -rotX, Space.World);
    ClueHolder.transform.Rotate(Vector3.right, rotY, Space.World);
}


}
