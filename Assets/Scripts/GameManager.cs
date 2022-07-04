using System.Collections;
using System.Collections.Generic;
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
    
    private bool isSound = false;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Config = config;
    }
    private void Start()
    {
        isSound = PlayerDatabase.instance.GetSound();
    }

    #region Settings

    public void CoinCollectSound()
    {
        Vibrate();
        if (!isSound) return;
        FeedbackManager.instance.CoinCollectSound.PlayFeedbacks();
    }
    public void SplashSound()
    {
        Vibrate();
        if (!isSound) return;
        FeedbackManager.instance.SplashSound.PlayFeedbacks();
    }
    public void LevelUpSound()
    {
        VibrateHigher();
        if (!isSound) return;
        FeedbackManager.instance.LevelUpSound.PlayFeedbacks();
    }
    public void HouseSounds(int i)
    {
        if (!isSound) return;
        switch (i)
        {
            case 0:
                FeedbackManager.instance.Charge.PlayFeedbacks();
                break;
            case 1:
                FeedbackManager.instance.Jump.PlayFeedbacks();
                break;
            case 2:
                FeedbackManager.instance.Land.PlayFeedbacks();
                break;
        }
    }
    public void DieFeedback()
    {
        VibrateHigher();
        FeedbackManager.instance.DieFeedback.PlayFeedbacks();
    }
    public void ScalePlayer(bool isActive)
    {
        if (isActive)
        {
            FeedbackManager.instance.ScalePlayer.GetComponent<MMFeedbackPosition>().InitialPosition =
                PlayerController.Instance.transform.localPosition;
            FeedbackManager.instance.ScalePlayer.PlayFeedbacks();
        }
        else
        {
            FeedbackManager.instance.ScalePlayer.StopFeedbacks();
        }
    }
    public void BadSound()
    {
        Vibrate();
        if (!isSound) return;
        FeedbackManager.instance.BadSound.StopFeedbacks();
        FeedbackManager.instance.BadSound.PlayFeedbacks();
    }

    public void LoseFeedback(bool isActive)
    {
        if (isActive)
            FeedbackManager.instance.LoseFeedback.PlayFeedbacks();
        else
        {
            FeedbackManager.instance.LoseFeedback.StopFeedbacks();
        }
    }
    public void LoseFeedbackSound(bool isActive)
    {
        if(!isSound) return;
        if (isActive)
            FeedbackManager.instance.LoseFeedbackSound.PlayFeedbacks();
        else
        {
            FeedbackManager.instance.LoseFeedbackSound.StopFeedbacks();
        }
    }
    
    public void CompleteSound()
    {
        VibrateHigher();
        if(!isSound) return;
        FeedbackManager.instance.CompleteSound.PlayFeedbacks();
    }
    public void CashOut()
    {
        if(!isSound) return;
        FeedbackManager.instance.CashOut.PlayFeedbacks();
    }
    public void PlayPopUp(bool isActive)
    {
        Vibrate();
        if (isActive)
        {
            if (!isSound) return;
            FeedbackManager.instance.PopUpOpen.PlayFeedbacks();
        }
        else
        {
            if (!isSound) return;
            FeedbackManager.instance.PopUpClose.PlayFeedbacks();
        }
    }

    public void Vibrate()
    {
        if (PlayerDatabase.instance.CanVibrate())
        {
            FeedbackManager.instance.Vibrate.PlayFeedbacks();
        }
    }
    
    public void VibrateHigher()
    {
        if (PlayerDatabase.instance.CanVibrate())
            FeedbackManager.instance.VibrateHigher.PlayFeedbacks();
    }

    #endregion
}
