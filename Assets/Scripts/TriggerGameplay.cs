using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TriggerGameplay : MonoBehaviour
{
    [SerializeField] private GameObject tbc;
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            hit.GetComponent<Player>().cutScene = true;
            StartCoroutine(hit.GetComponent<Player>().SpookyLights());
            StartCoroutine(EndDemo());
        }
    }
    IEnumerator EndDemo()
    {
        yield return new WaitForSeconds(8);
        tbc.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
