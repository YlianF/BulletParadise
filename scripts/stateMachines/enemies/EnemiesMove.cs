using Godot;
using System;

namespace Timtode.Entity.StateMachines;
public partial class EnemiesMove : StateMachine
{
    private Area2D _detectionZone;
    private NavigationAgent2D _navigationAgent2D;


    public override void _Ready()
    {
        base._Ready();
        
        _detectionZone = GetNode<Area2D>("FollowingZone");
        _detectionZone.BodyEntered += EntityEntered;
    }

    public override void StateEntered()
    {
        _detectionZone.Monitoring = true;
    }

    public override void StateExit()
    {
        //_detectionZone.Monitoring = false;
        SetDeferred("Monitoring",  false);

        var parentEnemy = (Enemy)_parentEntity;
        parentEnemy.Velocity = Vector2.Zero;
    }

    public void EntityEntered(Node2D node)
    {
        if(node is Player)
        {
            _parentBrain.ChangeStateMachin("EnemiesShoot");
        }
        
    }

    public override void StateProcess()
    {
        var parentEnemy = (Enemy)_parentEntity;

        parentEnemy.LookAt(parentEnemy.targetNode.GlobalPosition);
        
        float speed = parentEnemy.Speed;
        float rotation = parentEnemy.Rotation;

        Vector2 velocity = new Vector2(MathF.Cos(rotation)*parentEnemy.Speed, MathF.Sin(rotation)*parentEnemy.Speed);
        parentEnemy.Velocity = velocity;
    }
}
