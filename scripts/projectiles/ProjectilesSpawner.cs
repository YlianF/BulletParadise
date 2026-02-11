using Godot;
using System;

public partial class ProjectilesSpawner : Area2D
{
	[Export]
    protected float Damage = 25;
    [Export]
    protected float Speed = 300f;
    [Export]
	protected PackedScene SpawnBullet;
	[Export]
	protected float BulletNumber;
	[Export]
    public float AtkCooldown { get; set; } = 1.0f;
    [Export]
    public float Recoil = 0f;

	protected Node2D root;
	protected string Shooter;

	public void Constructor(Vector2 pos, float rot, string shooter, PackedScene spwnbullet)
	{
		GlobalPosition = pos;
		GlobalRotation = rot;
		Shooter = shooter;
		SpawnBullet = spwnbullet;
	}

	public override void _Ready() {
		base._Ready();
		root = (Node2D) GetTree().Root.GetNode("level");

        if (SpawnBullet is null) {
            SpawnBullet = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/bullet.tscn");
        }
	}
}
