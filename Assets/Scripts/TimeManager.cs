using UnityEngine;

public static class TimeManager
{
    public static void ChangeTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
