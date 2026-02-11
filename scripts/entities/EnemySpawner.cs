using Godot;
using System;
using Timtode.Entity;

public partial class EnemySpawner : Node2D
{
    [Export] PackedScene enemy;
    [Export] float SpawnTime;
    [Export] Godot.Collections.Array PossibleWeapons;
    [Export] Godot.Collections.Array PossibleModifiers;

    private Timer SpawnTimer;
    private int Wave = 1;
    private int EnnemyNumber = 5;
    private int currentEnnemies = 0;
    private bool isSpawning = true;

    public override void _Ready()
    {
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        
    }

    async public override void _PhysicsProcess(double delta)
    {
        if (isSpawning && currentEnnemies < EnnemyNumber && SpawnTimer.IsStopped())
        {
            SpawnEnnemy();
            SpawnTimer.Start(1);
            currentEnnemies++;
            if (currentEnnemies == EnnemyNumber)
            {
                isSpawning = false;
            }
        }

        if (!isSpawning && GetChildCount() == 1) // timer is the only child remaining
        {
            currentEnnemies = 0;
            EnnemyNumber += 5;
            isSpawning = true;
            SpawnTimer.Start(5);
        }
    }

    public void SpawnEnnemy()
    {
        Enemy NewEnemy = (Enemy)enemy.Instantiate();
        ProjectilesSpawner proj = GenerateWeapon(NewEnemy);
        NewEnemy.Call("Constructor", new Vector2((float)GD.RandRange(-500.0, 500), (float)GD.RandRange(-500, 500)), proj);
        AddChild(NewEnemy);
    }

    public ProjectilesSpawner GenerateWeapon(Enemy enemy)
    {
        var proj = (PackedScene) PossibleWeapons.PickRandom();
        var spwn = (PackedScene) PossibleModifiers.PickRandom();
        var spwn2 = (ProjectilesSpawner) spwn.Instantiate();
        spwn2.Call("Constructor", enemy.GlobalPosition, enemy.GlobalRotation, "ennemy", proj);
        return spwn2;
    }
}
