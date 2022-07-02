using System;
using System.Collections.Generic;
using System.Linq;
using GlobalTypes;
using UnityEngine;

public class IQStageManager : MonoBehaviour
{
    public int startPoint;
    public int targetPoint;
    [SerializeField] private int stageCount;
    [SerializeField] private List<Sprite> stageSprites;

    private List<Stage> _iqStages;
    private Stage _currentStage;
    private float _currentRatioOfBetweenStages;
    
    private void Awake()
    {
        if (stageCount != stageSprites.Count)
            throw new Exception("Stage Sprites not enough for Stage Count");

        _iqStages = new List<Stage>();
        
        //for calculations
        var stepAmount = (float)(targetPoint - startPoint) / stageCount;
        
        for (var i = 0; i < stageCount; i++)
        {
            var stage = new Stage(stageSprites[i], i * stepAmount, (i + 1) * stepAmount);
            _iqStages.Add(stage);
            
            Debug.Log(stage.StartPoint + " - " + stage.FinishPoint);
        }
        
        

        _currentStage = _iqStages.First();

    }

    private void OnEnable()
    {
        ProjectEvents.ScoreChanged += ScoreChanged;
    }
    
    private void OnDisable()
    {
        ProjectEvents.ScoreChanged -= ScoreChanged;
    }

    private void ScoreChanged(int newScore)
    {
        _currentStage = WhatStageEquivalentForScore(newScore);
        _currentRatioOfBetweenStages = (newScore - _currentStage.StartPoint) / (_currentStage.FinishPoint - _currentStage.StartPoint);
    }

    private Stage WhatStageEquivalentForScore(int score)
    {
        foreach (var stage in _iqStages)
        {
            if (score > stage.StartPoint && score < stage.FinishPoint)
                return stage;
        }
        
        if (score > _iqStages.Last().FinishPoint)
        {
            return  _iqStages.Last();
        }

        throw new Exception($"There is no equivalent stage for this score: {score}");
    }
}
