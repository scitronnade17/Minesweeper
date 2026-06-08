using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    [Header("Field Settings")]
    [Range(1, 19)]
    public int fieldWidth;
    [Range(1, 8)]
    public int fieldHeight;
    [Range(1, 151)]
    public int minesCount;

    [Header("Cell Prefab")]
    public GameObject cellPrefab;

    [Header("Sprites")]
    public Sprite emptySprite;
    public Sprite flagSprite;
    public Sprite mineSprite;
}