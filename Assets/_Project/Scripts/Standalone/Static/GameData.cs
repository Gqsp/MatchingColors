public static class GameData
{
    public static StartMode startMode;
    public static MenuLoadMode menuLoadMode;
    public static float timer;
    public static int deaths;
    public static int collectables;
}

public enum MenuLoadMode
{
    Startup,
    NormalGameEnd,
    SliperyGameEnd,
    BouncyGameEnd
}
