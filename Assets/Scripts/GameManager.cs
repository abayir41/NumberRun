using System.Collections;
using System.Collections.Generic;
using GlobalTypes;
using MoreMountains.Feedbacks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameConfig Config { get; private set; }
    [SerializeField] private GameConfig config;
    
    [Header("Movement")] 
    public float speed = 11;
    public int totalPathIfNotFinish = 1;
    public float moveCenter = 8;
    public float halfMovement = 4;
    [Header("Level Settings")] 
    public bool IsEditorGoal = false;
    public int editorLevelGoal = 10;
    [Header("Player")]
    public float colorDuration = 2f;
    public Color[] hitColors;
    [Header("Portal")] 
    public int[] addableExp = { 5, 30 };
    public int badPortalRate = 30;
    public int spawnPortalDistanceVertical = 15;
    public int onePortalSpawnRate = 30;
    public int portalSpawnRate = 90;
    public int distanceBetweenMobAndPortal = 5;
    [Header("Obs")] 
    public int obsSpawnRate = 0;
    public bool isOnlyOneObs = false;
    public int spawnObsNumber = 0;
    [Header("Spawn")] 
    public float numberBetweenSpace = -0.4f;
    public float spawnMobDistanceVertical = 15f;
    public float[] spawnMobDistanceHorizontal = { -1.5f, 0, 1.5f };
    public int spawnRate = 25;
    public int[] typeChances = { 30, 30, 15, 15 };
    public int[] spawnNumberLimitIncrease = { 1, 9 };
    public int[] spawnNumberLimitDecrease = { 1, 9 };
    public int[] spawnNumberLimitMultiply = { 1, 3 };
    public int[] spawnNumberLimitDivide = { 1, 3 };
    [Header("Final")]
    public RuntimeAnimatorController _houseAnimatorController;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Config = config;
    }
    private void Start()
    {
        
    }

    #region Settings

    public void CoinCollectSound()
    {
        Vibrate();
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.CoinCollectSound, FeedBackType.Sound);
    }
    public void SplashSound()
    {
        Vibrate();
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.SplashSound, FeedBackType.Sound);
    }
    public void LevelUpSound()
    {
        VibrateHigher();
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.LevelUpSound, FeedBackType.Sound);
    }
    public void HouseSounds(int i)
    {
        switch (i)
        {
            case 0:
                FeedbackManager.PlayFeedback(FeedbackManager.Instance.Charge, FeedBackType.Sound);
                break;
            case 1:
                FeedbackManager.PlayFeedback(FeedbackManager.Instance.Jump, FeedBackType.Sound);
                break;
            case 2:
                FeedbackManager.PlayFeedback(FeedbackManager.Instance.Land, FeedBackType.Sound);
                break;
        }
    }
    public void DieFeedback()
    {
        VibrateHigher();
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.DieFeedback, FeedBackType.PlayAnyway);
    }
    public void ScalePlayer(bool isActive)
    {
        if (isActive)
        {
            FeedbackManager.Instance.ScalePlayer.GetComponent<MMFeedbackPosition>().InitialPosition =
                PlayerController.Instance.transform.localPosition;
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.ScalePlayer, FeedBackType.PlayAnyway);
        }
        else
        {
            FeedbackManager.StopFeedback(FeedbackManager.Instance.ScalePlayer);
        }
    }
    public void BadSound()
    {
        Vibrate();
        FeedbackManager.StopFeedback(FeedbackManager.Instance.BadSound);
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.BadSound, FeedBackType.Sound);
    }

    public void LoseFeedback(bool isActive)
    {
        if (isActive)
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.LoseFeedback, FeedBackType.PlayAnyway);
        else
        {
            FeedbackManager.StopFeedback(FeedbackManager.Instance.LoseFeedback);
        }
    }
    public void LoseFeedbackSound(bool isActive)
    {
        if (isActive)
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.LoseFeedbackSound, FeedBackType.Sound);
        else
        {
            FeedbackManager.StopFeedback(FeedbackManager.Instance.LoseFeedbackSound);
        }
    }
    
    public void CompleteSound()
    {
        VibrateHigher();
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.CompleteSound,FeedBackType.Sound);
    }
    public void CashOut()
    {
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.CashOut,FeedBackType.Sound);
    }
    public void PlayPopUp(bool isActive)
    {
        Vibrate();
        if (isActive)
        {
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.PopUpOpen,FeedBackType.Sound);
        }
        else
        {
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.PopUpClose,FeedBackType.Sound);
        }
    }

    public void Vibrate()
    {
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.Vibrate,FeedBackType.Vibration);
    }
    
    public void VibrateHigher()
    {
        FeedbackManager.PlayFeedback(FeedbackManager.Instance.VibrateHigher,FeedBackType.Vibration);
    }

    #endregion
}
