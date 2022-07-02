using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectEvents
{
    public static Action GameWin;

    public static Action GameLost;

    //Passing the new score
    public static Action<int> ScoreChanged;

    public static Action StageChanged;
}
