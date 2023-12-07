using Godot;
using System;

public partial class CameraMovementComponent : Node
{
    [Export]
    private Camera2D _camera = null;
    [Export(PropertyHint.Range, "0,250,5,or_greater")]
    private int _speed = 50;


    [Export]
    private float _minZoom = 0.5f;

    [Export]
    private float _maxZoom = 2.0f;
    [Export]
    private float _zoomFactor = 0.1f;

    [Export]
    private float _zoomDuration = 0.2f;


    public override void _PhysicsProcess(double delta)
    {
        Vector2 inputDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        _camera.Position += inputDirection * _speed;
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("zoom_in"))
        {
            SetZoomLevel(_camera.Zoom.X + _zoomFactor);
        }
        else if (Input.IsActionJustPressed("zoom_out"))
        {
            SetZoomLevel(_camera.Zoom.X - _zoomFactor);
        }
    }

    void SetZoomLevel(float value)
    {
        float zoomLevel = Mathf.Clamp(value, _minZoom, _maxZoom);
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_camera, "zoom", new Vector2(zoomLevel, zoomLevel), _zoomDuration);
    }
}
