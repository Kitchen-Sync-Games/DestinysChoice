using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayGame : MonoBehaviour
{
    public void NextScene()
    {
        print("starting");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
