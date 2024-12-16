using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Lockpick : MonoBehaviour
{
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject slider1;
    [SerializeField] private GameObject slider2;
    [SerializeField] private GameObject slider3;
    [SerializeField] private GameObject interactPrompt;
    private GameObject doorObject;
    private Slider currentSlider;
    private int stepSize;
    private int winSpot;
    private string gameState;
    private GameObject goalHandle;
    public void StartGame(GameObject d)
    {
        interactPrompt.SetActive(false);
        GameObject.Find("Player").GetComponent<Player>().paused = true;
        doorObject = d;
        gameUI.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(PlayGame());
    }

    // You can tell I'm tired because my code looks like this:
    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(0.5f);
        Slide(slider1, 2);
        yield return new WaitUntil(() => gameState == "success" || gameState == "fail");
        if (gameState == "fail")
            yield break;
        yield return new WaitForSeconds(0.5f);
        Slide(slider2, 3);
        yield return new WaitUntil(() => gameState == "success" || gameState == "fail");
        if (gameState == "fail")
            yield break;
        yield return new WaitForSeconds(0.5f);
        Slide(slider3, 3);
        yield return new WaitUntil(() => gameState == "success" || gameState == "fail");
        if (gameState == "fail")
            yield break;
        yield return new WaitForSeconds(0.5f);
        // DOOR IS UNLOCKED
        doorObject.GetComponent<Door>().isLocked = false;
        CancelGame();

    }

    void Slide(GameObject slid, int speed)
    {
        stepSize = speed;
        currentSlider = slid.GetComponent<Slider>();
        slid.GetComponent<Slider>().value = 0;
        winSpot = (int)UnityEngine.Random.Range(0, currentSlider.maxValue);
        goalHandle = slid.transform.GetChild(2).GetChild(1).gameObject;
        goalHandle.GetComponent<RectTransform>().anchoredPosition = new Vector2(winSpot, 0);
        slid.SetActive(true);
        gameState = "base";
    }

    void Update()
    {
        //temporary
        if (Input.GetKey(KeyCode.Escape))
        {
            CancelGame();
        }
        if (currentSlider != null && gameState == "base")
        {
            currentSlider.value += stepSize;
            if (currentSlider.value <= currentSlider.minValue)
            {
                stepSize = Math.Abs(stepSize);
            } 
            else if (currentSlider.value >= currentSlider.maxValue)
            {
                stepSize = -Math.Abs(stepSize);
            }
        }
        if (Input.GetKey(KeyCode.Space) && gameState == "base")
        {
            gameState = "stopped";
            if (Math.Abs(currentSlider.value - winSpot) < currentSlider.maxValue/15)
            {
                // play success animation/effect
                gameState = "success";
            }
            else
            {
                gameState = "fail";
                slider2.SetActive(false);
                slider3.SetActive(false);
                // play fail animation/effect
                StartCoroutine(PlayGame());
            }

        }
    }

    public void CancelGame()
    {
        slider1.SetActive(false);
        slider2.SetActive(false);
        slider3.SetActive(false);
        gameUI.SetActive(true);
        gameObject.SetActive(false);
        GameObject.Find("Player").GetComponent<Player>().paused = false;
        GameObject.Find("Player").GetComponent<Player>().isInteracting = false;
    }
}
