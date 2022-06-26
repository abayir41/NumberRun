using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Beginner")]
    public GameObject joystick;
    public GameObject BeginnerPanel;
    [Header("Game Panel")] 
    public TextMeshProUGUI GoalText;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void StartGame()
    {
        LevelManager.Instance.BeginGame();
        
        BeginnerPanel.SetActive(false);
        joystick.SetActive(true);
    }

    public void SetGoalText(int _goal)
    {
        GoalText.gameObject.SetActive(true);
        GoalText.text = "Reach " + _goal;
    }
}
