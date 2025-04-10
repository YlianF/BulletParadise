using Godot;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    private int Speed = 300;
    [Export]
    private int Health = 100;

    private Node2D root;
    PackedScene bullet;
    private AnimationTree animationTree;
    private Timer AtkSpeed;

    public override void _Ready() {
        animationTree = GetNode<AnimationTree>("AnimationTree");
        root = (Node2D) GetParent();
        bullet = ResourceLoader.Load("res://scenes/projectiles/bullet.tscn") as PackedScene;
        AtkSpeed = GetNode<Timer>("AtkSpeed");
    }

    public void GetInput()
    {
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDir * Speed;

        HandleAnimations(inputDir);
    }

    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndCollide(Velocity * (float)delta);

        LookAt(GetGlobalMousePosition());

        if (Input.IsActionPressed("shoot") && AtkSpeed.IsStopped())
        {
            Shoot();
            AtkSpeed.Start();
        }
    }

    public void Shoot() {
        var instance = bullet.Instantiate();
		instance.Call("Constructor", GlobalPosition, Rotation, "player");
		root.AddChild(instance);
    }

    public void TakeDamage(int damage) {
        this.Health -= damage;
        if (Health <=0) {
            GameOver();
        }
    }

    public void GameOver() {
        GD.Print("Game Over !!");
    }






    public void HandleAnimations(Vector2 input) {
        int CurrentLeft = 0;
        int CurrentRight = 0;
        float AxisUsed = 0;

        if (GlobalRotation <= -0.75f && GlobalRotation >= -2.25f) {
            AxisUsed = input.X;
            CurrentLeft = -1;
            CurrentRight = 1;
        }
        if (GlobalRotation >= 0.75f && GlobalRotation <= 2.25f) {
            AxisUsed = input.X;
            CurrentLeft = 1;
            CurrentRight = -1;
        }
        if (GlobalRotation >= 2.5f || GlobalRotation <= -2.25f) {
            AxisUsed = input.Y;
            CurrentLeft = 1;
            CurrentRight = -1;
        }
        if (GlobalRotation <= 0.75f && GlobalRotation >= -0.75f) {
            AxisUsed = input.Y;
            CurrentLeft = -1;
            CurrentRight = 1;
        }

        if (AxisUsed == 0f) {
            animationTree.Set("parameters/conditions/idle", true);
            animationTree.Set("parameters/conditions/left_turn", false);
            animationTree.Set("parameters/conditions/right_turn", false);
        } else if (AxisUsed == CurrentLeft) {
            animationTree.Set("parameters/conditions/left_turn", true);
            animationTree.Set("parameters/conditions/right_turn", false);
            animationTree.Set("parameters/conditions/idle", false);
        } else if (AxisUsed == CurrentRight) {
            animationTree.Set("parameters/conditions/right_turn", true);
            animationTree.Set("parameters/conditions/left_turn", false);
            animationTree.Set("parameters/conditions/idle", false);
        }
    }
}
