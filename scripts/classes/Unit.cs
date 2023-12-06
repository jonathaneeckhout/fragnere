using Godot;
using System;

public partial class Unit : CharacterBody2D
{

    public bool Selected = false;

    protected Panel SelectedPanel = null;
    protected SteeringComponent SteeringComponent = null;

    public override void _Ready()
    {
        SteeringComponent = GetNode<SteeringComponent>("SteeringComponent");
        SelectedPanel = GetNode<Panel>("SelectedPanel");
    }


    public void SetSelected(bool value)
    {
        SelectedPanel.Visible = value;
        Selected = value;
    }

    public void Move(Vector2 position)
    {
        SteeringComponent.TargetPostion = position;
    }
}
