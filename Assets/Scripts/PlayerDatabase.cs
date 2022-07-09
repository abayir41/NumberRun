using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerDatabase : MonoBehaviour
{
    #region Settings
    
    public static bool CanVibrate()
    {
        return PlayerPrefs.GetInt("_vibrate_") == 0;
    }

    public static void ChangeVibrate(bool makeEnable)
    {
        PlayerPrefs.SetInt("_vibrate_",makeEnable ? 0 : 1);
    }
    
    public static bool CanSound()
    {
        return PlayerPrefs.GetInt("_sound_") == 0;
    }

    public static void ChangeSound(bool makeEnable)
    {
        PlayerPrefs.SetInt("_sound_",makeEnable ? 0 : 1);
    }
    #endregion


}
