using Godot;
using System;
using System.Collections;
using Timtode.Entity;

public partial class Projectiles : Area2D
{
    [Export]
    protected float Damage = 25;
    [Export]
    protected float Speed = 300f;
    [Export]
    public string ShootingType = "root";

    protected Vector2 direction; // TODO SERT A RIEN ?
    protected Timer Life;
	protected float LifeTime;
    protected string Shooter;
    

    public override void _Ready()
    {
        Life = GetNode<Timer>("Life");
        Life.Start(LifeTime);
    }

    // public void Constructor(float dmg, float spd, string shooter)
    // {
    //     damage = dmg;
    //     Speed = spd;
    //     Shooter = shooter;
    // }

    public void Constructor(Vector2 pos, float rot, string shooter)
    {
        GlobalPosition = pos;
		GlobalRotation = rot;
        Shooter = shooter;
    }


    public void _on_deathtimer_timeout()
    {
        QueueFree();
    }

    public void _on_body_entered (Node2D body) {
        if (body.HasMethod("TakeDamage")) {
            if ((body is Player && Shooter == "ennemy") || (body is Enemy && Shooter == "player")) {
                body.Call("TakeDamage", Damage);
                // TODO QueueFree();
            }
        }
    }
}
