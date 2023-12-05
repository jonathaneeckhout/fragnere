using Godot;
using System;

public partial class ShowUnitsSelectedComponent : Node
{
    public void Init(UnitSelectionComponent unit_selection_component)
    {
        unit_selection_component.UnitsSelected += OnUnitsSelected;
        unit_selection_component.UnitsDeselected += OnUnitsDeselected;
    }

    void OnUnitsSelected(Godot.Collections.Array<Node2D> units)
    {
        foreach (Node2D unit in units)
        {
            if (unit.HasMethod("SetSelected"))
            {
                unit.Call("SetSelected", true);
            }

        }
    }

    void OnUnitsDeselected(Godot.Collections.Array<Node2D> units)
    {
        foreach (Node2D unit in units)
        {
            if (unit.HasMethod("SetSelected"))
            {
                unit.Call("SetSelected", false);
            }

        }
    }
}
