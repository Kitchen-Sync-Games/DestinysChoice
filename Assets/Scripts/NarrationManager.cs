using UnityEngine;
using TMPro;
using System.Collections;

public class NarrationManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;               
    [SerializeField] private RectTransform uiPanel;           
    [SerializeField] private TextMeshProUGUI narrationText;  
    [SerializeField] private GameObject choices; 
    [SerializeField] private TMP_Text choice1text; 
    [SerializeField] private TMP_Text choice2text; 

    
    private Coroutine hideCoroutine;
    private float displayDuration = 5f;
    private bool isChoosing;
    public int playerchoice;

    public void ShowNarration(string text, Vector3 worldPosition, float time)
    {
        displayDuration = time;
        narrationText.text = text;
        
        //ConvertToScreenSpace(worldPosition);
        AdjustPanelSize();
        
        uiPanel.gameObject.SetActive(true);
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }
        hideCoroutine = StartCoroutine(HideNarrationAfterTime(displayDuration));
    }
    public void ShowChoices(string choice1, string choice2)
    {
        playerchoice = 0;
        isChoosing = true;
        choice1text.text = choice1;
        choice2text.text = choice2;
        choices.SetActive(true);
    }
    public void PlayerChoosed(int c)
    {
        playerchoice = c;
        isChoosing = false;
        choices.SetActive(false);
    }

    private void ConvertToScreenSpace(Vector3 position)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(position);
        uiPanel.position = screenPosition; 
    }

    private void AdjustPanelSize()
    {
        Vector2 textSize = narrationText.GetPreferredValues();
        textSize = new Vector2(Mathf.Clamp(textSize.x, 20, 800), Mathf.Clamp(textSize.y, 20, 250));
        narrationText.rectTransform.sizeDelta = textSize; 
        narrationText.ForceMeshUpdate();
        Vector2 padding = new Vector2(40, 20);
        uiPanel.sizeDelta = textSize + padding;
    }

    private IEnumerator HideNarrationAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        uiPanel.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isChoosing)
        {
            if (Input.GetKey(KeyCode.A))
            {
                PlayerChoosed(1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                PlayerChoosed(2);
            }
        }
    }
}
