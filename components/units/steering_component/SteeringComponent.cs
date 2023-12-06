using Godot;
using System;

public partial class SteeringComponent : Node2D
{
    [Export(PropertyHint.Range, "0,1000,10,or_greater")]
    private int _maxForce = 100;

    [Export(PropertyHint.Range, "0,1000,10,or_greater")]
    private int _speed = 300;

    [Export(PropertyHint.Range, "0,128,1,or_greater")]
    private int _arrivalDistance = 16;

    [Export(PropertyHint.Range, "0,1024,8,or_greater")]
    private int _leaderBehindDistance = 32;

    [Export(PropertyHint.Range, "0,1024,8,or_greater")]
    private int _leaderSightRadius = 24;

    [Export]
    public UnitGroupComponent UnitGroupComponent = null;

    [Export]
    public Vector2 TargetPostion = Vector2.Zero;

    private CharacterBody2D _targetUnit = null;


    public override void _Ready()
    {
        _targetUnit = GetNode<CharacterBody2D>("../");
        TargetPostion = _targetUnit.Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        Vector2 steering = Vector2.Zero;

        if (UnitGroupComponent == null || UnitGroupComponent.Leader != _targetUnit)
        {
            steering += Seek(TargetPostion, _arrivalDistance);
        }
        else
        {
            steering += FollowLeader(UnitGroupComponent.Leader);
        }
        _targetUnit.Velocity = steering * _speed;
        _targetUnit.MoveAndSlide();
    }

    Vector2 Seek(Vector2 target, int slowingRadius = 1)
    {
        Vector2 force;
        Vector2 desired;
        float distance;

        desired = _targetUnit.Position.DirectionTo(target);
        distance = _targetUnit.Position.DistanceTo(target);

        if (distance <= slowingRadius)
        {
            desired *= distance / slowingRadius;
        }

        force = desired;

        return force;
    }

    Vector2 Arrive(Vector2 target, int slowingRadius = 16)
    {
        return Seek(target, slowingRadius);
    }

    Vector2 Flee(Vector2 target)
    {
        Vector2 force;
        Vector2 desired;

        desired = target.DirectionTo(_targetUnit.Position);

        force = desired - _targetUnit.Velocity;

        return force;
    }

    Vector2 Evade(Vector2 target)
    {
        float distance;
        float updatesNeeded;
        Vector2 tv;
        Vector2 targetFuturePosition = Vector2.Zero;

        // distance = _targetUnit.Position.DistanceTo(target);
        // updatesNeeded = distance / _maxSpeed;
        // tv = _targetUnit.Velocity / updatesNeeded;

        // targetFuturePosition = _targetUnit.Position + tv;

        return Flee(targetFuturePosition);

    }

    Vector2 Separation()
    {
        Vector2 force = Vector2.Zero;
        return force;
    }

    Vector2 FollowLeader(CharacterBody2D leader)
    {
        Vector2 tv = leader.Velocity;
        Vector2 force = Vector2.Zero;
        Vector2 ahead;
        Vector2 behind;

        tv = tv.Normalized();
        tv /= _leaderBehindDistance;

        ahead = leader.Position + tv;

        tv /= -1;

        behind = leader.Position + tv;

        if (IsOnLeaderSight(leader, ahead))
        {
            force += Evade(leader.Position);
            force /= 1.8f;
        }

        force += Arrive(behind, _arrivalDistance / 4);

        return force;
    }

    bool IsOnLeaderSight(CharacterBody2D leader, Vector2 leaderAhead)
    {
        return leaderAhead.DistanceTo(_targetUnit.Position) <= _leaderSightRadius || leader.Position.DistanceTo(_targetUnit.Position) <= _leaderSightRadius;
    }

}
