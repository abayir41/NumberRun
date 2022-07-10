using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHorizontalMover : MonoBehaviour
{
    public static PlayerHorizontalMover Instance;
    public Vector3 offset = new Vector3(0, 0.3f, 0);
    [SerializeField] public SliderJoystick sliderJoystick;
    [SerializeField] private Transform cachedTransform;
    [SerializeField] public Player player;
    
    private bool controlled;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        if (cachedTransform == null)
        {
            cachedTransform = this.transform;
        }
        if (player == null)
        {
            player = GetComponent<Player>();
        }
    }

    public void Update()
    {
        if (player.GameState == GameState.Started)
        {
            MoveH(sliderJoystick.HorizontalPosition);
        }
    }
    public void MoveH(float Position)
    {
        Position = Mathf.Clamp(Position, -1, 1);
        Vector3 nextPos = cachedTransform.localPosition + offset;
        nextPos.x = Position * GameManager.Instance.moveCenter;
        cachedTransform.localPosition = nextPos / GameManager.Instance.halfMovement + offset;
    }
}
