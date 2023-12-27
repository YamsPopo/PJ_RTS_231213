using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IStateMachine
{
    object GetOwner();
    void SetState(int stateName);
}
public abstract class State
{
    protected IStateMachine sm;
    // owner�� ��ӹ޴� �ڽĿ��� �����������
    public virtual void Init(IStateMachine sm)
    {
        this.sm = sm;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public class StateMachine<T> : IStateMachine where T : class
{
    public Dictionary<int, State> stateDic;

    public State curState;
    private T owner;
    public int stateEnumInt;

    public StateMachine(T owner)
    {
        stateDic = new Dictionary<int, State>();
        this.owner = owner;
    }

    public object GetOwner()
    {
        return owner;
    }

    public void SetState(int stateName)
    {
        if (!stateDic.ContainsKey(stateName))
            return;

        if (curState != null)
            curState.Exit();

        curState = stateDic[stateName];
        curState.Enter();
        //���� ������Ʈ int�� ��ȯ
        stateEnumInt = stateDic.FirstOrDefault(x => x.Value == curState).Key;
    }

    public void AddState(int name, State state)
    {
        if (stateDic.ContainsKey(name))
            return;

        stateDic.Add(name, state);
        state.Init(this);
    }

    public void UpdateState()
    {
        curState.Update();
    }
}