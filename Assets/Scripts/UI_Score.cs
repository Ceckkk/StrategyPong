using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UI_Score : MonoBehaviour
{
    [SerializeField] private PlayerEnum _player;
    [SerializeField] private int _score = 0;
    private Text _text;

    private void Awake()
    {
        EventAggregator<GoalMessage>.Subscribe(OnGoal);
    }

    private void OnDestroy()
    {
        EventAggregator<GoalMessage>.Unsubscribe(OnGoal);
    }

    private void Start()
    {
        _text = GetComponent<Text>();
        _score = 0;
        UpdateScore();
    }

    private void OnGoal(GoalMessage message)
    {
        if (message.player == _player)
        {
            _score++;
            UpdateScore();
        }
    }

    private void UpdateScore()
    {
        _text.text = "" + _score;
    }
}
