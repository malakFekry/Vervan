using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FloatingBubble : MonoBehaviour
{
    public TextMeshProUGUI label;
    public Button button;

    // Getter for the letter
    public string MyLetter { get; private set; }

    private GameManager gameManager;
    private RectTransform rectTransform;
    private bool isPopping = false;

    public void Setup(GameManager manager, string letter)
    {
        gameManager = manager;
        MyLetter = letter;

        if (label != null) label.text = MyLetter;
        rectTransform = GetComponent<RectTransform>();

        // Ensure scale is 1 when spawned
        transform.localScale = Vector3.one;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnTap);
    }

    // Call this to move the bubble smoothly
    public void MoveTo(Vector2 targetPosition, float duration)
    {
        if (isPopping) return; // Don't move if popping
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(targetPosition, duration));
    }

    IEnumerator MoveRoutine(Vector2 targetPos, float duration)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = targetPos;
    }

    void OnTap()
    {
        if (!isPopping) gameManager.CheckInput(MyLetter, this);
    }

    // --- NEW POP ANIMATION ---
    public void Pop()
    {
        isPopping = true;
        button.interactable = false; // Prevent double clicks
        StartCoroutine(AnimatePop());
    }

    IEnumerator AnimatePop()
    {
        float duration = 0.3f; // Animation speed
        float elapsed = 0f;
        Vector3 originalScale = transform.localScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Scale from 1 down to 0
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject); // Actually destroy it now
    }
}