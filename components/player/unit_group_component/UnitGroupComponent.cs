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
        for (int i = 0; i < Units.Count; i++)
        {
            Units[i].SetUnitGroupComponent(null);
        }
        Units.Clear();
        Leader = null;
    }

    public void Move(Vector2 position)
    {
        float closestDistance = 0.0f;
        Unit closestUnit = null;
        foreach (Unit unit in Units)
        {
            if (closestUnit == null)
            {
                closestDistance = unit.Position.DistanceTo(position);
                closestUnit = unit;
            }
            else
            {
                float distance = unit.Position.DistanceTo(position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = unit;
                }
            }
            Leader = closestUnit;
            unit.Move(position);
        }
    }

}
