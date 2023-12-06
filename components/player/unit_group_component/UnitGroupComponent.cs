using Godot;
using System;

public partial class UnitGroupComponent : Node
{

    [Export]
    private UnitSelectionComponent _unitSelectionComponent = null;

    public Unit Leader = null;
    public Godot.Collections.Array<Unit> Units = new();

    public override void _Ready()
    {
        _unitSelectionComponent.UnitsSelected += OnUnitsSelected;
        _unitSelectionComponent.UnitsDeselected += OnUnitsDeselected;
    }

    void OnUnitsSelected(Godot.Collections.Array<Unit> units)
    {
        Leader = null;
        Units = units.Duplicate();
        for (int i = 0; i < Units.Count; i++)
        {
            if (i == 0)
            {
                Leader = Units[i];
            }
            Units[i].SetUnitGroupComponent(this);
        }
    }

    void OnUnitsDeselected(Godot.Collections.Array<Unit> units)
    {
        Leader = null;
        Units.Clear();
    }

    public void Move(Vector2 position)
    {
        foreach (Unit unit in Units)
        {
            unit.Move(position);
        }
    }

}
