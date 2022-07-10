using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GlobalTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIElement : MonoBehaviour
{
    public bool IsElementActive { get; private set; }
    
    [SerializeField] private UIAnimationType animationType;
    [SerializeField] private UIAnimationDirections directionOfObject;
    [SerializeField] private float amountOfMoveCoEfficient = 1;
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private bool autoDisableComponents = true;
    [SerializeField] private List<UIElement> children;
    
    private static GameConfig Config => GameManager.Config;
    private RectTransform _rectTransform;

    private Tweener _currentAnimation;

    private List<Behaviour> _enableDisableComponents;
    private Image _image;
    private TextMeshProUGUI _text;
    private Selectable _selectable;

    private List<Image> _childrenImages;
    private List<TextMeshProUGUI> _childrenTexts;
    private List<Selectable> _childrenSelectables;
    
    private void Awake()
    {
        _enableDisableComponents = new List<Behaviour>();
        
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _text = GetComponent<TextMeshProUGUI>();
        _selectable = GetComponent<Selectable>();
        
        var allComponents = GetComponents<Behaviour>();
        foreach (var component in allComponents)
        {
            if(component == this)
                continue;
            
            _enableDisableComponents.Add(component);
        }
    }

    public void Hide(Action callback = null)
    {
        if (_currentAnimation.IsActive())
        {
            Debug.LogWarning($"There was an animation playing but had to stop by system: {gameObject.name}");
            CompleteCurrentTheAnimation();
        }
        
        switch (animationType)
        {
            case UIAnimationType.BasicEnableDisable:
                DisableInteractiveness();
                DisableComponents();
                callback?.Invoke();
                break;
            
            case UIAnimationType.Move:
                Move(directionOfObject, callback);
                break;
            
            case UIAnimationType.FadeOut:
                FadeOut(callback);
                break;
            
            case UIAnimationType.Minimize:
                Minimize(callback);
                break;
            
            case UIAnimationType.Nothing:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        children.ForEach(element => element.Hide());
    }

    public void Show(Action callback = null)
    {
        if (_currentAnimation.IsActive())
        {
            Debug.LogWarning($"There was an animation playing but had to stop by system: {gameObject.name}");
            CompleteCurrentTheAnimation();
        }

        switch (animationType)
        {
            case UIAnimationType.BasicEnableDisable:
                EnableComponents();
                EnableInteractiveness();
                callback?.Invoke();
                break;
            
            case UIAnimationType.Move:
                MoveBackward(directionOfObject, callback);
                break;
            
            case UIAnimationType.FadeOut:
                FadeOutBackWard(callback);
                break;
            
            case UIAnimationType.Minimize:
                NormalSize(callback);
                break;
            
            case UIAnimationType.Nothing:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        children.ForEach(element => element.Show());
    }

    #region Enabling Disabling

    public void EnableInteractiveness()
    {
        if(_selectable != null)
            _selectable.interactable = true;
    }
    
    public void DisableInteractiveness()
    {
        if(_selectable != null)
            _selectable.interactable = false;
    }
    
    public void EnableComponents()
    {
        _enableDisableComponents?.ForEach(behaviour => behaviour.enabled = true);
        IsElementActive = true;
    }
    
    public void DisableComponents()
    {
        _enableDisableComponents?.ForEach(behaviour => behaviour.enabled = false);
        IsElementActive = false;
    }

    #endregion

    public float GetAnimationDurationTime()
    {
        switch (animationType)
        {
            case UIAnimationType.BasicEnableDisable:
                return 0;
            case UIAnimationType.Move:
                return Config.UIMoveAnimationDuration;
            case UIAnimationType.FadeOut:
                return Config.UIFadeOutAnimationDuration;
            case UIAnimationType.Minimize:
                return Config.UIMinimizeTime;
            case UIAnimationType.Nothing:
                return 0;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void CompleteCurrentTheAnimation()
    {
        if (_currentAnimation.IsActive())
        {
            _currentAnimation.Complete();
            _currentAnimation.Kill();
        }
        else
            Debug.LogWarning($"There is no animation playing: {gameObject.name}");    
    }

    #region Move Animation
    
    private float? _originalX;
    private float? _originalY;

    public Tweener Move(UIAnimationDirections direction, Action callback = null)
    {
        callback += () =>
        {
            if (autoDisableComponents && _selectable != null)
            {
                DisableInteractiveness();
                DisableComponents();
            }
        };
        
        
        var realPosition = _rectTransform.position;
        _originalX ??= realPosition.x;
        _originalY ??= realPosition.y;

        switch (direction)
        {
            case UIAnimationDirections.Left:
                 _currentAnimation = _rectTransform.DOMoveX(-_rectTransform.rect.width * amountOfMoveCoEfficient, Config.UIMoveAnimationDuration).SetEase(easeType)
                    .OnComplete(() => callback?.Invoke());
                
                 return _currentAnimation;
            
            case UIAnimationDirections.Right:
                _currentAnimation = _rectTransform
                    .DOMoveX(Screen.width + _rectTransform.rect.width * amountOfMoveCoEfficient, Config.UIMoveAnimationDuration).SetEase(easeType)
                    .OnComplete(() => callback?.Invoke());
                
                return _currentAnimation;
            
            case UIAnimationDirections.Up:
                _currentAnimation = _rectTransform
                    .DOMoveY(Screen.height + _rectTransform.rect.height * amountOfMoveCoEfficient, Config.UIMoveAnimationDuration).SetEase(easeType)
                    .OnComplete(() => callback?.Invoke());
                
                return _currentAnimation;
            
            case UIAnimationDirections.Down:
                _currentAnimation = _rectTransform.DOMoveY(-_rectTransform.rect.height * amountOfMoveCoEfficient, Config.UIMoveAnimationDuration).SetEase(easeType)
                    .OnComplete(() => callback?.Invoke());
                
                return _currentAnimation;
        }
        
        Debug.LogWarning($"Nothing was animated: {gameObject.name}");
        return null;
    }

    public Tweener MoveBackward(UIAnimationDirections direction, Action callback = null)
    {
        
        if (autoDisableComponents && _selectable != null)
        {
            EnableInteractiveness();
            EnableComponents();
        }
        
        if (_originalX is null|| _originalY is null)
        {
            throw new Exception($"Position seems not cached, You may not call the Move function first: {gameObject.name}");
        }

        switch (direction)
        {
            case UIAnimationDirections.Left:
                _currentAnimation = _rectTransform.DOMoveX(_originalX.Value, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
                return _currentAnimation;
            
            case UIAnimationDirections.Right:
                _currentAnimation = _rectTransform.DOMoveX(_originalX.Value, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
                return _currentAnimation;
            
            case UIAnimationDirections.Up:
                _currentAnimation = _rectTransform.DOMoveY(_originalY.Value, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
                return _currentAnimation;
            
            case UIAnimationDirections.Down:
                _currentAnimation = _rectTransform.DOMoveY(_originalY.Value, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
                return _currentAnimation;
        }

        Debug.LogWarning($"Nothing was animated: {gameObject.name}");
        return null;
    }
    
    #endregion
    
    #region FadeOut

    private float? _originalImgAlpha;
    private float? _originalTextAlpha;
    public Tweener FadeOut(Action callback = null)
    {
        callback += () =>
        {
            if (autoDisableComponents && _selectable != null)
            {
                DisableInteractiveness();
                DisableComponents();
            }
        };
        
        if (_image != null)
        {
            _originalImgAlpha ??= _image.color.a;
            
            _currentAnimation = _image.DOFade(0, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
            return _currentAnimation;
        }
         
        if (_text != null)
        {
            _originalTextAlpha ??= _text.color.a;
            
            _currentAnimation = _text.DOFade(0, Config.UIFadeOutAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
            return _currentAnimation;
        }

        Debug.LogWarning($"Calling this function is not suitable for this object: {gameObject.name}");
        return null;
    }

    public Tweener FadeOutBackWard(Action callback = null)
    {
        if (autoDisableComponents && _selectable != null)
        {
            EnableInteractiveness();
            EnableComponents();
        }
        
        if (_image != null)
        {
            if (_originalImgAlpha is null)
                throw new Exception($"Image Alpha seems not cached, You may not call the Fadeout function first: {gameObject.name}");
            
            _currentAnimation = _image.DOFade(_originalImgAlpha.Value, Config.UIMoveAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
            return _currentAnimation;
        }
         
        if (_text != null)
        {
            if (_originalTextAlpha is null)
                throw new Exception($"Text Alpha seems not cached, You may not call the Fadeout function first: {gameObject.name}");
            
            _currentAnimation = _text.DOFade(_originalTextAlpha.Value, Config.UIFadeOutAnimationDuration).SetEase(easeType).OnComplete(() => callback?.Invoke());
            return _currentAnimation;
        }

        Debug.LogWarning($"Calling this function is not suitable for this object: {gameObject.name}");
        return null;
    }

    #endregion

    #region Minimizing
    
    public Tweener Minimize(Action callback = null)
    {
        callback += () =>
        {
            if (autoDisableComponents && _selectable != null)
            {
                DisableInteractiveness();
                DisableComponents();
            }
        };
        
        _currentAnimation = _rectTransform.DOScale(Vector3.zero, Config.UIMinimizeTime).SetEase(easeType).OnComplete(() => callback?.Invoke());
        return _currentAnimation;
    }
    
    public Tweener NormalSize(Action callback = null)
    {
        if (autoDisableComponents && _selectable != null)
        {
            EnableInteractiveness();
            EnableComponents();
        }
        
        _currentAnimation = _rectTransform.DOScale(Vector3.one, Config.UIMinimizeTime).SetEase(easeType).OnComplete(() => callback?.Invoke());
        return _currentAnimation;
    }
    
    #endregion

    #region FadeLoop

    public Tweener StartFadeLoop()
    {
        if (_image != null)
        {
            _currentAnimation = _image.DOFade(0, Config.UIFadeLoopAnimationDuration).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
            return _currentAnimation;
        }
         
        if (_text != null)
        {
            _currentAnimation = _text.DOFade(0, Config.UIFadeLoopAnimationDuration).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
            return _currentAnimation;
        }
        
        Debug.LogWarning($"Calling this function is not suitable for this object: {gameObject.name}");
        return null;
    }

    #endregion
   


}
