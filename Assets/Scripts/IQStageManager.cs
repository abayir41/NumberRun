using System;
using System.Collections.Generic;
using System.Linq;
using GlobalTypes;
using UnityEngine;

public class IQStageManager : MonoBehaviour
{
    public static IQStageManager Instance { get; private set; }
    
    private int _targetPoint;
    [SerializeField] private int stageCount;
    [SerializeField] private List<Sprite> stageSprites;

    private List<Stage> _iqStages;
    private Stage _currentStage;
    private float _currentRatioOfBetweenStages;
    private float _score;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        
        if (stageCount != stageSprites.Count)
            throw new Exception("Stage Sprites not enough for Stage Count");

        _iqStages = new List<Stage>();
        
    }

    private void OnEnable()
    {
        ProjectEvents.ScoreChanged += ScoreChanged;
    }
    
    private void OnDisable()
    {
        ProjectEvents.ScoreChanged -= ScoreChanged;
    }

    public void SetupTheIQStates(int targetPoint)
    {
        _targetPoint = targetPoint;
        
        //for calculations
        var stepAmount = (float)targetPoint / stageCount;

        //Creating iq stages
        for (var i = 0; i < stageCount; i++)
        {
            var stage = new Stage(stageSprites[i], i * stepAmount, (i + 1) * stepAmount);
            _iqStages.Add(stage);
        }

        _currentStage = _iqStages.First();
    }

    private void ScoreChanged(float newScore)
    {
        var currentAbsoluteDistance = _targetPoint - Math.Min(_targetPoint, Math.Abs(_score - _targetPoint));
        
        var newAbsoluteDistance = _targetPoint - Math.Min(_targetPoint, Math.Abs(newScore - _targetPoint));
        var newStage = WhatStageEquivalentForScore(newAbsoluteDistance);
        var newRatio = (newAbsoluteDistance - newStage.StartPoint) / (newStage.FinishPoint - newStage.StartPoint);

        //Throwing events for UI
        UIManager.Instance.PrepareSequenceForSlide();
        if (IsScoreInRightSide(_score) != IsScoreInRightSide(newScore))
        {
            while (!_currentStage.Equals(_iqStages.Last()))
            {
                _currentStage = _iqStages[_iqStages.IndexOf(_currentStage) + 1];
                ProjectEvents.StageUpped?.Invoke(_currentStage);
            }
            ProjectEvents.StagesPassedOverTheTarget?.Invoke();
        }

        while (!_currentStage.Equals(newStage))
        {
            var index = _iqStages.IndexOf(_currentStage);
            if (currentAbsoluteDistance < newAbsoluteDistance)
            {
                _currentStage = _iqStages[index + 1];
                ProjectEvents.StageUpped?.Invoke(_currentStage);
            }
            else
            {
                _currentStage = _iqStages[index - 1];
                ProjectEvents.StageDowned?.Invoke(_currentStage);
            }
        }

        _score = newScore;
        _currentRatioOfBetweenStages = newRatio;
        ProjectEvents.LastRatioChanged?.Invoke(_currentRatioOfBetweenStages);
        
    }

    private Stage WhatStageEquivalentForScore(float score)
    {
        foreach (var stage in _iqStages)
        {
            if (score >= stage.StartPoint && score <= stage.FinishPoint)
                return stage;
        }
        
        if (score > _iqStages.Last().FinishPoint)
        {
            return  _iqStages.Last();
        }
        
        if (score < _iqStages.First().StartPoint)
        {
            return  _iqStages.First();
        }

        throw new Exception($"There is no equivalent stage for this score: {score}");
    }

    private bool IsScoreInRightSide(float score)
    {
        return score > _targetPoint;
    }

    public Stage GetCurrentStage()
    {
        return _currentStage;
    }

    public Stage GetPreviousStage(Stage stage)
    {
        if (_iqStages.IndexOf(stage) == 0)
            return null;
        else
            return _iqStages[_iqStages.IndexOf(stage) - 1];
    }
}
