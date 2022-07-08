using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIElementGroup : MonoBehaviour
{
    [SerializeField] private List<UIElement> uiElements;

    public void HideTheElements(Action callback = null)
    {
        var maxDurationElement = uiElements.OrderByDescending(element => element.GetAnimationDurationTime()).First();
        foreach (var element in uiElements)    
        {
            if(element == maxDurationElement)
                element.Hide(callback);
            else
                element.Hide();    
        }
    }

    public void ShowTheElements(Action callback = null)
    {
        var maxDurationElement = uiElements.OrderByDescending(element => element.GetAnimationDurationTime()).First();
        foreach (var element in uiElements)    
        {
            if(element == maxDurationElement)
                element.Show(callback);
            else
                element.Show();    
        }
    }

    public void HideInstantly()
    {
        uiElements.ForEach(element =>
        {
            element.Hide();
            element.CompleteCurrentTheAnimation();
        });
    }
    
    public void ShowInstantly()
    {
        uiElements.ForEach(element =>
        {
            element.Show();
            element.CompleteCurrentTheAnimation();
        });
    }

}
