using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
   
    public VisualElement ui;
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    void OnEnable()
    {
        Debug.Log("Enable main menu");
        var root = GetComponent<UIDocument>().rootVisualElement;
        var mainMenu = root.Q<VisualElement>("Panel");
        Debug.Log("main menu");
        var startButton = root.Q<Button>("StartButton");
        startButton.clicked += OnStartButtonClicked;
    }

//IEnumerator Start()
//{
//    yield return null; // wait 1 frame

//    ui = GetComponent<UIDocument>().rootVisualElement;
//    startButton = ui.Q<Button>("StartButton");

//    startButton.clicked += OnStartButtonClicked;
//    Debug.Log("ready");

//}





//void OnEnable()
//{
//    var doc = GetComponent<UIDocument>();
//    ui = doc.rootVisualElement;
//    Debug.Log("enterrrr main menu here");
//    ui.RegisterCallback<GeometryChangedEvent>(OnUIReady);
//}

//void OnUIReady(GeometryChangedEvent evt)
//{
//    Debug.Log("ready");
//    ui.UnregisterCallback<GeometryChangedEvent>(OnUIReady);

//    startButton = ui.Q<Button>("StartButton");

//    if (startButton == null)
//    {
//        Debug.LogError("StartButton NOT FOUND");
//        return;
//    }

//    startButton.clicked += OnStartButtonClicked;

//    Debug.Log("UI fully ready, button wired");
//}








//void OnEnable()
//{
//    Debug.Log("hellooooooooo pressed");
//    ui = GetComponent<UIDocument>().rootVisualElement;
//    var doc = GetComponent<UIDocument>();
//    Debug.Log("UIDocument enabled = " + doc.enabled);
//    Debug.Log("Panel Settings = " + doc.panelSettings);

//    Debug.Log("after root visual");
//    startButton = ui.Q<Button>("StartButton");
//    Debug.Log("start button assigned");
//    if (startButton == null)
//    {
//        Debug.LogError("StartButton NOT FOUND in UXML");
//        return;
//    }
//    Debug.Log("StartButton found");

//    startButton.RegisterCallback<PointerDownEvent>(evt =>
//    {
//        Debug.Log("PointerDown reached StartButton");
//    });

//    //settingsButton = ui.Q<Button>("SettingsButton");
//    //quitButton = ui.Q<Button>("QuitButton");

//    startButton.clicked += OnStartButtonClicked;
//    Debug.Log("llllllllll");
//    //startButton.clicked += () =>
//    //{
//    //    Debug.Log("Start pressed");
//    //    UIManager.Instance.ShowHUD();
//    //};
//    //settingsButton.clicked += OnSettingsButtonClicked;
//    //quitButton.clicked += OnQuitButtonClicked;
//    //quitButton.clicked += () =>
//    //{
//    //    Debug.Log("Quit pressed");
//    //};
//}




//private void Start()
//{
//    Debug.Log("start zeft");
//    startButton.clicked += OnStartButtonClicked;
//    Debug.Log("end zeft");
//}



//    private void OnQuitButtonClicked()
//    {
//        Debug.Log("Quit clicked!");
//        Application.Quit();
//    }
//    private void OnSettingsButtonClicked()
//    {

//    }
private void OnStartButtonClicked()
    {
        Debug.Log("Start pressed");
        if (UIManager.Instance == null) Debug.Log("null zeft");
        else { Debug.Log("not null zeft");
            UIManager.Instance.ShowHUD();
        }
        //{
        //    UIManager.Instance.ShowHUD();
        //}
        //else
        //{
        //    Debug.Log("UIManager.Instance is null! Make sure UIManager exists in the scene.");
        //}

        //        Debug.Log("start clicked!");
        //        //gameObject.SetActive(false);
        //        SceneManager.LoadScene("UI_Test_Scene");
        //        Debug.Log("start clickedddd!");
    }

}
