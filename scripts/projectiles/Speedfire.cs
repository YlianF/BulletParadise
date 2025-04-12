using Godot;
using System;

public partial class Speedfire : ProjectilesSpawner
{
    [Export]
    float TimeBetweenShots;

    public override void _Ready() {
		base._Ready();
	}

	public void Constructor(Vector2 pos, float rot, string shooter, float dmg, float spd, PackedScene spwnbullet, float timeshots) {
		base.Constructor(pos, rot, shooter, dmg, spd, spwnbullet);
		TimeBetweenShots = timeshots;
    }

	async public void Shoot() {
        var CurrentRot = GlobalRotation;
		for (int i=0; i<BulletNumber; i++) {
			Projectiles shot = (Projectiles) SpawnBullet.Instantiate();
			shot.Call("Constructor", GlobalPosition, CurrentRot, "player");
			root.AddChild(shot);

            await ToSignal(GetTree().CreateTimer(TimeBetweenShots), "timeout");
		}
	}
}
