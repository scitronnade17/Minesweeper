using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(GridLayoutGroup))]
public class GameFieldLayout : MonoBehaviour
{
    private IConfigDataService configDataService;

    [Inject]
    public void Construct(IConfigDataService _configDataService)
    {
        configDataService = _configDataService;
    }

    public void Apply()
    {
        var gridLayout = GetComponent<GridLayoutGroup>();
        GameData data = configDataService.GetGameData();

        gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = data.fieldWidth;

        float totalWidth = data.fieldWidth * gridLayout.cellSize.x
                          + (data.fieldWidth - 1) * gridLayout.spacing.x
                          + gridLayout.padding.left * 2;

        float totalHeight = data.fieldHeight * gridLayout.cellSize.y
                          + (data.fieldHeight - 1) * gridLayout.spacing.y
                          + gridLayout.padding.top * 2;

        GetComponent<RectTransform>().sizeDelta = new Vector2(totalWidth, totalHeight);
    }
}