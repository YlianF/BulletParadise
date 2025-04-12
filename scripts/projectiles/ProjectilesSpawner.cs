using Godot;
using System;

public partial class ProjectilesSpawner : Projectiles
{
    [Export]
	protected PackedScene SpawnBullet;
	[Export]
	protected float BulletNumber;

	protected Node2D root;

	public void Constructor(Vector2 pos, float rot, string shooter, float dmg, float spd, PackedScene spwnbullet) {
		base.Constructor(pos, rot, shooter, dmg, spd);
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
