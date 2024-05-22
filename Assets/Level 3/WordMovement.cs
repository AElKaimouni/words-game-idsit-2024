using UnityEngine;
using UnityEngine.EventSystems;

public class WordMovement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 initialPosition;
    private bool isPlacedCorrectly = false;
    private string correctWord;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isPlacedCorrectly)
            return;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlacedCorrectly)
            return;

        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlacedCorrectly)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        // Check if the word is placed in a correct position
        if (true)
        {
            isPlacedCorrectly = true;
            score.Instance.WordPlacedCorrectly(correctWord);
        }
        else
        {
            rectTransform.anchoredPosition = initialPosition;
            isPlacedCorrectly = false;
        }
    }

    public void SetCorrectWord(string word)
    {
        correctWord = word;
    }
}
