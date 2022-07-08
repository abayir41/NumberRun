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
    public float UIMinimizeTime => uiMinimizeTime;
    
    [Header("UI Configurations")] 
    [SerializeField] private float uiMoveAnimationDuration;
    [SerializeField] private float uiFadeOutAnimationDuration;
    [SerializeField] private float uiFadeLoopAnimationDuration;
    [SerializeField] private float uiMinimizeTime;
}
