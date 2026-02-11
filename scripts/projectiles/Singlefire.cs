using Godot;
using System;

public partial class Singlefire : ProjectilesSpawner
{
    public override void _Ready() {
		base._Ready();
	}

	public void Constructor(Vector2 pos, float rot, string shooter, float dmg, float spd, PackedScene spwnbullet) {
		base.Constructor(pos, rot, shooter, spwnbullet);
	}

	public void Shoot() {
		var CurrentRot = GlobalRotation;

		Projectiles shot = (Projectiles) SpawnBullet.Instantiate();
		shot.Call("Constructor", GlobalPosition, CurrentRot, Shooter);
		root.AddChild(shot);
	}
}