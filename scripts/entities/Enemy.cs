using Godot;
using System;

namespace Timtode.Entity;
public partial class Enemy : Entity
{
	public Node2D targetNode;
	public Polygon2D skin;
	
	public override void _Ready()
	{
        base._Ready();
		targetNode = (Node2D) GetTree().Root.GetNode("level").GetNode("Player");
	}
}
