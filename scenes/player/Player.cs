using Godot;
using System;

public partial class Player : Node2D
{
    private UnitSelectionComponent unit_selection_component = null;
    private ShowUnitsSelectedComponent show_unit_component = null;
    private InputComponent input_component = null;
    public override void _Ready()
    {
        unit_selection_component = GetNode<UnitSelectionComponent>("UnitSelectionComponent");
        show_unit_component = GetNode<ShowUnitsSelectedComponent>("ShowUnitsSelectedComponent");
        show_unit_component.Init(unit_selection_component);
        input_component = GetNode<InputComponent>("InputComponent");
    }


}
