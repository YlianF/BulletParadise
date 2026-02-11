using Godot;
using System;

public partial class Camera : Camera2D
{
    [Export] private Vector2 DesiredOffset;
    [Export] private int MinOffset = -500;
    [Export] private int MaxOffset = 500;
    [Export] int randomStrength = 200;
	[Export] float shakeFade = 5;

	int shake_strength = 0;

    private CharacterBody2D Player;

    public override void _Ready()
    {
        Player = (CharacterBody2D)GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (shake_strength > 0) {
			shake_strength = (int) Mathf.Lerp(shake_strength, 0.0, shakeFade * delta);
			Offset = randomOffset();
		}

        DesiredOffset = (GetGlobalMousePosition() - GlobalPosition) * 0.5f;
        DesiredOffset.X = Mathf.Clamp(DesiredOffset.X, MinOffset, MaxOffset);
        DesiredOffset.Y = Mathf.Clamp(DesiredOffset.Y, MinOffset / 2.0f, MaxOffset / 2.0f);

        GlobalPosition = Player.GlobalPosition + DesiredOffset;
    }

	public void apply_shake()
	{
		shake_strength = randomStrength;
	}

	public Vector2 randomOffset() {
		Random rng = new Random();
		return new Vector2(rng.Next(-shake_strength, shake_strength) / 10, rng.Next(-shake_strength, shake_strength) / 10);
	}
}
