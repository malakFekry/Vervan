using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    public VisualElement ui;
    private ProgressBar healthBar;
    private ProgressBar skillBar;
    private Button pauseButton;

    //IEnumerator Start()
    //{
    //    yield return null; // wait 1 frame

    //    ui = GetComponent<UIDocument>().rootVisualElement;
    //    Debug.Log("enter hud");
    //    healthBar = ui.Q<ProgressBar>("HealthBar");
    //    Debug.Log("enter hud");
    //    //skillBar = ui.Q<ProgressBar>("SkillBar");
    //    //Debug.Log("enter hud");
    //    ////pauseButton = ui.Q<Button>("Pause");
    //    ////Debug.Log("enter hud");

    //    ////// Example test values
    //    healthBar.value = 20;
    //    Debug.Log("update hud");
    //    //skillBar.value = 20;
    //}

    void OnEnable()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log("enter hud");
        healthBar = ui.Q<ProgressBar>("HealthBar");
        Debug.Log("enter hud");
        if (healthBar == null) Debug.Log("errorrr");
        skillBar = ui.Q<ProgressBar>("SkillBar");
        Debug.Log("enter hud");
        pauseButton = ui.Q<Button>("Pause");
        //Debug.Log("enter hud");

        ////// Example test values
        healthBar.value = 20;
        skillBar.value = 20;


        pauseButton.clicked += OnPauseClicked;
    }

    private void OnPauseClicked()
    {
        // Call UIManager to show Pause Menu
        Debug.Log("Pausseeee");
        //UIManager.Instance.ShowPauseMenu();
    }

    public void SetHealth(float health) => healthBar.value = health;
    //public void SetSkill(float skill) => skillBar.value = skill;
}
