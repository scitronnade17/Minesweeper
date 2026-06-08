using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CellView : MonoBehaviour, IPointerClickHandler
{
    public CellModel Model { get; private set; }

    private Image image;
    private TMP_Text numberText;

    private Sprite defaultSprite;
    private Sprite emptySprite;
    private Sprite flagSprite;
    private Sprite mineSprite;

    public event Action<CellView> OnLeftClick;
    public event Action<CellView> OnRightClick;

    public void Init(Sprite empty, Sprite flag, Sprite mine)
    {
        image = GetComponent<Image>();
        numberText = GetComponentInChildren<TextMeshProUGUI>(includeInactive: true);

        defaultSprite = image.sprite;
        emptySprite = empty;
        flagSprite = flag;
        mineSprite = mine;
    }

    public void Bind(CellModel model)
    {
        Model = model;
        Model.OnStateChanged += Refresh;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Model.IsReveal) return;

        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftClick?.Invoke(this);
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightClick?.Invoke(this);
    }

    private void Refresh(CellModel model)
    {
        if (model.IsReveal)
        {
            image.sprite = model.IsMine ? mineSprite : emptySprite;

            if (!model.IsMine && model.NeighbourMineCount > 0)
            {
                numberText.gameObject.SetActive(true);
                numberText.text = model.NeighbourMineCount.ToString();
                numberText.color = GetNumberColor(model.NeighbourMineCount);
            }
        }
        else
            image.sprite = model.IsFlag ? flagSprite : defaultSprite;
    }

    private Color GetNumberColor(int count) => count switch
    {
        1 => new Color(0f, 0f, 1f),
        2 => new Color(0f, 0.5f, 0f),
        3 => new Color(1f, 0f, 0f),
        4 => new Color(0f, 0f, 0.5f),
        5 => new Color(0.5f, 0f, 0f),
        6 => new Color(0f, 0.5f, 0.5f),
        7 => new Color(0f, 0f, 0f),
        _ => new Color(0.5f, 0.5f, 0.5f),
    };

    private void OnDestroy()
    {
        if (Model != null)
            Model.OnStateChanged -= Refresh;
    }
}