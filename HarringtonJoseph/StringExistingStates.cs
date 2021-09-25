using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public struct StringExistingStates 
{
    public string[] m_states;
}


[System.Serializable]
public struct TransitionsAndExistingState
{
    public uint m_statesCount;
    public uint m_transactionsCount;
    public TransitionAsUInt[] m_transitionsAsIndex;

    public void Init(uint stateCount, uint transactionCount)
    {
        m_statesCount = stateCount;
        m_transactionsCount = transactionCount;
        m_transitionsAsIndex = new TransitionAsUInt[transactionCount];
    }

    public void GetSourceTransactionsOfState(uint state, out IEnumerable<uint> transactionsIds)
    {
        IEnumerable<TransitionAsUInt> t = m_transitionsAsIndex.Where(k => k.m_destinationStateId  == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }

    public void GetDestinationTransactionOfState(uint state, out IEnumerable<uint> transactionsIds)
    {
        IEnumerable<TransitionAsUInt> t = m_transitionsAsIndex.Where(k => k.m_sourceStateId == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }
}

[System.Serializable]
public struct JobableTransitionsAndExistingState
{
    public uint m_statesCount;
    public uint m_transactionsCount;
    public NativeArray<TransitionAsUInt> m_transitionsAsIndex;

    public void Init(uint stateCount, uint transactionCount)
    {
        m_statesCount = stateCount;
        m_transactionsCount = transactionCount;
        m_transitionsAsIndex = new NativeArray<TransitionAsUInt>((int)transactionCount, Allocator.Persistent);
    }
    public void Dispose()
    {
        m_transitionsAsIndex.Dispose();
    }
}

[System.Serializable]
public struct TransitionAsUInt {
    public uint m_transitionId;
    public uint m_sourceStateId;
    public uint m_destinationStateId;
}