using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    //-------------------------------
    //UI Configurations
    //-------------------------------

    public float UIMoveAnimationDuration => uiMoveAnimationDuration;
    public float UIFadeLoopAnimationDuration => uiFadeLoopAnimationDuration;
    public float UIFadeOutAnimationDuration => uiFadeOutAnimationDuration;
    public float UITimeBetweenPerScoreAdding => uiTimeBetweenPerScoreAdding;
    public float UIMinimizeTime => uiMinimizeTime;
    public float DurationOfSkinUnlockAnimation => durationOfSkinUnlockAnimation;
    
    [Header("UI Configurations")] 
    [SerializeField] private float uiMoveAnimationDuration;
    [SerializeField] private float uiFadeOutAnimationDuration;
    [SerializeField] private float uiFadeLoopAnimationDuration;
    [SerializeField] private float uiTimeBetweenPerScoreAdding;
    [SerializeField] private float uiMinimizeTime;
    [SerializeField] private float durationOfSkinUnlockAnimation;
}
