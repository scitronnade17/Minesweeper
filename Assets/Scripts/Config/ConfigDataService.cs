using UnityEngine;

public interface IConfigDataService
{
    void Load();
    GameData GetGameData();
}

public class ConfigDataService : IConfigDataService
{
    private GameData gameData;

    public void Load()
    {
        gameData = Resources.Load<GameData>("Configs/GameData");
    }

    public GameData GetGameData() => gameData;
}
