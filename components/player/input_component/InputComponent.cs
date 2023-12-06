using Godot;
using System;

public partial class InputComponent : Node2D
{
    [Export]
    public UnitGroupComponent UnitGroupComponent = null;
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("right_click"))
        {
            Vector2 position = GetGlobalMousePosition();
            UnitGroupComponent.Move(position);
        }
    }
}
