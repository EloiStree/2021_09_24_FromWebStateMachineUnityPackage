

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

    public void GetNameOfState(in byte stateId, out string currentState)
    {
        StringFSMDeductionUtility.ConvertStateName(in stateId, out currentState, m_source);
    }

    public void GetNameOfTransaction(in byte transactionId, out string previousTransaction)
    {
        StringFSMDeductionUtility.ConvertTransitionName(in transactionId, out previousTransaction, m_source);
    }

    public void GetInitStateId(out byte initState)
    {
        GetInitState(out string initStateName);
        GetIdOfState(in initStateName, out initState);
    }

    public void GetIdOfState(in string stateName, out byte stateId)
    {
        StringFSMDeductionUtility.ConvertStateName(in stateName, out stateId, in m_source);
    }

    public void GetIdOfTransaction(in string transactionName,out byte count, out IEnumerable<byte> transactionId)
    {
        StringFSMDeductionUtility.ConvertTransitionName(in transactionName, out count, out transactionId, in m_source);
    }

  
    public void HasTransactionBetweenStates(in byte fromStateIndex, in byte toStateIndex, out bool found, out byte transactionId)
    {
        for (byte i = 0; i < m_source.m_uintIndexes.m_transactionsCount; i++)
        {
            if (
                m_source.m_uintIndexes.m_transitionsAsIndex[i].m_sourceStateId == fromStateIndex &&
            m_source.m_uintIndexes.m_transitionsAsIndex[i].m_destinationStateId == toStateIndex) {
                found = true;
                transactionId = i;
                return;
            }

        }
        found = false;
        transactionId = 0;
    }


   

    public void GetInitState(out string stateName)
    {
        stateName = m_source.m_source.m_stringFSM.m_initialState;
    }

    

    public void GetInitState(out byte stateId)
    {
        GetIdOfState(in m_source.m_source.m_stringFSM.m_initialState, out stateId);
    }

    public void HasNextStateForTransition(in byte currentStateId, in string nextTransitionName, out bool found, out byte nextState)
    {
        
        GetStateNextTransactionsIndex(currentStateId, out IEnumerable<byte> transactionsId);

        string tName = "";
        foreach (byte tId in transactionsId)
        {
            GetNameOfTransaction(in tId, out tName);
            if(tName.Length== nextTransitionName.Length 
                && tName.IndexOf(nextTransitionName,StringComparison.OrdinalIgnoreCase) == 0 )
            {
                nextState = m_source.m_uintIndexes.m_transitionsAsIndex[tId].m_destinationStateId;
                found = true;
                return;
            }

        }
        found = false;
        nextState = 0;
    }

    public void GetRandomNextTransactionIdFromTransaction(in byte transactionId, out byte nextTranscationId)
    {
        byte destinationStateId = m_source.m_uintIndexes.m_transitionsAsIndex[transactionId].m_destinationStateId;
        GetStateNextTransactionsIndex(in destinationStateId, out IEnumerable<byte> tDestinations);
        nextTranscationId = RandomElement<byte>(tDestinations);
    }
  
    public void GetStatesOfTransactionId(in byte transactionId, out byte sourceStateId, out byte destinationStateId)
    {
        sourceStateId =  m_source.m_uintIndexes.m_transitionsAsIndex[transactionId].m_sourceStateId;
        destinationStateId = m_source.m_uintIndexes.m_transitionsAsIndex[transactionId].m_destinationStateId;
    }

    public StringTransaction GetTransactionInfo(in byte idIndex)
    {
       return m_source.m_source.m_stringFSM.m_transactions.m_transactions[idIndex];
    }
    public StringTransaction GetTransactionInfo(in string transactionName)
    {
        GetIdOfState(in transactionName, out byte stateId);
       return  GetTransactionInfo(stateId);
    }

    public void GetAllTransitions(out IEnumerable<StringTransaction> transactions)
    {
        transactions = m_source.m_source.m_stringFSM.m_transactions.m_transactions;
    }

    public void GetTransactionIdFromStatesId(in byte previousStateId, in byte newStateId, out byte transactionId)
    {
        for (byte i = 0; i < m_source.m_uintIndexes.m_transactionsCount; i++)
        {
            if (m_source.m_uintIndexes.m_transitionsAsIndex[i].m_destinationStateId
                == newStateId &&
                m_source.m_uintIndexes.m_transitionsAsIndex[i].m_sourceStateId
                == previousStateId) {
                 transactionId = i;
                return;
            }
        }
        throw new Exception("You gave states that don't have transaction:" + previousStateId + " " + newStateId);
    }

    public void GetAllStates(out IEnumerable<string> transactions)
    {
        transactions = m_source.m_source.m_stringFSM.m_states.m_states;
    }

    public void GetAllTransitionsDistinctName(out IEnumerable<string> transactions)
    {
        transactions = m_source.m_transactionsCollision.m_uniqueTransitions;
    }


    public void GetStatePreviousTransactionsName(in byte stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionSource;
    }

    public void GetStateNextTransactionsName(in byte stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionDestination;
    }

    public void GetStatePreviousStateName(in byte stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_stateSource;
    }

    public void GetStateNextStateName(in byte stateId, out IEnumerable<string> transactionsName)
    {
        transactionsName = m_source.m_stringIdNeighborPerState.m_listOfStateNeighbor[stateId].m_stateDestination;
    }

    public void GetStatePreviousTransactionsIndex(in byte stateId, out IEnumerable<byte> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionSource;
    }

    public void GetStateNextTransactionsIndex(in byte stateId, out IEnumerable<byte> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_transitionDestination;
    }

    public void GetStatePreviousStateIndex(in byte stateId, out IEnumerable<byte> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_stateSource;
    }

    public void GetStateNextStatesIndex(in byte stateId, out IEnumerable<byte> transactionsName)
    {
        transactionsName = m_source.m_uintIndexNeighborPerState.m_listOfStateNeighbor[stateId].m_stateDestination;
    }

    public void GetStatePreviousAndNextTransactionsIndex(in byte stateId, out IEnumerable<byte> transactionsId)
    {
        GetStatePreviousTransactionsIndex(in stateId, out IEnumerable<byte> tp);
        GetStateNextTransactionsIndex(in stateId, out IEnumerable<byte> tn);
        transactionsId = Enumerable.Concat(tp, tn);
      }


    public void GetStatePreviousAndNextStateIndex(in byte stateId, out IEnumerable<byte> statesIds)
    {
        GetStatePreviousStateIndex(in stateId, out IEnumerable<byte> sp);
        GetStateNextStatesIndex(in stateId, out IEnumerable<byte> sn);
        statesIds = Enumerable.Concat(sp, sn);
    }
    public void GetStatePreviousAndNextTransactionsName(in byte stateId, out IEnumerable<string> transactionsName)
    {
        GetStatePreviousTransactionsName(in stateId, out IEnumerable<string> tp);
        GetStateNextTransactionsName(in stateId, out IEnumerable<string> tn);
        transactionsName = Enumerable.Concat(tp, tn);
    }


    public void GetStatePreviousAndNextStateName(in byte stateId, out IEnumerable<string> statesName)
    {
        GetStatePreviousStateName(in stateId, out IEnumerable<string> sp);
        GetStateNextStateName(in stateId, out IEnumerable<string> sn);
        statesName = Enumerable.Concat(sp, sn);
    }

    public void GetIdOfTransaction(in StringTransaction transaction, out byte transactionId)
    {
        StringFSMDeductionUtility.ConvertTransition(in transaction, out transactionId, in m_source);
    }


    private static int m_randomIndex = 0;
    public static T RandomElement<T>(IEnumerable<T> enumerable)
    {
        return RandomElementUsing<T>(enumerable, new System.Random(m_randomIndex++));
    }
   
    public static T RandomElementUsing<T>( IEnumerable<T> enumerable, Random rand)
    {
        int index = rand.Next(0, enumerable.Count());
        return enumerable.ElementAt(index);
    }
}