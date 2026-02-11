using Godot;
using System;


public partial class Player : CharacterBody2D
{
    [Export]
    private int Speed = 300;
    [Export]
    private float Health = 100;

    private ProjectilesSpawner Weapon1;
    [Export]
    private PackedScene Bullet1;
    [Export]
    private PackedScene WeaponSpwn1;
    private ProjectilesSpawner Weapon2;
    [Export]
    private PackedScene Bullet2;
    [Export]
    private PackedScene WeaponSpwn2;

    private Node2D root;
    PackedScene bullet;
    private AnimationTree animationTree;
    private Timer AtkSpeed;
    private Timer RecoilTimer;
    private Camera2D Camera;

    ProjectilesSpawner CurrentWeapon;

    public override void _Ready() {
        animationTree = GetNode<AnimationTree>("AnimationTree");
        root = (Node2D) GetTree().Root.GetNode("level");
        
        AtkSpeed = GetNode<Timer>("AtkSpeed");
        RecoilTimer = GetNode<Timer>("Recoil");
        Camera = GetNode<Camera2D>("Camera2D");


        Weapon1 = (ProjectilesSpawner) WeaponSpwn1.Instantiate();
        Weapon1.Call("Constructor", GlobalPosition, GlobalRotation, "player", Bullet1);
        this.AddChild(Weapon1);
        // Weapon2 = (ProjectilesSpawner) WeaponSpwn2.Instantiate();
        // Weapon2.Call("Constructor", GlobalPosition, GlobalRotation, "player", 25, 20, Bullet2);
        Weapon2 = (ProjectilesSpawner) WeaponSpwn2.Instantiate();
        this.AddChild(Weapon2);
        EquipWeapon(Weapon1);
    }

    public void EquipWeapon(ProjectilesSpawner Weapon)
    {
        CurrentWeapon = Weapon;
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
            CurrentWeapon.Call("Shoot");

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
        Camera.Call("apply_shake");
        this.Health -= damage;
        if (Health <=0) {
            GameOver();
        }
    }

    public void GameOver() {
        GD.Print("Game Over !!");
    }


    public void SwitchWeapon() {
        if (CurrentWeapon == Weapon1) {
            EquipWeapon(Weapon2);
        } else {
            EquipWeapon(Weapon1);
        }
    }






    public void HandleAnimations(Vector2 input) {
        float AxisUsed = 0;

        if (GlobalRotation >= -Mathf.Pi/4 && GlobalRotation <= Mathf.Pi/4) { // look to right
            AxisUsed = input.Y;
        } else if (GlobalRotation <= -3*Mathf.Pi/4 || GlobalRotation >= 3*Mathf.Pi/4) { // look to left
            AxisUsed = -input.Y;
        } else if (GlobalRotation <= -Mathf.Pi/4 && GlobalRotation >= -3*Mathf.Pi/4) { // look up
            AxisUsed = input.X;
        } else if (GlobalRotation >= Mathf.Pi/4 && GlobalRotation <= 3*Mathf.Pi/4) { // look down
            AxisUsed = -input.X;
        }

        if (AxisUsed == 0f) {
            animationTree.Set("parameters/conditions/idle", true);
            animationTree.Set("parameters/conditions/left_turn", false);
            animationTree.Set("parameters/conditions/right_turn", false);
        } else if (AxisUsed > 0) {
            animationTree.Set("parameters/conditions/right_turn", true);
            animationTree.Set("parameters/conditions/left_turn", false);
            animationTree.Set("parameters/conditions/idle", false);
        } else if (AxisUsed < 0) {
            animationTree.Set("parameters/conditions/left_turn", true);
            animationTree.Set("parameters/conditions/right_turn", false);
            animationTree.Set("parameters/conditions/idle", false);
        }
    }
}
