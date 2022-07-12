using UnityEngine;


[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    //-------------------------------
    //Game Configurations
    //-------------------------------
    public float TimeEquivalentScore => timeEquivalentScore;
    
    [Header("Game Configurations")] 
    [SerializeField] private float timeEquivalentScore;
   
    
    
    //-------------------------------
    //UI Configurations
    //-------------------------------

    public float UIMoveAnimationDuration => uiMoveAnimationDuration;
    public float UIFadeLoopAnimationDuration => uiFadeLoopAnimationDuration;
    public float UIFadeOutAnimationDuration => uiFadeOutAnimationDuration;
    public float UIMinimizeTime => uiMinimizeTime;
    public float UITimeDecreasingTime => uiTimeDecreasingTime;
    public float UIScaleLoopDuration => uiScaleLoopDuration;
    public float UIRotationLoopDuration => uiRotationLoopDuration;
    
    [Header("UI Configurations")] 
    [SerializeField] private float uiMoveAnimationDuration;
    [SerializeField] private float uiFadeOutAnimationDuration;
    [SerializeField] private float uiFadeLoopAnimationDuration;
    [SerializeField] private float uiMinimizeTime;
    [SerializeField] private float uiTimeDecreasingTime;
    [SerializeField] private float uiScaleLoopDuration;
    [SerializeField] private float uiRotationLoopDuration;

}
