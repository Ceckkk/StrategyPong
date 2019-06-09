using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalModeManager : MonoBehaviour
{
    [SerializeField] private float _timeScale = 0.01f;

    [SerializeField] private Ball _ball;

    [SerializeField] private int _maxDeflectorCount = 1;

    [SerializeField] private Button _endTurnButton;

    private int _deflectorCount = 0;

    private PlayerEnum _currentPlayer = PlayerEnum.No_one;

    private bool CanSpawnDeflector { get => _deflectorCount < _maxDeflectorCount; }

    private void Awake()
    {
        EventAggregator<GoalAreaEnterMessage>.Subscribe(OnGoalAreaEnter);
        EventAggregator<MidfieldEnterMessage>.Subscribe(OnMidfieldEnter);
        EventAggregator<MidfieldExitMessage>.Subscribe(OnMidfieldExit);
        EventAggregator<SpawnDeflectorMessage>.Subscribe(OnSpawnDeflector);
    }

    private void Start()
    {
        _endTurnButton.onClick.AddListener(EndTurn);
        KickOff(PlayerEnum.Player1);
    }

    private void OnDestroy()
    {
        EventAggregator<GoalAreaEnterMessage>.Unsubscribe(OnGoalAreaEnter);
        EventAggregator<MidfieldEnterMessage>.Unsubscribe(OnMidfieldEnter);
        EventAggregator<MidfieldExitMessage>.Unsubscribe(OnMidfieldExit);
        EventAggregator<SpawnDeflectorMessage>.Unsubscribe(OnSpawnDeflector);
    }

    private void OnMidfieldEnter(MidfieldEnterMessage message)
    {
        TimeManager.ChangeTimeScale(_timeScale);
        if (message.position.x > 0)
            ChangeTurn(PlayerEnum.Player1);
        else if (message.position.x < 0)
            ChangeTurn(PlayerEnum.Player2);
    }

    private void OnMidfieldExit(MidfieldExitMessage message)
    {
        EndTurn();
    }

    private void OnDeflectorSpawned()
    {
        _deflectorCount++;
    }

    private void ChangeTurn(PlayerEnum player)
    {
        // _ball.SetLightEnabled(player != Player.No_one);
        _currentPlayer = player;
        _endTurnButton.gameObject.SetActive(player != PlayerEnum.No_one);

        EventAggregator<TurnChangedMessage>.Publish(new TurnChangedMessage() { player = player });
    }

    private void EndTurn()
    {
        _deflectorCount = 0;
        TimeManager.ChangeTimeScale(1);
        ChangeTurn(PlayerEnum.No_one);
    }

    private void KickOff(PlayerEnum player)
    {
        // _midfield.ignoreTrigger = true;
        _ball.KickOff(player == PlayerEnum.Player1 ? Vector2.left : Vector2.right);
        TimeManager.ChangeTimeScale(_timeScale);
        ChangeTurn(player);
    }

    private void OnGoalAreaEnter(GoalAreaEnterMessage message)
    {
        Debug.Log(message.player + " scored!");

        StartCoroutine(RespawnRoutine(() =>
        {
            EventAggregator<GoalMessage>.Publish(new GoalMessage() { player = message.player });
            KickOff(message.player == PlayerEnum.Player1 ? PlayerEnum.Player2 : PlayerEnum.Player1);
        }));
    }

    private void OnSpawnDeflector(SpawnDeflectorMessage message)
    {
        if (CanSpawnDeflector)
        {
            message.playerArea.SpawnDeflector();
            _deflectorCount++;
        }
    }

    private IEnumerator RespawnRoutine(Action callback)
    {
        yield return _ball.RespawnBall();

        if (callback != null)
            callback();
    }
}
