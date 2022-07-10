using System;
using System.Collections;
using System.Collections.Generic;
using BezierSolution;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action Died;
    public GameState GameState { get; private set; } = GameState.NotStarted;

    [SerializeField] public BeziePathFollower pathFolower;
    [SerializeField] private SliderJoystick SliderJoystick;
    private bool isMain = false;
    
    private void Awake()
    {
        if (pathFolower == null)
        {
            pathFolower = GetComponentInParent<BeziePathFollower>();
        }
    }

    public void StartPlayer(bool _isMain)
    {
        isMain = _isMain;
        if (isMain)
        {
            GameState = GameState.Started;
            pathFolower.canMove = true;
            pathFolower.SetSpeed(GameManager.Instance.speed);
            pathFolower.PathCompleted += LevelComplete;
            SliderJoystick.FirstTouch += GameStart;
        }
    }

    public void DiePlayer()
    {
        GameState = GameState.NotStarted;
        pathFolower.canMove = false;
    }

    public void ChangeRoute(BezierSpline path)
    {
        pathFolower.bezierSpline = path;
    }
    
    private void OnDisable()
    {
        if (isMain)
        {
            pathFolower.PathCompleted -= LevelComplete;
            SliderJoystick.FirstTouch -= GameStart;
        }
    }

    public void LevelComplete()
    {
        LevelManager.Instance.PathCompleted();
        GameState = GameState.LevelEnded;
        Died?.Invoke();
    }

    public void Die()
    {
        
        GameState = GameState.PlayerDied;
        Died?.Invoke();
    }

    public void LevelDone()
    {
        GameState = GameState.LevelEnded;
    }

    private void GameStart()
    {
        GameState = GameState.Started;
    }
}
