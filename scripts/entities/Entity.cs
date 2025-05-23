using Godot;
using System;
using Timtode.Entity.StateMachines;

namespace Timtode.Entity;
public abstract partial class Entity : CharacterBody2D
{
    [Export]
	public int Speed { get; set; } = 300;
	[Export]
	public float Health { get; set; } = 50;
    [Export]
    public PackedScene Weapon;

    protected StateMachinesBrain _brain;

    public override void _Ready()
	{
        _brain = GetNode<StateMachinesBrain>("Brain");
	}

    public override void _Process(double delta)
    {
        _brain.BrainProcess();
        MoveAndSlide();
    }

    public void TakeDamage(float damage) {
        this.Health -= damage;
        if (Health <=0) {
            Die();
        }
    }

    public void Die() {
        this.QueueFree();
    }

    protected virtual void Move(){}
}
