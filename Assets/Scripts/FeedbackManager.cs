using System;
using System.Collections;
using System.Collections.Generic;
using GlobalTypes;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private MMFeedbacks _Vibrate;
    [SerializeField] private MMFeedbacks _VibrateHigher;
    [SerializeField] private MMFeedbacks _CashOut;
    [SerializeField] private MMFeedbacks _CoinCollect;
    [SerializeField] private MMFeedbacks _CoinCollectSound;
    [SerializeField] private MMFeedbacks _PopUpOpen;
    [SerializeField] private MMFeedbacks _PopUpClose;
    [SerializeField] private MMFeedbacks _CompleteSound;
    [SerializeField] private MMFeedbacks _ScalePlayer;
    [SerializeField] private MMFeedbacks _LoseFeedback;
    [SerializeField] private MMFeedbacks _LoseFeedbackSound;
    [SerializeField] private MMFeedbacks _BadSound;
    [SerializeField] private MMFeedbacks _SplashSound;
    [SerializeField] private MMFeedbacks _LevelUpSound;
    [SerializeField] private MMFeedbacks _DieFeedback;
    [SerializeField] private MMFeedbacks _Charge;
    [SerializeField] private MMFeedbacks _Jump;
    [SerializeField] private MMFeedbacks _Land;

    public MMFeedbacks Vibrate => _Vibrate;
    public MMFeedbacks VibrateHigher => _VibrateHigher;
    public MMFeedbacks CashOut => _CashOut;
    public MMFeedbacks CoinCollect => _CoinCollect;
    public MMFeedbacks CoinCollectSound => _CoinCollectSound;
    public MMFeedbacks PopUpOpen => _PopUpOpen;
    public MMFeedbacks PopUpClose => _PopUpClose;
    public MMFeedbacks CompleteSound => _CompleteSound;
    public MMFeedbacks ScalePlayer => _ScalePlayer;
    public MMFeedbacks LoseFeedback => _LoseFeedback;
    public MMFeedbacks LoseFeedbackSound => _LoseFeedbackSound;
    public MMFeedbacks BadSound => _BadSound;
    public MMFeedbacks SplashSound => _SplashSound;
    public MMFeedbacks LevelUpSound => _LevelUpSound;
    public MMFeedbacks DieFeedback => _DieFeedback;
    public MMFeedbacks Charge => _Charge;
    public MMFeedbacks Jump => _Jump;
    public MMFeedbacks Land => _Land;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public static void PlayFeedback(MMFeedbacks feedback, FeedBackType feedBackType)
    {
        switch (feedBackType)
        {
            case FeedBackType.Sound:
                if(PlayerDatabase.CanSound())
                    feedback.PlayFeedbacks();
                break;
            
            case FeedBackType.Vibration:
                if(PlayerDatabase.CanVibrate())
                    feedback.PlayFeedbacks();
                break;
            
            case FeedBackType.Normal:
                feedback.PlayFeedbacks();    
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(feedBackType), feedBackType, null);
        }
    }

    public static void StopFeedback(MMFeedbacks feedbacks)
    {
        feedbacks.StopFeedbacks();
    }
    
}