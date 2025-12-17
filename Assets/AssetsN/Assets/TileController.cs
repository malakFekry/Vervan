using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    
    public AssetsN.GameManager gameManager;
    private Image myImage;
    private Button myButton;

    public bool isTarget = false; 
    public bool wasClicked = false; 

    // الألوان
    public Color normalColor = new Color32(100, 60, 30, 255); // بني
    public Color activeColor = Color.cyan; // لون الباترن
    public Color correctColor = Color.green; // إجابة صح (أخضر علطول)
    public Color wrongColor = Color.red; // إجابة غلط (أحمر علطول)

    void Start()
    {
        myImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(OnTileClicked);
    }

    public void ResetTile()
    {
        isTarget = false;
        wasClicked = false;
        myImage.color = normalColor;
        myButton.interactable = false;
    }

    public void HighlightTile()
    {
        myImage.color = activeColor;
    }

    public void HideTile()
    {
        myImage.color = normalColor;
    }

    public void EnableInteraction(bool tileEnabled)
    {
        myButton.interactable = tileEnabled;
    }

    // دالة التلوين البسيطة (زي الكود القديم)
    public void SetColor(bool isCorrect)
    {
        if (isCorrect)
            myImage.color = correctColor;
        else
            myImage.color = wrongColor;
    }

    void OnTileClicked()
    {
        gameManager.CheckTile(this);
    }
}