using MoreMountains.Feedbacks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Movement")] 
    public float speed = 11;
    public int totalPathIfNotFinish = 1;
    public float moveCenter = 8;
    public float halfMovement = 4;
    [Header("Level Settings")] 
    public bool isEditorGoal;
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
    public int obsSpawnRate;
    public bool isOnlyOneObs;
    public int spawnObsNumber;
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
    public RuntimeAnimatorController houseAnimatorController;
    
    private bool _isSound;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        _isSound = PlayerDatabase.Instance.GetSound();
    }

    #region Settings

    public void CoinCollectSound()
    {
        Vibrate();
        if (!_isSound) return;
        FeedbackManager.Instance.CoinCollectSound.PlayFeedbacks();
    }
    public void SplashSound()
    {
        Vibrate();
        if (!_isSound) return;
        FeedbackManager.Instance.SplashSound.PlayFeedbacks();
    }
    public void LevelUpSound()
    {
        VibrateHigher();
        if (!_isSound) return;
        FeedbackManager.Instance.LevelUpSound.PlayFeedbacks();
    }
    public void HouseSounds(int i)
    {
        if (!_isSound) return;
        switch (i)
        {
            case 0:
                FeedbackManager.Instance.Charge.PlayFeedbacks();
                break;
            case 1:
                FeedbackManager.Instance.Jump.PlayFeedbacks();
                break;
            case 2:
                FeedbackManager.Instance.Land.PlayFeedbacks();
                break;
        }
    }
    public void DieFeedback()
    {
        VibrateHigher();
        FeedbackManager.Instance.DieFeedback.PlayFeedbacks();
    }
    public void ScalePlayer(bool isActive)
    {
        if (isActive)
        {
            FeedbackManager.Instance.ScalePlayer.GetComponent<MMFeedbackPosition>().InitialPosition =
                PlayerController.Instance.transform.localPosition;
            FeedbackManager.Instance.ScalePlayer.PlayFeedbacks();
        }
        else
        {
            FeedbackManager.Instance.ScalePlayer.StopFeedbacks();
        }
    }
    public void BadSound()
    {
        Vibrate();
        if (!_isSound) return;
        FeedbackManager.Instance.BadSound.StopFeedbacks();
        FeedbackManager.Instance.BadSound.PlayFeedbacks();
    }

    public void LoseFeedback(bool isActive)
    {
        if (isActive)
            FeedbackManager.Instance.LoseFeedback.PlayFeedbacks();
        else
        {
            FeedbackManager.Instance.LoseFeedback.StopFeedbacks();
        }
    }
    public void LoseFeedbackSound(bool isActive)
    {
        if(!_isSound) return;
        if (isActive)
            FeedbackManager.Instance.LoseFeedbackSound.PlayFeedbacks();
        else
        {
            FeedbackManager.Instance.LoseFeedbackSound.StopFeedbacks();
        }
    }
    
    public void CompleteSound()
    {
        VibrateHigher();
        if(!_isSound) return;
        FeedbackManager.Instance.CompleteSound.PlayFeedbacks();
    }
    public void CashOut()
    {
        if(!_isSound) return;
        FeedbackManager.Instance.CashOut.PlayFeedbacks();
    }
    public void PlayPopUp(bool isActive)
    {
        Vibrate();
        if (isActive)
        {
            if (!_isSound) return;
            FeedbackManager.Instance.PopUpOpen.PlayFeedbacks();
        }
        else
        {
            if (!_isSound) return;
            FeedbackManager.Instance.PopUpClose.PlayFeedbacks();
        }
    }

    public void Vibrate()
    {
        if (PlayerDatabase.Instance.CanVibrate())
        {
            FeedbackManager.Instance.Vibrate.PlayFeedbacks();
        }
    }
    
    public void VibrateHigher()
    {
        if (PlayerDatabase.Instance.CanVibrate())
            FeedbackManager.Instance.VibrateHigher.PlayFeedbacks();
    }

    #endregion
}
