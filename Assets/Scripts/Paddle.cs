using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] private PlayerEnum _player;
    [SerializeField] private float _maxInputDistance;

    private Vector3 _offset;
    private Camera _camera;
    private float _cameraDistance;

    private SpriteRenderer _renderer;
    private Light _light;
    private LeanFinger _finger;

    private void Awake()
    {
        EventAggregator<GoalMessage>.Subscribe(OnGoal);
        EventAggregator<TurnChangedMessage>.Subscribe(OnTurnChanged);
    }

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _light = GetComponent<Light>();
        _camera = Camera.main;
        _cameraDistance = -_camera.transform.position.z + transform.position.z;

        ResetPosition();
    }

    private void OnDestroy()
    {
        EventAggregator<GoalMessage>.Unsubscribe(OnGoal);
        EventAggregator<TurnChangedMessage>.Unsubscribe(OnTurnChanged);
    }

    private void Update()
    {
        var fingers = LeanTouch.GetFingers(false, false);

        if (fingers.Count == 0)
            return;

        if (fingers.Count > 1)
        {
            var twistDegrees = LeanGesture.GetTwistDegrees(fingers);
            transform.Rotate(0, 0, twistDegrees);
        }
        else
        {
            var finger = fingers[0];
            if (finger.Down)
            {
                _offset = finger.GetWorldPosition(_cameraDistance, _camera) - transform.position;
            }
            else
            {
                var position = transform.position;
                var dragPosition = finger.GetWorldPosition(_cameraDistance, _camera);
                if (Mathf.Abs(dragPosition.x - transform.position.x) <= _maxInputDistance)
                {
                    dragPosition -= _offset;
                    position.y = Mathf.Clamp(dragPosition.y, -4.5f, 4.5f);
                    transform.position = position;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TimeManager.ChangeTimeScale(1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TimeManager.ChangeTimeScale(0.01f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TimeManager.ChangeTimeScale(1f);
    }

    private void ResetPosition()
    {
        transform.position = new Vector2(transform.position.x, 0);
        transform.rotation = Quaternion.identity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _maxInputDistance);
    }

    private void OnTurnChanged(TurnChangedMessage message)
    {
        enabled = _player == message.player;

        var active = _player == message.player || message.player == PlayerEnum.No_one;
        _light.enabled = active;
        var color = _renderer.color;
        color.a = active ? 1f : 50f / 255f;
        _renderer.color = color;
    }

    private void OnGoal(GoalMessage message)
    {
        ResetPosition();
    }
}
