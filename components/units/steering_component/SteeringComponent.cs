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

    [Export(PropertyHint.Range, "0,128,1,or_greater")]
    private int _separationDistance = 96;

    [Export(PropertyHint.Range, "0,128,1,or_greater")]
    private int _cohesionDistance = 256;

    [Export(PropertyHint.Range, "0,1024,8,or_greater")]
    private int _leaderBehindDistance = 32;

    [Export(PropertyHint.Range, "0,1024,8,or_greater")]
    private int _leaderSightRadius = 24;

    [Export]
    public UnitGroupComponent UnitGroupComponent = null;

    public bool Moving = false;
    private Vector2 _targetPosition = Vector2.Zero;

    private CharacterBody2D _targetUnit = null;


    public override void _Ready()
    {
        _targetUnit = GetNode<CharacterBody2D>("../");
        _targetPosition = _targetUnit.Position;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        Vector2 steering = Vector2.Zero;

        // Leader
        if (UnitGroupComponent == null || UnitGroupComponent.Leader == _targetUnit)
        {
            if (Moving)
            {
                if (_targetUnit.Position.DistanceTo(_targetPosition) < 8)
                {
                    Moving = false;
                    _targetPosition = Position;
                }
                else
                {
                    steering += Seek(_targetPosition, _arrivalDistance) * 0.8f;

                }
            }
        }
        // Rest of the group
        else
        {
            Vector2 separation = Separation();
            if (Moving)
            {
                Vector2 seek = Seek(_targetPosition, _arrivalDistance) * 0.8f;
                if (!UnitGroupComponent.Leader.IsMoving() && separation.Length() > seek.Length())
                {
                    Moving = false;
                    _targetPosition = Position;
                }
                else
                {
                    steering += seek;

                }
            }

            steering += separation;
        }

        // steering += Separation();
        _targetUnit.Velocity = steering.Normalized() * _speed;
        _targetUnit.Position += _targetUnit.Velocity * (float)delta;
        // _targetUnit.MoveAndSlide();
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

        return force.Normalized();
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

        return force.Normalized();
    }

    Vector2 Evade(Unit target)
    {
        float distance = _targetUnit.Position.DistanceTo(target.Position);
        float updatesNeeded = distance / _speed;
        Vector2 tv = target.Velocity * updatesNeeded;
        Vector2 targetFuturePosition = _targetUnit.Position + tv;

        return Flee(targetFuturePosition);

    }

    Vector2 Separation()
    {
        Vector2 force = Vector2.Zero;
        int neighborCount = 0;

        if (UnitGroupComponent == null)
        {
            return Vector2.Zero;
        }

        foreach (Unit unit in UnitGroupComponent.Units)
        {
            if (unit != _targetUnit && _targetUnit.Position.DistanceTo(unit.Position) <= _separationDistance)
            {
                force += unit.Position.DirectionTo(_targetUnit.Position);
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            force /= neighborCount;
        }


        return force.Normalized();
    }

    Vector2 Cohesion()
    {
        Vector2 middlePoint = Vector2.Zero;
        Vector2 force = Vector2.Zero;
        int neighborCount = 0;

        if (UnitGroupComponent == null)
        {
            return Vector2.Zero;
        }

        foreach (Unit unit in UnitGroupComponent.Units)
        {
            if (unit != _targetUnit && _targetUnit.Position.DistanceTo(unit.Position) <= _cohesionDistance)
            {
                middlePoint += unit.Position;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            middlePoint /= neighborCount;
            force = _targetUnit.Position.DirectionTo(middlePoint);
        }

        return force;
    }


    Vector2 FollowLeader(Unit leader)
    {
        Vector2 tv = leader.Velocity;
        Vector2 force = Vector2.Zero;
        Vector2 ahead;
        Vector2 behind;

        tv = tv.Normalized();
        tv *= _leaderBehindDistance;

        ahead = leader.Position + tv;

        tv *= -1;

        behind = leader.Position + tv;

        if (IsOnLeaderSight(leader, ahead))
        {
            force += Evade(leader);
            force *= 1.8f;
        }

        force += Arrive(behind, _arrivalDistance / 4);

        return force.Normalized();
    }

    bool IsOnLeaderSight(CharacterBody2D leader, Vector2 leaderAhead)
    {
        return leaderAhead.DistanceTo(_targetUnit.Position) <= _leaderSightRadius || leader.Position.DistanceTo(_targetUnit.Position) <= _leaderSightRadius;
    }

    public void Move(Vector2 position)
    {
        Moving = true;
        _targetPosition = position;
    }

}
