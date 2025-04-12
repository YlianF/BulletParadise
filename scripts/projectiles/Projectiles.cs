using Godot;
using System;
using System.Collections;
using Timtode.Entity;

public partial class Projectiles : Area2D
{
    [Export]
    protected float damage = 25;
    [Export]
    protected float Speed = 300f;
    [Export]
    public float AtkCooldown { get; set; } = 1.0f;
    [Export]
    public float Recoil = 0f;
    [Export]
    public string ShootingType = "root";

    protected Vector2 direction;
    protected Timer Life;
	protected float LifeTime;
    protected string Shooter;

    public override void _Ready() {
        Life = GetNode<Timer>("Life");
        Life.Start(LifeTime);
    }

    public void Constructor(Vector2 pos, float rot, string shooter, float dmg, float spd) {
        this.GlobalPosition = pos;
        GlobalRotation = rot;
        Shooter = shooter;
        damage = dmg;
        Speed = spd;
    }

    public void Constructor(Vector2 pos, float rot, string shooter) {
        this.GlobalPosition = pos;
        GlobalRotation = rot;
        Shooter = shooter;
    }

    public void _on_deathtimer_timeout () {
        QueueFree();
    }

    public void _on_body_entered (Node2D body) {
        if (body.HasMethod("TakeDamage")) {
            if ((body is Player && Shooter == "ennemy") || (body is Enemy && Shooter == "player")) {
                body.Call("TakeDamage", damage);
                //Queuefree
            }
        }
        
    }
}
