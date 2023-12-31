using Godot;
using System;

public partial class ShowUnitsSelectedComponent : Node
{
    [Export]
    private UnitSelectionComponent _unitSelectionComponent = null;

    public override void _Ready()
    {
        _unitSelectionComponent.UnitsSelected += OnUnitsSelected;
        _unitSelectionComponent.UnitsDeselected += OnUnitsDeselected;
    }

    void OnUnitsSelected(Godot.Collections.Array<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unit.SetSelected(true);
        }
    }

    void OnUnitsDeselected(Godot.Collections.Array<Unit> units)
    {
        foreach (Unit unit in units)
        {
            unit.SetSelected(false);
        }
    }
}
