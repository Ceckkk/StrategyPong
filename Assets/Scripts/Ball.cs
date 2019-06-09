using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _initialForce = 1000f;
    private Rigidbody2D _rigidbody;
    private TrailRenderer _trailRenderer;
    private Light _light;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _light = GetComponent<Light>();
    }

    public void SetLightEnabled(bool flag)
    {
        _light.enabled = flag;
    }

    public void KickOff(Vector2 direction)
    {
        _rigidbody.AddForce(direction * _initialForce, ForceMode2D.Impulse);
        Debug.Log("GO!");
    }

    public Coroutine RespawnBall()
    {
        return StartCoroutine(RespawnBallRoutine());
    }

    private IEnumerator RespawnBallRoutine()
    {
        yield return new WaitForSecondsRealtime(1);

        _trailRenderer.emitting = false;

        yield return new WaitForEndOfFrame();

        transform.position = Vector3.zero;
        _rigidbody.velocity = Vector2.zero;

        yield return new WaitForEndOfFrame();

        _trailRenderer.emitting = true;

        Debug.Log("Ready?");
    }
}
