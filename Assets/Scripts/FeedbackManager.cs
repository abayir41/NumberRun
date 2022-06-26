using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private MMFeedbacks vibrate;
    [SerializeField] private MMFeedbacks vibrateHigher;
    [SerializeField] private MMFeedbacks cashOut;
    [SerializeField] private MMFeedbacks coinCollect;
    [SerializeField] private MMFeedbacks coinCollectSound;
    [SerializeField] private MMFeedbacks popUpOpen;
    [SerializeField] private MMFeedbacks popUpClose;
    [SerializeField] private MMFeedbacks completeSound;
    [SerializeField] private MMFeedbacks scalePlayer;
    [SerializeField] private MMFeedbacks loseFeedback;
    [SerializeField] private MMFeedbacks loseFeedbackSound;
    [SerializeField] private MMFeedbacks badSound;
    [SerializeField] private MMFeedbacks splashSound;
    [SerializeField] private MMFeedbacks levelUpSound;
    [SerializeField] private MMFeedbacks dieFeedback;
    [SerializeField] private MMFeedbacks charge;
    [SerializeField] private MMFeedbacks jump;
    [SerializeField] private MMFeedbacks land;

    public MMFeedbacks Vibrate => vibrate;
    public MMFeedbacks VibrateHigher => vibrateHigher;
    public MMFeedbacks CashOut => cashOut;
    public MMFeedbacks CoinCollect => coinCollect;
    public MMFeedbacks CoinCollectSound => coinCollectSound;
    public MMFeedbacks PopUpOpen => popUpOpen;
    public MMFeedbacks PopUpClose => popUpClose;
    public MMFeedbacks CompleteSound => completeSound;
    public MMFeedbacks ScalePlayer => scalePlayer;
    public MMFeedbacks LoseFeedback => loseFeedback;
    public MMFeedbacks LoseFeedbackSound => loseFeedbackSound;
    public MMFeedbacks BadSound => badSound;
    public MMFeedbacks SplashSound => splashSound;
    public MMFeedbacks LevelUpSound => levelUpSound;
    public MMFeedbacks DieFeedback => dieFeedback;
    public MMFeedbacks Charge => charge;
    public MMFeedbacks Jump => jump;
    public MMFeedbacks Land => land;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
}