using Godot;
using System;

public partial class Shotgun : Projectiles
{
	[Export]
	PackedScene SpawnBullet;
	[Export]
	float BulletNumber;
	[Export]
	float range = 0.3f;

	private float StartRange;
	private float AngleBetweenBullets;

	private Node2D root;

	public override void _Ready() {
		base._Ready();
		
		//SpawnBullet = ResourceLoader.Load<PackedScene>("res://scenes/projectiles/bullet.tscn");
		root = (Node2D) GetTree().Root.GetNode("level");

		StartRange = 0 - range/2;
		AngleBetweenBullets = range/(BulletNumber -1);
	}

	public void Shoot() {
		for (int i=0; i<BulletNumber; i++) {
			Projectiles shot = (Projectiles) SpawnBullet.Instantiate();
			shot.Call("Constructor", GlobalPosition, GlobalRotation - (StartRange + AngleBetweenBullets * i), "player");
			root.AddChild(shot);
		}
	}
}
