using Godot;
using System;

public partial class Laser : Projectiles
{
    RayCast2D laser;
    Timer CastTime;
    Timer Startup;
    CollisionShape2D Collision;

    public override void _Ready() {
        laser = GetNode<RayCast2D>("LaserBeam2D");
        CastTime = GetNode<Timer>("CastTime");
        Startup = GetNode<Timer>("Startup");
        Collision = GetNode<CollisionShape2D>("CollisionShape2D");
        Shooter = "player";
    }

    public void Shoot() {
        laser.Call("Shoot");
        CastTime.Start();
        Startup.Start();
    }

    public void _on_startup_timeout() {
        Collision.Disabled = false;
    }

    public void _on_cast_t_ime_timeout() {
        StopShoot();
    }

    public void StopShoot() {
        laser.Call("StopShoot");
        Collision.Disabled = true;
    }
}
