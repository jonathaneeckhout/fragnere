using Godot;
using System;

public partial class UnitSelectionComponent : Node2D
{
    private Panel selectionPanel = null;
    private Area2D selectionArea = null;
    private bool dragging = false;
    private Vector2 startPoint = Vector2.Zero;

    public override void _Ready()
    {
        selectionPanel = GetNode<Panel>("SelectionPanel");
        selectionArea = GetNode<Area2D>("SelectionArea");

    }

    public override void _Process(double delta)
    {
        if (dragging)
        {
            DrawSelectionPanel();
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Hide the settings menu if visible.
        if (Input.IsActionJustPressed("right_click"))
        {
            OnRightClicked();
        }
        else if (Input.IsActionJustReleased("right_click"))
        {
            OnRightReleased();
        }
    }

    private void OnRightClicked()
    {
        dragging = true;
        startPoint = GetViewport().GetMousePosition();
        selectionPanel.Show();
    }

    private void OnRightReleased()
    {
        dragging = false;
        selectionPanel.Hide();
    }

    private void DrawSelectionPanel()
    {
        Vector2 currentMousePostion = GetViewport().GetMousePosition();
        float height = Mathf.Abs(currentMousePostion.Y - startPoint.Y);
        float width = Mathf.Abs(currentMousePostion.X - startPoint.X);
        float x;
        float y;

        selectionPanel.Size = new Vector2(width, height);

        if (currentMousePostion.X > startPoint.X)
        {
            x = startPoint.X;
        }
        else
        {
            x = currentMousePostion.X;
        }

        if (currentMousePostion.Y > startPoint.Y)
        {
            y = startPoint.Y;
        }
        else
        {
            y = currentMousePostion.Y;
        }

        selectionPanel.Position = new Vector2(x, y);
    }
}
