using UnityEngine;

namespace GlobalTypes
{
    public enum PortalType
    {
        Choosable,//in the air
        Zero,//ground default
        One,
    }

    public enum ExpType
    {
        Add,
        Remove
    }

    public enum MobType
    {
        Normal,
        Boss
    }
    
    public class Stage
    {
        public readonly float StartPoint;
        public readonly float FinishPoint;
        public Sprite StageSprite;

        public Stage(Sprite stageSprite, float startPoint, float finishPoint)
        {
            StageSprite = stageSprite;
            FinishPoint = finishPoint;
            StartPoint = startPoint;
        }
        
    }

}