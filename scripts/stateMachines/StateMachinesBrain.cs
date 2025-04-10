using Godot;
using System.Collections.Generic;


namespace Timtode.Entity.StateMachines;
public partial class StateMachinesBrain : Node
{
    private Dictionary<string, StateMachine> stateMachines = new Dictionary<string, StateMachine>();
    [Export]
    private StateMachine _startingState;
    private StateMachine currentStateMachine;
    public Node2D parentEntity;


    // Get Children StateMachine
    public override void _Ready()
	{
        parentEntity = GetParent<Node2D>();

        foreach(var chidren in GetChildren())
        {
            if(chidren is StateMachine stateMachine)
            {
                stateMachines.Add(stateMachine.Name, stateMachine);
            }
            else
            {
                GD.PushWarning($"In {Name}, child {chidren.Name} is not a {nameof(StateMachine)}");
            }
        }


        if(stateMachines.Count == 0)
        {
            GD.PushError($"{Name} don't have a {nameof(StateMachine)} child. He doesn't work");
        }
        else if(_startingState is null)
        {
            GD.PushError($"{Name} don't have a {nameof(_startingState)} filde fill. He doesn't work");
        }
        else
        {
            ChangeStateMachin(_startingState);
        }
	}

	
	public void BrainProcess()
	{
        currentStateMachine?.StateProcess();
	}


    public void ChangeStateMachin(string stateName)
    {
        try
        {
            ChangeStateMachin(stateMachines[stateName]);
        }
        catch(KeyNotFoundException)
        {
            GD.PushError($"{Name} don't have state {stateName}");
        }
    }

    public void ChangeStateMachin(StateMachine newStateMachine)
    {
        currentStateMachine?.StateExit();

        newStateMachine.StateEntered();
        currentStateMachine = newStateMachine;
    }
}