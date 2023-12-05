using Godot;
using System;

public partial class Worker : CharacterBody2D
{
    public const float Speed = 300.0f;
    [Export]
    public bool selected = false;

    private Panel selectedPanel = null;
    private Vector2 target = Vector2.Zero;

    public override void _Ready()
    {
        target = Position;
        selectedPanel = GetNode<Panel>("SelectedPanel");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Position.DistanceTo(target) > 8)
        {
            Velocity = Position.DirectionTo(target) * Speed;
        }
        else
        {
            Velocity = Vector2.Zero;
        }
        MoveAndSlide();
    }


    public void SetSelected(bool value)
    {
        selectedPanel.Visible = value;
        selected = value;
    }

    public void Move(Vector2 position)
    {
        target = position;
    }
}
