using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BezierSolution;
using System;

public class BeziePathFollower : MonoBehaviour
{
    public float PassedPath => passedPath;
    public event Action PathCompleted;

    [SerializeField] public BezierSpline bezierSpline;
    [SerializeField] private float speed;
    [SerializeField] public Player player;
    public bool canMove = false;

    private float passedPath;
    private Transform cachedTransform;
    private bool IsPathCompleted;
    public bool IsFinalMap = false;
    private void Start()
    {
        cachedTransform = this.transform;
    }
    private void Update()
    {
        if (canMove)
        {
            TryMove();
        }
    }

    private bool TryMove()
    {
        if (!IsPathCompleted)
        {
            SetNextPosition();
            if (!IsFinalMap)
                SetNextRotation();

            if (passedPath > GameManager.Instance.totalPathIfNotFinish)
            {
                PathCompleted?.Invoke();
                IsPathCompleted = true;
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    

    public void StartFollower()
    {
        player.Died += Stop;
    }

    private void OnDisable()
    {
        if (player != null)
            player.Died -= Stop;
    }

    private void Stop()
    {
        speed = 0;
    }
    private void SetNextPosition()
    {
        var nextPos = bezierSpline.MoveAlongSpline(ref passedPath, speed * Time.deltaTime);
        cachedTransform.position = nextPos;
    }

    private void SetNextRotation()
    {
        BezierSpline.Segment segment = bezierSpline.GetSegmentAt(passedPath);
        var targetRotation = Quaternion.LookRotation(segment.GetTangent(), segment.GetNormal());
        cachedTransform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

    }

    public void SetSpeed(float _speed)
    {
        speed = _speed;
    }
    
    public float GetSpeed()
    {
        return speed;
    }
}
