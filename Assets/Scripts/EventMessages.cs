using UnityEngine;

public class GoalAreaEnterMessage
{
    public PlayerEnum player;
}

public class MidfieldEnterMessage
{
    public Vector3 position;
}

public class MidfieldExitMessage
{
}

public class GoalMessage
{
    public PlayerEnum player;
}

public class SpawnDeflectorMessage
{
    public PlayerArea playerArea;
}

public class TurnChangedMessage
{
    public PlayerEnum player;
}