using System.Collections.Generic;

public sealed class GameData
{
    private static readonly GameData _instance = new GameData(); 

    public static GameData Instance
    {
        get
        {
            return _instance;
        }
    }

    public bool IsNewGame { get; set; }

    private GameData()
    {

    }
}
