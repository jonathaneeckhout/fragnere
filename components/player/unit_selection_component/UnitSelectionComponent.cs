using System;
using Godot;

public partial class UnitSelectionComponent : Node2D
{
    [Signal]
    public delegate void UnitsSelectedEventHandler(Godot.Collections.Array<Unit> units);

    [Signal]
    public delegate void UnitsDeselectedEventHandler(Godot.Collections.Array<Unit> units);


    [Export]
    public Godot.Collections.Array<Unit> selectedUnits = new();

    private Panel selectionPanel = null;

    private bool dragging = false;
    private Vector2 startPoint = Vector2.Zero;


    public override void _Ready()
    {
        selectionPanel = GetNode<Panel>("SelectionPanel");
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
        if (Input.IsActionJustPressed("left_click"))
        {
            OnRightClicked();
        }
        else if (Input.IsActionJustReleased("left_click"))
        {
            OnRightReleased();
        }
    }

    private void OnRightClicked()
    {
        dragging = true;
        startPoint = GetGlobalMousePosition();
        selectionPanel.Show();
    }

    private void OnRightReleased()
    {
        Vector2 endPoint = GetGlobalMousePosition();
        float height = Mathf.Abs(endPoint.Y - startPoint.Y);
        float width = Mathf.Abs(endPoint.X - startPoint.X);

        dragging = false;

        if (selectedUnits.Count > 0)
        {
            EmitSignal(SignalName.UnitsDeselected, selectedUnits);
            selectedUnits.Clear();
        }

        RectangleShape2D selectRectangle = new()
        {
            Size = new Vector2(width, height)
        };
        PhysicsDirectSpaceState2D space = GetWorld2D().DirectSpaceState;
        Vector2 topCorner = CalculateTopCorner(startPoint, endPoint);
        PhysicsShapeQueryParameters2D query = new()
        {
            CollideWithAreas = false,
            CollideWithBodies = true,
            CollisionMask = 2,
            Shape = selectRectangle,
            Transform = new Transform2D(0, new Vector2(topCorner.X + (width / 2), topCorner.Y + (height / 2)))
        };
        Godot.Collections.Array<Godot.Collections.Dictionary> selected = space.IntersectShape(query);
        foreach (Godot.Collections.Dictionary res in selected)
        {
            Node node = res["collider"].As<Node>();
            if (node is Unit unit)
            {
                selectedUnits.Add(unit);
            }
        }

        if (selectedUnits.Count > 0)
        {
            EmitSignal(SignalName.UnitsSelected, selectedUnits);
        }

        selectionPanel.Hide();
    }

    private void DrawSelectionPanel()
    {
        Vector2 currentMousePostion = GetGlobalMousePosition();
        float height = Mathf.Abs(currentMousePostion.Y - startPoint.Y);
        float width = Mathf.Abs(currentMousePostion.X - startPoint.X);
        Vector2 size = new(width, height);

        selectionPanel.Size = size;

        selectionPanel.Position = CalculateTopCorner(currentMousePostion, startPoint);
    }

    private static Vector2 CalculateTopCorner(Vector2 start, Vector2 end)
    {
        Vector2 topCorner = new();

        if (end.X > start.X)
        {
            topCorner.X = start.X;
        }
        else
        {
            topCorner.X = end.X;
        }

        if (end.Y > start.Y)
        {
            topCorner.Y = start.Y;
        }
        else
        {
            topCorner.Y = end.Y;
        }
        return topCorner;
    }
}
