using Godot;
using System;

public partial class Player : Node2D
{
    private UnitSelectionComponent unit_selection_component = null;
    private ShowUnitsSelectedComponent show_unit_component = null;
    public override void _Ready()
    {
        unit_selection_component = GetNode<UnitSelectionComponent>("UnitSelectionComponent");
        show_unit_component = GetNode<ShowUnitsSelectedComponent>("ShowUnitsSelectedComponent");
        show_unit_component.Init(unit_selection_component);
    }
    public override void _Input(InputEvent @event)
    {
        // Hide the settings menu if visible.
        if (Input.IsActionJustPressed("right_click"))
        {
            Vector2 position = GetGlobalMousePosition();
            foreach (Node2D unit in unit_selection_component.selectedUnits)
            {
                if (unit.HasMethod("Move"))
                {
                    unit.Call("Move", position);
                }

            }
        }
    }

}
