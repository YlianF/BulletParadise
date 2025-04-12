public partial class Bullet : Projectiles
{
    public override void _PhysicsProcess(double delta)
    {
        Position += Transform.X * Speed * (float) delta;
    }
}
