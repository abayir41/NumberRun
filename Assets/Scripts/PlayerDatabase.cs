using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDatabase : MonoBehaviour
{
    public static PlayerDatabase instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    #region Settings
    
    public bool CanVibrate()
    {
        return PlayerPrefs.GetInt("_vibrate_") == 0;
    }

    public void ChangeVibrate(int vibrate)
    {
        PlayerPrefs.SetInt("_vibrate_",vibrate);
    }
    
    public bool GetSound()
    {
        return PlayerPrefs.GetInt("_sound_") == 0;
    }

    public void ChangeSound(int sound)
    {
        PlayerPrefs.SetInt("_sound_",sound);
    }
    #endregion


}
