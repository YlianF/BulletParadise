using Godot;
using System;
using System.Collections.Generic;

namespace Timtode.Entity.StateMachines.Enemies;
public partial class EnemiesShoot : StateMachine
{
    private Area2D _detectionZone;
    private List<Entity> potentialTarget = new List<Entity>();
    private RayCast2D rayCast;

    private Node2D root;
    PackedScene bullet;
    private Timer AtkSpeed;


    public override void _Ready()
    {
        base._Ready();
        
        var parentEnemies = (Entity)_parentEntity;

        _detectionZone = GetNode<Area2D>("DetectionZone");
        _detectionZone.BodyEntered += EntityEntered;
        _detectionZone.BodyExited += EntityExited;

        root = (Node2D) (Node2D) GetTree().Root.GetNode("level");
        bullet = ResourceLoader.Load("res://scenes/projectiles/bullet.tscn") as PackedScene;
        AtkSpeed = parentEnemies.GetNode<Timer>("AtkSpeed");
    }

    public override void StateEntered()
    {
        _detectionZone.Monitoring = true;
    }

    public override void StateExit()
    {
        _detectionZone.SetDeferred("Monitoring",  false);
    }

    public override void StateProcess()
    {
        var parentEnemy = (Enemy)_parentEntity;
        parentEnemy.LookAt(parentEnemy.targetNode.GlobalPosition);
        
        if (AtkSpeed.IsStopped())
        {
            Shoot();
            AtkSpeed.Start();
        }
    }

    public void Shoot() {
        var parentEnemy = (Enemy)_parentEntity;
        var instance = bullet.Instantiate();
		instance.Call("Constructor", parentEnemy.GlobalPosition, parentEnemy.Rotation, "ennemy");
		root.AddChild(instance);
    }

    public void EntityEntered(Node2D node)
    {
        
    }

    public void EntityExited(Node2D node)
    {
        if(node is Player)
        {
            _parentBrain.ChangeStateMachin("EnemiesMove");
        }
    }
}
