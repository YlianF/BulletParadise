using Godot;
using System;

public partial class WeaponStats : Resource
{
    [Export] float Damage;
    [Export] float Speed;
    [Export] int BulletNumber;
    [Export] float AtkCooldown;
    [Export] float Recoil;
}
