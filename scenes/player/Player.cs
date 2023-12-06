using Godot;
using System;

public partial class Player : Node2D
{
    private UnitSelectionComponent _unitSelectionComponent = null;
    private ShowUnitsSelectedComponent _showUnitsSelectedComponent = null;
    private InputComponent _inputComponent = null;

    public override void _Ready()
    {
        _unitSelectionComponent = GetNode<UnitSelectionComponent>("UnitSelectionComponent");
        _showUnitsSelectedComponent = GetNode<ShowUnitsSelectedComponent>("ShowUnitsSelectedComponent");
        _inputComponent = GetNode<InputComponent>("InputComponent");
    }


}
