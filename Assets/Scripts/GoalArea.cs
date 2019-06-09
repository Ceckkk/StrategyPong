using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    [SerializeField] private PlayerEnum _player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("GOAL!");
        EventAggregator<GoalMessage>.Publish(new GoalMessage() { player = _player });
    }
}
