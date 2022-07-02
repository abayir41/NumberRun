using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GlobalTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Beginner")]
    public GameObject joystick;
    public GameObject BeginnerPanel;
    [Header("Game Panel")] 
    public TextMeshProUGUI GoalText;

    [Header("IQ Slider")] 
    [SerializeField] private float animTime;
    [SerializeField] private Slider iqSlide;
    [SerializeField] private GameObject iqCurrentGameObjectKnob;
    [SerializeField] private GameObject iqLeftTargetGameObject;
    [SerializeField] private GameObject iqRightTargetGameObject;
    private Image _iqCurrentImageKnob;
    private static Image _iqLeftTargetImage;
    private Image _iqRightTargetImage;
    private Sequence _currentSequence;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _iqCurrentImageKnob = iqCurrentGameObjectKnob.GetComponent<Image>();
        _iqLeftTargetImage = iqLeftTargetGameObject.GetComponent<Image>();
        _iqRightTargetImage = iqRightTargetGameObject.GetComponent<Image>();
    }

    private void Start()
    {
        _iqRightTargetImage.sprite = IQStageManager.Instance.GetCurrentStage().StageSprite;
        iqRightTargetGameObject.SetActive(true);
        iqLeftTargetGameObject.SetActive(false);
        iqCurrentGameObjectKnob.SetActive(false);
    }

    private void OnEnable()
    {
        ProjectEvents.StageUpped += StageUpped;
        ProjectEvents.StageDowned += StageDowned;
        ProjectEvents.LastRatioChanged += SliderLastRatioChanged;
        ProjectEvents.StagesPassedOverTheTarget += StagesPassedOverTheTarget;
    }

    

    private void OnDisable()
    {
        ProjectEvents.StageUpped -= StageUpped;
        ProjectEvents.StageDowned -= StageDowned;
        ProjectEvents.LastRatioChanged -= SliderLastRatioChanged;
        ProjectEvents.StagesPassedOverTheTarget -= StagesPassedOverTheTarget;


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

    public void PrepareSequenceForSlide()
    {
        if (_currentSequence.IsActive())
        {
            var sliderAnimationSequence = DOTween.Sequence().Pause();
            _currentSequence.OnKill(() =>
            {
                sliderAnimationSequence.Play();
            });
            _currentSequence = sliderAnimationSequence;
        }
        else
        {
            var sliderAnimationSequence = DOTween.Sequence();
            _currentSequence = sliderAnimationSequence;
        }
    }
    
    private void StageUpped(Stage stage)
    {
        Debug.Log("StageUpped");
        
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.maxValue, animTime).OnComplete(() =>
        {
            _iqCurrentImageKnob.sprite = _iqRightTargetImage.sprite;
            _iqRightTargetImage.sprite = stage.StageSprite;
            _iqLeftTargetImage.sprite = stage.StageSprite;
            iqCurrentGameObjectKnob.SetActive(true);
            iqSlide.value = iqSlide.minValue;
        });
        _currentSequence.Append(tween);
    }
    
    private void StageDowned(Stage stage)
    {
        Debug.Log("StageDowned");
        
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.minValue, animTime).OnComplete(() =>
        {
            _iqLeftTargetImage.sprite = _iqCurrentImageKnob.sprite;
            _iqRightTargetImage.sprite = _iqCurrentImageKnob.sprite;
            
            var previousStage = IQStageManager.Instance.GetPreviousStage(stage);
            if (previousStage == null)
                iqCurrentGameObjectKnob.SetActive(false);
            else
                _iqCurrentImageKnob.sprite = previousStage.StageSprite;
            
            iqSlide.value = iqSlide.maxValue;
        });
        _currentSequence.Append(tween);

    }
    
    private void SliderLastRatioChanged(float lastRatio)
    {
        Debug.Log("SliderRatio: "+lastRatio);
        
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, lastRatio, animTime);
        _currentSequence.Append(tween);
    }
    
    private void StagesPassedOverTheTarget()
    {
        
        Debug.Log("wrd");
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.maxValue, animTime).OnComplete(() =>
        {
            if (iqSlide.direction == Slider.Direction.LeftToRight)
            {
                iqRightTargetGameObject.SetActive(false);
                iqLeftTargetGameObject.SetActive(true);
                iqSlide.direction = Slider.Direction.RightToLeft;
            }
            else
            {
                iqRightTargetGameObject.SetActive(true);
                iqLeftTargetGameObject.SetActive(false);
                iqSlide.direction = Slider.Direction.LeftToRight;
            }
        });
        _currentSequence.Append(tween);

    }
}
