

using System;
using System.Collections.Generic;
using System.Linq;

public  class StringFSMAccess
{

    public StringFSMDeductedScriptable m_source;

    public StringFSMAccess(StringFSMDeductedScriptable source)
    {
        m_source = source;
    }

    public void GetNameOfState(in uint stateId, out string currentState)
    {
        StringFSMDeductionUtility.ConvertStateName(in stateId, out currentState, m_source);
    }

    public void GetNameOfTransaction(in uint transactionId, out string previousTransaction)
    {
        StringFSMDeductionUtility.ConvertTransitionName(in transactionId, out previousTransaction, m_source);
    }

    public void GetIdOfState(in string stateName, out uint stateId)
    {
        StringFSMDeductionUtility.ConvertStateName(in stateName, out stateId, in m_source);
    }

    public void GetIdOfTransaction(in string transactionName,out uint count, out IEnumerable<uint> transactionId)
    {
        StringFSMDeductionUtility.ConvertTransitionName(in transactionName, out count, out transactionId, in m_source);
    }

    public void GetInitState(out string stateName)
    {
        stateName = m_source.m_source.m_stringFSM.m_initialState;
    }
    public void GetInitState(out uint stateId)
    {
        GetIdOfState(in m_source.m_source.m_stringFSM.m_initialState, out stateId);
    }

   
    public StringTransaction GetTransactionInfo(in uint idIndex)
    {
       return m_source.m_source.m_stringFSM.m_transactions.m_transactions[idIndex];
    }
    public StringTransaction GetTransactionInfo(in string transactionName)
    {
        GetIdOfState(in transactionName, out uint stateId);
       return  GetTransactionInfo(stateId);
    }

    public void GetAllTransitions(out IEnumerable<StringTransaction> transactions)
    {
        transactions = m_source.m_source.m_stringFSM.m_transactions.m_transactions;
    }
    public void GetAllStates(out IEnumerable<string> transactions)
    {
        transactions = m_source.m_source.m_stringFSM.m_states.m_states;
    }

    public void GetAllTransitionsDistinctName(out IEnumerable<string> transactions)
    {
        transactions = m_source.m_transactionsCollision.m_uniqueTransitions;
    }


    public void GetStatePreviousTransactionsName(in uint stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionSource;
    }

    public void GetStateNextTransactionsName(in uint stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionDestination;
    }

    public void GetStatePreviousStateName(in uint stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_stateSource;
    }

    public void GetStateNextStateName(in uint stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_stateDestination;
    }

    public void GetStatePreviousTransactionsIndex(in uint stateId, out IEnumerable<uint> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionSource;
    }

    public void GetStateNextTransactionsIndex(in uint stateId, out IEnumerable<uint> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionDestination;
    }

    public void GetStatePreviousStateIndex(in uint stateId, out IEnumerable<uint> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_stateSource;
    }

    public void GetStateNextStateIndex(in uint stateId, out IEnumerable<uint> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_stateDestination;
    }

    public void GetStatePreviousAndNextTransactionsIndex(in uint stateId, out IEnumerable<uint> transactionsId)
    {
        GetStatePreviousTransactionsIndex(in stateId, out IEnumerable<uint> tp);
        GetStateNextTransactionsIndex(in stateId, out IEnumerable<uint> tn);
        transactionsId = Enumerable.Concat(tp, tn);
      }


    public void GetStatePreviousAndNextStateIndex(in uint stateId, out IEnumerable<uint> statesIds)
    {
        GetStatePreviousStateIndex(in stateId, out IEnumerable<uint> sp);
        GetStateNextStateIndex(in stateId, out IEnumerable<uint> sn);
        statesIds = Enumerable.Concat(sp, sn);
    }
    public void GetStatePreviousAndNextTransactionsName(in uint stateId, out IEnumerable<string> transactionsName)
    {
        GetStatePreviousTransactionsName(in stateId, out IEnumerable<string> tp);
        GetStateNextTransactionsName(in stateId, out IEnumerable<string> tn);
        transactionsName = Enumerable.Concat(tp, tn);
    }


    public void GetStatePreviousAndNextStateName(in uint stateId, out IEnumerable<string> statesName)
    {
        GetStatePreviousStateName(in stateId, out IEnumerable<string> sp);
        GetStateNextStateName(in stateId, out IEnumerable<string> sn);
        statesName = Enumerable.Concat(sp, sn);
    }

   
}