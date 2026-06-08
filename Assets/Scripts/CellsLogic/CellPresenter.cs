using System;
using UnityEngine;

public class CellPresenter
{
    public Vector2Int Position => model.Position;
    public bool IsMine => model.IsMine;
    public bool IsReveal => model.IsReveal;
    public bool IsFlag => model.IsFlag;
    public bool IsEmpty => model.IsEmpty;

    public event Action<CellPresenter> OnLeftClick;
    public event Action<CellPresenter> OnRightClick;
    public event Action OnFlagChanged;

    private readonly CellModel model;
    private readonly CellView view;

    public CellPresenter(CellModel _model, CellView _view)
    {
        model = _model;
        view = _view;

        view.Bind(model);
        view.OnLeftClick += _ => OnLeftClick?.Invoke(this);
        view.OnRightClick += _ => OnRightClick?.Invoke(this);
    }

    public void Reveal() => model.Reveal();
    public void ToggleFlag()
    {
        model.ToggleFlag();
        if (!model.IsReveal)
            OnFlagChanged?.Invoke();
    }
    public void SetMine() => model.SetMine();
    public void ClearMine() => model.ClearMine();
    public void SetNumber(int count) => model.SetNumber(count);
}