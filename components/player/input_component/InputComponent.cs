using Godot;
using System;

public partial class InputComponent : Node2D
{
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("right_click"))
        {
            Vector2 position = GetGlobalMousePosition();
            GetTree().CallGroup("selected", "Move", position);
        }
    }
}
