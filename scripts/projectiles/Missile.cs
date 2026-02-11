using Godot;

public partial class Missile : Projectiles
{
    Player target;
    Timer AutoTrack;

    public override void _Ready()
    {
        base._Ready();
        target = (Player)GetTree().Root.GetNode("level").GetNode("Player");
        AutoTrack = GetNode<Timer>("AutoTrack");
        AutoTrack.Start(1);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!AutoTrack.IsStopped())
        {
            Position += Transform.X * Speed * (float)delta;
        }
        else
        {
            
            Position += Transform.X * Speed * 2 * (float)delta;
        }
    }

    public void _on_auto_track_timeout()
    {
        LookAt(target.GlobalPosition);
    }
}
