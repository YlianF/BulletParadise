using Godot;
using System;

public partial class Shotgun : ProjectilesSpawner
{
	[Export]
	float range = 0.3f;

	private float StartRange;
	private float AngleBetweenBullets;

	public override void _Ready() {
		base._Ready();

		StartRange = 0 - range/2;
		AngleBetweenBullets = range/(BulletNumber -1);
	}

	public void Constructor(Vector2 pos, float rot, string shooter, PackedScene spwnbullet, float rng) {
		base.Constructor(pos, rot, shooter, spwnbullet);
		range = rng;
    }

	public void Shoot() {
		for (int i = 0; i < BulletNumber; i++)
		{
			Projectiles shot = (Projectiles) SpawnBullet.Instantiate();
			shot.Call("Constructor", GlobalPosition, GlobalRotation - (StartRange + AngleBetweenBullets * i), Shooter);
			root.AddChild(shot);
		}
	}
}
