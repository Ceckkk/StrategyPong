using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflector : MonoBehaviour
{
    public PlayerEnum player;
    public Vector2 maxPos;
    public Vector2 minPos;

    private Vector3 _offset;
    private Camera _camera;
    private float _cameraDistance;

    private void Awake()
    {
        EventAggregator<GoalMessage>.Subscribe(OnGoal);
        EventAggregator<TurnChangedMessage>.Subscribe(OnTurnChanged);
    }

    private void Start()
    {
        _camera = Camera.main;
        _cameraDistance = -_camera.transform.position.z + transform.position.z;
    }

    private void OnDestroy()
    {
        EventAggregator<GoalMessage>.Unsubscribe(OnGoal);
        EventAggregator<TurnChangedMessage>.Unsubscribe(OnTurnChanged);
    }

    private void OnMouseDown()
    {
        if (!enabled)
            return;

        _offset = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraDistance)) - transform.position;
    }

    private void OnMouseDrag()
    {
        if (!enabled)
            return;

        var position = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraDistance)) - _offset;

        position.x = Mathf.Clamp(position.x, minPos.x, maxPos.x);
        position.y = Mathf.Clamp(position.y, minPos.y, maxPos.y);
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DestroyDeflector();
    }

    private void OnGoal(GoalMessage message)
    {
        DestroyDeflector();
    }

    private void DestroyDeflector()
    {
        Destroy(gameObject);
    }

    private void OnTurnChanged(TurnChangedMessage message)
    {
        enabled = player == message.player;
    }
}