using System;
using GlobalTypes;

public static class ProjectEvents
{
    public static Action GameWin;

    public static Action GameLost;

    //Passing the new score
    public static Action<float> ScoreChanged;
    //Passing new time
    public static Action<int> TimeChanged;

    #region IQStages

    //first arg is where we upped, second arg is what is the next stage
    public static Action<Stage> StageUpped;

    //first arg is where we upped, second arg is what is the next stage
    public static Action<Stage> StageDowned;
    
    public static Action<float> LastRatioChanged;

    public static Action StagesPassedOverTheTarget;

    #endregion

    #region SliderAnimEvents

    public static Action StageUppedAnimCompleted;

    public static Action StageDownAnimCompleted;
    
    public static Action StagesPassedOverTheTargetAnimCompleted;
    
    #endregion

    #region UIEvents

    public static Action UITargetTextStartedTheHideAnimation;

    #endregion
}
