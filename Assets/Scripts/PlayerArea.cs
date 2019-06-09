using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    [SerializeField] private PlayerEnum _player;
    [SerializeField] private Deflector _deflectorPrefab;

    private Camera _camera;
    private float _cameraDistance;
    public event Action<PlayerArea> SpawnDeflectorEvent;

    private void Awake()
    {
        EventAggregator<TurnChangedMessage>.Subscribe(OnTurnChanged);
    }

    private void Start()
    {
        _camera = Camera.main;
        _cameraDistance = -_camera.transform.position.z + transform.position.z;
    }

    private void OnDestroy()
    {
        EventAggregator<TurnChangedMessage>.Unsubscribe(OnTurnChanged);
    }

    private void OnMouseUpAsButton()
    {
        if (!enabled)
            return;

        if (SpawnDeflectorEvent != null)
            SpawnDeflectorEvent(this);
    }

    public void SpawnDeflector()
    {
        var position = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _cameraDistance - 1));
        var deflector = Instantiate<Deflector>(_deflectorPrefab, position, Quaternion.identity);
        deflector.player = _player;
        deflector.minPos = transform.position - transform.localScale / 2;
        deflector.maxPos = transform.position + transform.localScale / 2;
    }

    private void OnTurnChanged(TurnChangedMessage message)
    {
        enabled = _player == message.player;
    }
}
