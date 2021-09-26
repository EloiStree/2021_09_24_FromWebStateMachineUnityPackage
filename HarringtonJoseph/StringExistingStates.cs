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
public struct TransitionsAndExistingStateUIntRegister
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
public struct TransitionAsUInt
{
    public uint m_transitionId;
    public uint m_sourceStateId;
    public uint m_destinationStateId;
}



[System.Serializable]
public struct PerStateNeighborIndexCollectionAsIndex
{
    public PerStateNeighborAsIndex[] m_listOfStateNeighbor;
}

[System.Serializable]
public struct PerStateNeighborIndexCollectionAsString
{
    public PerStateNeighborAsString[] m_listOfStateNeighbor;
}
[System.Serializable]
public struct PerStateNeighborAsIndex
{
    public uint m_stateId;
    public uint[] m_stateDestination;
    public uint[] m_transitionDestination;
    public uint[] m_transitionSource;
    public uint[] m_stateSource;
}
[System.Serializable]
public struct PerStateNeighborAsString
{
    public string m_stateId;
    public string[] m_stateDestination;
    public string[] m_transitionDestination;
    public string[] m_transitionSource;
    public string[] m_stateSource;
}
[System.Serializable]
public struct TransactionNameCollisionsCollection
{
    public string[] m_uniqueTransitions;
    public TransactionNameCollisionsToIndexes[] m_transactionsNameToIndexes;
}
[System.Serializable]
public struct TransactionNameCollisionsToIndexes {
    public string m_transactionName;
    public uint[] m_transactionIndexIds;

    public bool IsUnique() { return m_transactionIndexIds.Length == 1; }
    public bool IsUnique(out uint id) {
        bool isUnique = m_transactionIndexIds.Length == 1;
        id = isUnique ? m_transactionIndexIds[0] : 0;
        return isUnique;

    }
    public bool HasMultiple()
    {
        return m_transactionIndexIds.Length > 1;
    }
    public bool HasMultiple(out uint [] ids) {
        ids = m_transactionIndexIds;
        return m_transactionIndexIds.Length > 1; 
    }
    public void GetAll(out uint[] ids)
    {
        ids = m_transactionIndexIds;
    }

}


//[System.Serializable]
//public struct JobableTransitionsAndExistingState
//{
//    public uint m_statesCount;
//    public uint m_transactionsCount;
//    public NativeArray<TransitionAsUInt> m_transitionsAsIndex;

//    public void Init(uint stateCount, uint transactionCount)
//    {
//        m_statesCount = stateCount;
//        m_transactionsCount = transactionCount;
//        m_transitionsAsIndex = new NativeArray<TransitionAsUInt>((int)transactionCount, Allocator.Persistent);
//    }
//    public void Dispose()
//    {
//        m_transitionsAsIndex.Dispose();
//    }
//}

