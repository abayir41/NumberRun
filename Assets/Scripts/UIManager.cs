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
    [SerializeField] private UIElement goalTextUIElement;
    
    [Header("IQ Slider")] 
    [SerializeField] private float animTime;
    [SerializeField] private List<Slider> iqSlide;
    [SerializeField] private List<UIElement> iqCurrentGameObjectKnob;
    [SerializeField] private List<UIElement> iqLeftTargetGameObject;
    [SerializeField] private List<UIElement> iqRightTargetGameObject;
    private List<Image> _iqCurrentImageKnob;
    private List<Image> _iqLeftTargetImage;
    private List<Image> _iqRightTargetImage;
    private Sequence _currentSequence;
    private float _iqSlideValue;

    [Header("Start Menu")] 
    [SerializeField] private GameObject startMenu;
    [SerializeField] private UIElementGroup startMenuElements;
    [SerializeField] private UIElement tapToStart;

    [Header("Game Menu")] 
    [SerializeField] private UIElementGroup gameMenuElements;

    [Header("Settings Menu")] 
    [SerializeField] private UIElement settingsPanel;
    private bool _isSettingOpen = false;

    [Header("End Menu")] 
    [SerializeField] private GameObject endUI;
    [SerializeField] private UIElement background;
    [SerializeField] private UIElement successBackground;
    [SerializeField] private UIElement successText;
    [SerializeField] private UIElement timeText;
    [SerializeField] private UIElement timeAsNumber;
    [SerializeField] private UIElement emojiShine;
    [SerializeField] private UIElement emoji;
    [SerializeField] private UIElement tapToContinue;
    [SerializeField] private UIElement tapToContinueButton;
    [SerializeField] private UIElement iqSlideEndGame;

    [Header("Sound and Vibration")] 
    [SerializeField] private Image soundImage;
    [SerializeField] private Image vibrationImage;
    [SerializeField] private Sprite soundEnableSprite;
    [SerializeField] private Sprite soundDisableSprite;
    [SerializeField] private Sprite vibrationEnableSprite;
    [SerializeField] private Sprite vibrationDisableSprite;
    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _iqCurrentImageKnob = iqCurrentGameObjectKnob.ConvertAll(input => input.GetComponent<Image>());
        _iqLeftTargetImage = iqLeftTargetGameObject.ConvertAll(input => input.GetComponent<Image>());
        _iqRightTargetImage = iqRightTargetGameObject.ConvertAll(input => input.GetComponent<Image>());
    }

    private void Start()
    {
        //Slider configurations
        foreach (var image in _iqRightTargetImage)
            image.sprite = IQStageManager.Instance.GetCurrentStage().StageSprite;
        
        iqRightTargetGameObject.ForEach(element => element.Show());
        iqLeftTargetGameObject.ForEach(element => element.Hide());
        iqCurrentGameObjectKnob.ForEach(element => element.Hide());

        //Scene panels configuration
        gameMenuElements.HideInstantly();
        settingsPanel.Hide();
        settingsPanel.CompleteCurrentTheAnimation();
        tapToStart.StartFadeLoop();
        
        //EndUI
        background.Hide();
        successBackground.Hide();
        successText.Hide();
        timeText.Hide();
        timeAsNumber.Hide();
        emojiShine.Hide();
        emoji.Hide();
        tapToContinue.Hide();
        tapToContinueButton.Hide();
        iqSlideEndGame.Hide();
        
        background.CompleteCurrentTheAnimation();
        successBackground.CompleteCurrentTheAnimation();
        successText.CompleteCurrentTheAnimation();
        timeText.CompleteCurrentTheAnimation();
        timeAsNumber.CompleteCurrentTheAnimation();
        emojiShine.CompleteCurrentTheAnimation();
        emoji.CompleteCurrentTheAnimation();
        tapToContinue.CompleteCurrentTheAnimation();
        tapToContinueButton.CompleteCurrentTheAnimation();
        iqSlideEndGame.CompleteCurrentTheAnimation();
        
        //Prevent UI collision
        endUI.SetActive(false);
        
        
        //Set Sound and vibration Images
        if (PlayerDatabase.CanSound())
        {
            soundImage.sprite = soundEnableSprite;
        }
        else
        {
            soundImage.sprite = soundDisableSprite;
        }

        if (PlayerDatabase.CanVibrate())
        {
            vibrationImage.sprite = vibrationEnableSprite;
        }
        else
        {
            vibrationImage.sprite = vibrationDisableSprite;
        }

    }

    private void OnEnable()
    {
        ProjectEvents.StageUpped += StageUpped;
        ProjectEvents.StageDowned += StageDowned;
        ProjectEvents.LastRatioChanged += SliderLastRatioChanged;
        ProjectEvents.StagesPassedOverTheTarget += StagesPassedOverTheTarget;
        ProjectEvents.GameWin += GameWin;
    }

  


    private void OnDisable()
    {
        ProjectEvents.StageUpped -= StageUpped;
        ProjectEvents.StageDowned -= StageDowned;
        ProjectEvents.LastRatioChanged -= SliderLastRatioChanged;
        ProjectEvents.StagesPassedOverTheTarget -= StagesPassedOverTheTarget;
        ProjectEvents.GameWin -= GameWin;
    }
    

    public void StartGame()
    {
        if (_isSettingOpen)
        {
            SettingToggle(() =>
            {
                startMenuElements.HideTheElements(() =>
                {
                    gameMenuElements.ShowTheElements(() =>
                    {
                        LevelManager.Instance.BeginGame();
                        joystick.SetActive(true);
                    });
                });
            });
        }
        else
        {
            startMenuElements.HideTheElements(() =>
            {
                gameMenuElements.ShowTheElements(() =>
                {
                    LevelManager.Instance.BeginGame();
                    joystick.SetActive(true);
                });
            });
        }
        
    }
    
    private void GameWin()
    {
        startMenu.SetActive(false);
        endUI.SetActive(true);
        gameMenuElements.HideTheElements(() =>
        {
            StartCoroutine(EndGameAnim());    
        });
    }

    private IEnumerator EndGameAnim()
    {
        background.Show();
        
        yield return new WaitForSeconds(0.10f);
        successBackground.Show();
        successText.Show();
        
        yield return new WaitForSeconds(0.10f);
        timeText.Show();
        yield return new WaitForSeconds(0.5f);
        timeAsNumber.Show();
        
        yield return new WaitForSeconds(0.5f);
        iqSlideEndGame.Show();

    }

    public void SetGoalText(int _goal)
    {
        GoalText.text = "Reach " + _goal;
    }

    public void SettingToggle()
    {
        if (!_isSettingOpen)
        {
            settingsPanel.Show();
            _isSettingOpen = !_isSettingOpen;
        }
        else
        {
            settingsPanel.Hide();
            _isSettingOpen = !_isSettingOpen;
        }
    }
    
    public void SettingToggle(Action callback)
    {
        if (!_isSettingOpen)
        {
            settingsPanel.Show(callback);
            _isSettingOpen = !_isSettingOpen;
        }
        else
        {
            settingsPanel.Hide(callback);
            _isSettingOpen = !_isSettingOpen;
        }
    }

    public void SoundToggle()
    {
        if (PlayerDatabase.CanSound())
        {
            PlayerDatabase.ChangeSound(false);
            soundImage.sprite = soundDisableSprite;
        }
        else
        {
            PlayerDatabase.ChangeSound(true);
            soundImage.sprite = soundEnableSprite;
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.CoinCollectSound, FeedBackType.PlayAnyway);
        }
    }

    public void VibrationToggle()
    {
        if (PlayerDatabase.CanVibrate())
        {
            PlayerDatabase.ChangeVibrate(false);
            vibrationImage.sprite = vibrationDisableSprite;
        }
        else
        {
            PlayerDatabase.ChangeVibrate(true);
            vibrationImage.sprite = vibrationEnableSprite;
            FeedbackManager.PlayFeedback(FeedbackManager.Instance.Vibrate, FeedBackType.PlayAnyway);
        }
    }
    
    #region Slide Process

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
        var tween = DOTween.To(() => _iqSlideValue,
                newValue => { _iqSlideValue = newValue; iqSlide.ForEach(slider => slider.value = newValue);},
                iqSlide[0].maxValue, animTime)
            .OnComplete(() =>
        {
            for (var i = 0; i < _iqCurrentImageKnob.Count; i++)
                _iqCurrentImageKnob[i].sprite = _iqRightTargetImage[i].sprite;
            
            _iqRightTargetImage.ForEach(image => image.sprite = stage.StageSprite);
            _iqLeftTargetImage.ForEach(image => image.sprite = stage.StageSprite);

            iqCurrentGameObjectKnob.ForEach(element => element.Show());

            iqSlide.ForEach(slider => slider.value = slider.minValue);
            
            ProjectEvents.StageUppedAnimCompleted?.Invoke();
        });
        _currentSequence.Append(tween);
    }
    
    private void StageDowned(Stage stage)
    {
        var tween = DOTween.To(() => _iqSlideValue,
                newValue =>
                {
                    _iqSlideValue = newValue;
                    iqSlide.ForEach(slider => slider.value = newValue);
                },
                iqSlide[0].minValue, animTime)
            .OnComplete(() =>
            {

                for (var i = 0; i < _iqLeftTargetImage.Count; i++)
                    _iqLeftTargetImage[i].sprite = _iqCurrentImageKnob[i].sprite;

                for (var i = 0; i < _iqRightTargetImage.Count; i++)
                    _iqRightTargetImage[i].sprite = _iqCurrentImageKnob[i].sprite;

                var previousStage = IQStageManager.Instance.GetPreviousStage(stage);
                if (previousStage == null)
                    iqCurrentGameObjectKnob.ForEach(element => element.Hide());
                else
                    _iqCurrentImageKnob.ForEach(image => image.sprite = stage.StageSprite);

                iqSlide.ForEach(slider => slider.value = slider.maxValue);

                ProjectEvents.StageDownAnimCompleted?.Invoke();
            });
        _currentSequence.Append(tween);

    }
    
    private void SliderLastRatioChanged(float lastRatio)
    {
        var tween = DOTween.To(() => _iqSlideValue,
            newValue =>
            {
                _iqSlideValue = newValue;
                iqSlide.ForEach(slider => slider.value = newValue);
            },
            lastRatio, animTime);
        _currentSequence.Append(tween);
    }
    
    private void StagesPassedOverTheTarget()
    {
        var tween = DOTween.To(() => _iqSlideValue, newValue =>
        {
            _iqSlideValue = newValue;
            iqSlide.ForEach(slider => slider.value = newValue);
        }, iqSlide[0].maxValue, animTime).OnComplete(() =>
        {
            if (iqSlide[0].direction == Slider.Direction.LeftToRight)
            {
                iqRightTargetGameObject.ForEach(element => element.Hide());
                iqLeftTargetGameObject.ForEach(element => element.Show());
                iqSlide.ForEach(slider => slider.direction = Slider.Direction.RightToLeft);
            }
            else
            {
                iqRightTargetGameObject.ForEach(element => element.Show());
                iqLeftTargetGameObject.ForEach(element => element.Hide());
                iqSlide.ForEach(slider => slider.direction = Slider.Direction.LeftToRight);
            }
            
            ProjectEvents.StagesPassedOverTheTargetAnimCompleted?.Invoke();
        });
        _currentSequence.Append(tween);

    }

    #endregion
    
}
