using Godot;

public partial class UnitSelectionComponent : Node2D
{
    [Signal]
    public delegate void UnitsSelectedEventHandler(Godot.Collections.Array<Node2D> units);

    [Signal]
    public delegate void UnitsDeselectedEventHandler(Godot.Collections.Array<Node2D> units);


    [Export]
    public Godot.Collections.Array<Node2D> selectedUnits = new();

    private Panel selectionPanel = null;
    private Area2D selectionArea = null;

    private CollisionShape2D selectionAreaCollisionShape = null;

    private bool dragging = false;
    private Vector2 startPoint = Vector2.Zero;

    private Godot.Collections.Array<Node2D> underSelectionUnits = new();


    public override void _Ready()
    {
        selectionPanel = GetNode<Panel>("SelectionPanel");
        selectionArea = GetNode<Area2D>("SelectionArea");
        selectionAreaCollisionShape = GetNode<CollisionShape2D>("SelectionArea/CollisionShape2D");

        selectionArea.BodyEntered += OnBodyEntered;
        selectionArea.BodyExited += OnBodyExited;
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
        selectionAreaCollisionShape.Show();
    }

    private void OnRightReleased()
    {
        dragging = false;

        if (selectedUnits.Count > 0)
        {
            EmitSignal(SignalName.UnitsDeselected, selectedUnits);
        }

        selectedUnits = underSelectionUnits.Duplicate();

        if (selectedUnits.Count > 0)
        {
            EmitSignal(SignalName.UnitsSelected, selectedUnits);
        }

        underSelectionUnits.Clear();

        selectionPanel.Hide();
        selectionAreaCollisionShape.Hide();
    }

    private void DrawSelectionPanel()
    {
        Vector2 currentMousePostion = GetGlobalMousePosition();
        float height = Mathf.Abs(currentMousePostion.Y - startPoint.Y);
        float width = Mathf.Abs(currentMousePostion.X - startPoint.X);
        Vector2 size = new Vector2(width, height);
        Vector2 topCorner = Vector2.Zero;

        selectionPanel.Size = size;

        if (selectionAreaCollisionShape.Shape is RectangleShape2D rectangleShape2D)
        {
            rectangleShape2D.Size = size;
        }

        if (currentMousePostion.X > startPoint.X)
        {
            topCorner.X = startPoint.X;
        }
        else
        {
            topCorner.X = currentMousePostion.X;
        }

        if (currentMousePostion.Y > startPoint.Y)
        {
            topCorner.Y = startPoint.Y;
        }
        else
        {
            topCorner.Y = currentMousePostion.Y;
        }


        selectionPanel.Position = topCorner;
        selectionArea.Position = topCorner + size / 2;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!underSelectionUnits.Contains(body))
        {
            underSelectionUnits.Add(body);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (underSelectionUnits.Contains(body))
        {
            underSelectionUnits.Remove(body);
        }
    }
}
