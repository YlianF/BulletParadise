using Godot;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    private int Speed = 300;
    [Export]
    private float Health = 100;
    [Export]
    private PackedScene Weapon1;
    [Export]
    private PackedScene Weapon2;

    private Node2D root;
    PackedScene bullet;
    private AnimationTree animationTree;
    private Timer AtkSpeed;
    private Timer RecoilTimer;

    Projectiles CurrentWeapon;

    public override void _Ready() {
        animationTree = GetNode<AnimationTree>("AnimationTree");
        root = (Node2D) GetTree().Root.GetNode("level");
        
        AtkSpeed = GetNode<Timer>("AtkSpeed");
        RecoilTimer = GetNode<Timer>("Recoil");

        EquipWeapon(Weapon1);
        
    }

    public void EquipWeapon(PackedScene Weapon) {
        bullet = Weapon;
        this.CurrentWeapon = (Projectiles) bullet.Instantiate();

        if (CurrentWeapon.ShootingType == "player") {
            this.AddChild(CurrentWeapon);
        }
    }

    public void GetInput()
    {
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Velocity = inputDir * Speed;

        HandleAnimations(inputDir);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionPressed("shoot") && AtkSpeed.IsStopped())
        {
            if (CurrentWeapon.ShootingType == "root") {
                Projectiles shot = (Projectiles) bullet.Instantiate();
                shot.Call("Constructor", GlobalPosition, Rotation, "player");
                root.AddChild(shot);
            } else {
                CurrentWeapon.Call("Shoot");
            }
            
            AtkSpeed.Start(CurrentWeapon.AtkCooldown);
            RecoilTimer.Start();
        }

        if (Input.IsActionJustReleased("change_wpn") && Weapon2 is not null) {
            AtkSpeed.Stop();
            SwitchWeapon();
        }

        if (!RecoilTimer.IsStopped()) {
            Position -= Transform.X * CurrentWeapon.Recoil * ((float) RecoilTimer.TimeLeft / (float) RecoilTimer.WaitTime) / 50;
        }

        GetInput();
        MoveAndCollide(Velocity * (float)delta);
        LookAt(GetGlobalMousePosition());

    }

    public void TakeDamage(float damage) {
        this.Health -= damage;
        if (Health <=0) {
            GameOver();
        }
    }

    public void GameOver() {
        GD.Print("Game Over !!");
    }


    public void SwitchWeapon() {
        if (bullet == Weapon1) {
            EquipWeapon(Weapon2);
        } else {
            EquipWeapon(Weapon1);
        }
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
