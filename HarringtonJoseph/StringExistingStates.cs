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
    public void InitWith(string[] states)
    {
        m_states = states.ToArray();
    }
}


[System.Serializable]
public struct StateAndTransactionIntegerRegister
{
    public byte m_statesCount;
    public byte m_transactionsCount;
    public TransitionAsIntegerByte[] m_transitionsAsIndex;

    public void Init(byte stateCount, byte transactionCount)
    {
        m_statesCount = stateCount;
        m_transactionsCount = transactionCount;
        m_transitionsAsIndex = new TransitionAsIntegerByte[transactionCount];
    }

    public void GetSourceTransactionsOfState(byte state, out IEnumerable<byte> transactionsIds)
    {
        IEnumerable<TransitionAsIntegerByte> t = m_transitionsAsIndex.Where(k => k.m_destinationStateId  == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }

    public void GetDestinationTransactionOfState(byte state, out IEnumerable<byte> transactionsIds)
    {
        IEnumerable<TransitionAsIntegerByte> t = m_transitionsAsIndex.Where(k => k.m_sourceStateId == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }
}
[System.Serializable]
public struct TransitionAsIntegerByte
{
    public byte m_transitionId;
    public byte m_sourceStateId;
    public byte m_destinationStateId;
}


#region Pre State

    #region STRING
    [System.Serializable]
    public struct PerStateNeighborIndexCollectionAsString
    {
        public PerStateNeighborAsString[] m_listOfStateNeighbor;
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
    #endregion 

    #region Uint
    [System.Serializable]
    public struct PerStateNeighborIndexCollectionAsIndex
    {
        public PerStateNeighborAsIndex[] m_listOfStateNeighbor;
    }
    [System.Serializable]
    public struct PerStateNeighborAsIndex
    {
        public byte m_stateId;
        public byte[] m_stateDestination;
        public byte[] m_transitionDestination;
        public byte[] m_transitionSource;
        public byte[] m_stateSource;
    }
    #endregion 




#endregion


[System.Serializable]
public struct TransactionNameCollisionsCollection
{
    public string[] m_uniqueTransitions;
    public TransactionNameCollisionsToIndexes[] m_transactionsNameToIndexes;
}
[System.Serializable]
public struct TransactionNameCollisionsToIndexes {
    public string m_transactionName;
    public byte[] m_transactionIndexIds;

    public bool IsUnique() { return m_transactionIndexIds.Length == 1; }
    public bool IsUnique(out byte id) {
        bool isUnique = m_transactionIndexIds.Length == 1;
        id =(byte)( isUnique ? m_transactionIndexIds[0] : 0);
        return isUnique;

    }
    public bool HasMultiple()
    {
        return m_transactionIndexIds.Length > 1;
    }
    public bool HasMultiple(out byte[] ids) {
        ids = m_transactionIndexIds;
        return m_transactionIndexIds.Length > 1; 
    }
    public void GetAll(out byte[] ids)
    {
        ids = m_transactionIndexIds;
    }

}

#region JOBABLE



public struct StateAndTransactionIntegerRegisterJobable
{
    public byte m_statesCount;
    public byte m_transactionsCount;
    public NativeArray<TransitionAsIntegerByte> m_transitionsAsIndex;

    public void Init(byte stateCount, IEnumerable<TransitionAsIntegerByte> transcationsAsUints)
    {
        m_statesCount = stateCount;
        TransitionAsIntegerByte[] array = transcationsAsUints.ToArray();
        m_transactionsCount =(byte) array.Length;
        m_transitionsAsIndex = new NativeArray<TransitionAsIntegerByte>(array, Allocator.Persistent);
    }
    public void Dipose() {
        if(m_transitionsAsIndex.IsCreated)
            m_transitionsAsIndex.Dispose();
    }

    public void GetTransactionInfo(in byte transactionInfo, out TransitionAsIntegerByte transaction) {
        transaction = m_transitionsAsIndex[transactionInfo];
    }

    /// <summary>
    /// If Possible prefer using per state neighbor that store the neighbor transaction info
    /// </summary>
    public void GetSourceTransactionsOfState(byte state, out IEnumerable<byte> transactionsIds)
    {
        IEnumerable<TransitionAsIntegerByte> t = m_transitionsAsIndex.Where(k => k.m_destinationStateId == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }
    /// <summary>
    /// If Possible prefer using per state neighbor that store the neighbor transaction info
    /// </summary>
    public void GetDestinationTransactionOfState(byte state, out IEnumerable<byte> transactionsIds)
    {
        IEnumerable<TransitionAsIntegerByte> t = m_transitionsAsIndex.Where(k => k.m_sourceStateId == state);
        if (t.Count() > 0)
            transactionsIds = t.Select(k => k.m_transitionId);
        else transactionsIds = null;
    }
}

public struct FullNativeNeighor
{
    public byte m_stateId;
    public NativeArray<byte> m_stateDestination;
    public NativeArray<byte> m_transitionDestination;
    public NativeArray<byte> m_transitionSource;
    public NativeArray<byte> m_stateSource;

    public void Init(in PerStateNeighborAsIndex toMirror) { 
    

    }
    public void Dispose()
    {
        if (m_stateDestination.IsCreated)
            m_stateDestination.Dispose();
        if (m_transitionDestination.IsCreated)
            m_transitionDestination.Dispose();
        if (m_transitionSource.IsCreated)
            m_transitionSource.Dispose();
        if (m_stateSource.IsCreated)
            m_stateSource.Dispose();
    }
}
public struct NeighorNextJobable
{
    public byte m_stateId;
    public NativeArray<byte> m_stateDestination;
    public NativeArray<byte> m_transitionDestination;

    public void Init(in PerStateNeighborAsIndex toMirror)
    {
        m_stateDestination = new NativeArray<byte>(toMirror.m_stateDestination, Allocator.Persistent);
        m_transitionDestination = new NativeArray<byte>(toMirror.m_transitionDestination, Allocator.Persistent);

    }
    public void Dispose()
    {
        if (m_stateDestination.IsCreated)
            m_stateDestination.Dispose();
        if (m_transitionDestination.IsCreated)
            m_transitionDestination.Dispose();
    }
}




#endregion




