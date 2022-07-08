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
    [SerializeField] private Slider iqSlide;
    [SerializeField] private UIElement iqCurrentGameObjectKnob;
    [SerializeField] private UIElement iqLeftTargetGameObject;
    [SerializeField] private UIElement iqRightTargetGameObject;
    private Image _iqCurrentImageKnob;
    private Image _iqLeftTargetImage;
    private Image _iqRightTargetImage;
    private Sequence _currentSequence;

    [Header("Start Menu")] 
    [SerializeField] private UIElementGroup startMenuElements;
    [SerializeField] private UIElement tapToStart;

    [Header("Game Menu")] 
    [SerializeField] private UIElementGroup gameMenuElements;

    [Header("Settings Menu")] 
    [SerializeField] private UIElement settingsPanel;
    private bool _isSettingOpen = false;

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

        _iqCurrentImageKnob = iqCurrentGameObjectKnob.GetComponent<Image>();
        _iqLeftTargetImage = iqLeftTargetGameObject.GetComponent<Image>();
        _iqRightTargetImage = iqRightTargetGameObject.GetComponent<Image>();
    }

    private void Start()
    {
        //Slider configurations
        _iqRightTargetImage.sprite = IQStageManager.Instance.GetCurrentStage().StageSprite;
        iqRightTargetGameObject.Show();
        iqLeftTargetGameObject.Hide();
        iqCurrentGameObjectKnob.Hide();

        //Scene panels configuration
        gameMenuElements.HideInstantly();
        settingsPanel.Hide();
        settingsPanel.CompleteCurrentTheAnimation();

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
        startMenuElements.HideTheElements(() =>
        {
            gameMenuElements.ShowTheElements(() =>
            {
                LevelManager.Instance.BeginGame();
                joystick.SetActive(true);
            });
        });
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
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.maxValue, animTime).OnComplete(() =>
        {
            _iqCurrentImageKnob.sprite = _iqRightTargetImage.sprite;
            _iqRightTargetImage.sprite = stage.StageSprite;
            _iqLeftTargetImage.sprite = stage.StageSprite;
            iqCurrentGameObjectKnob.Show();
            iqSlide.value = iqSlide.minValue;
        });
        _currentSequence.Append(tween);
    }
    
    private void StageDowned(Stage stage)
    {
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.minValue, animTime).OnComplete(() =>
        {
            _iqLeftTargetImage.sprite = _iqCurrentImageKnob.sprite;
            _iqRightTargetImage.sprite = _iqCurrentImageKnob.sprite;
            
            var previousStage = IQStageManager.Instance.GetPreviousStage(stage);
            if (previousStage == null)
                iqCurrentGameObjectKnob.Hide();
            else
                _iqCurrentImageKnob.sprite = previousStage.StageSprite;
            
            iqSlide.value = iqSlide.maxValue;
        });
        _currentSequence.Append(tween);

    }
    
    private void SliderLastRatioChanged(float lastRatio)
    {
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, lastRatio, animTime);
        _currentSequence.Append(tween);
    }
    
    private void StagesPassedOverTheTarget()
    {
        var tween = DOTween.To(() => iqSlide.value, newValue => iqSlide.value = newValue, iqSlide.maxValue, animTime).OnComplete(() =>
        {
            if (iqSlide.direction == Slider.Direction.LeftToRight)
            {
                iqRightTargetGameObject.Hide();
                iqLeftTargetGameObject.Show();
                iqSlide.direction = Slider.Direction.RightToLeft;
            }
            else
            {
                iqRightTargetGameObject.Show();
                iqLeftTargetGameObject.Hide();
                iqSlide.direction = Slider.Direction.LeftToRight;
            }
        });
        _currentSequence.Append(tween);

    }

    #endregion
    
}
