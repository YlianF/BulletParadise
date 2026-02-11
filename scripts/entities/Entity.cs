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
    public PackedScene WeaponToSet;

    public ProjectilesSpawner Weapon;

    public Timer RecoilTimer;
    protected StateMachinesBrain _brain;

    public override void _Ready()
    {
        RecoilTimer = GetNode<Timer>("Recoil");
        _brain = GetNode<StateMachinesBrain>("Brain");
	}

    public override void _PhysicsProcess(double delta)
    {
        _brain.BrainProcess();
        MoveAndSlide();

        if (!RecoilTimer.IsStopped()) {
            Position -= Transform.X * Weapon.Recoil * ((float) RecoilTimer.TimeLeft / (float) RecoilTimer.WaitTime) / 50;
        }
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
