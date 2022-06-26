using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Beginner")]
    public GameObject joystick;
    public GameObject beginnerPanel;
    [Header("Game Panel")] 
    public TextMeshProUGUI goalText;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void StartGame()
    {
        LevelManager.Instance.BeginGame();
        
        beginnerPanel.SetActive(false);
        joystick.SetActive(true);
    }

    public void SetGoalText(int goal)
    {
        goalText.gameObject.SetActive(true);
        goalText.text = "Reach " + goal;
    }
}
