using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midfield : MonoBehaviour
{
    public bool ignoreTrigger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ignoreTrigger)
            return;

        EventAggregator<MidfieldEnterMessage>.Publish(new MidfieldEnterMessage() { position = other.gameObject.transform.position });
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ignoreTrigger = false;
        EventAggregator<MidfieldExitMessage>.Publish(new MidfieldExitMessage());
    }
}
