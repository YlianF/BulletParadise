using Godot;
using System;

namespace Timtode.Entity.StateMachines;
public abstract partial class StateMachine : Node
{
    protected StateMachinesBrain _parentBrain;
    protected Node2D _parentEntity;

    public override void _Ready()
    {
        try
        {
            _parentBrain = GetParent<StateMachinesBrain>();
        }
        catch(Exception)
        {
            GD.PushWarning($"{Name} is not a child of type {nameof(StateMachinesBrain)}");
        }
        
        _parentEntity = GetParent().GetParent<Node2D>();
    }


    public virtual void StateEntered(){}
    public virtual void StateProcess(){}
    public virtual void StateExit(){}
}
