using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "StringFSMDeducted", menuName = "ScriptableObjects/String FSM/SFSM Deducted", order = 1)]
public class StringFSMDeductedScriptable : ScriptableObject
{
    public StringFSMScriptable m_source;

    public StateAndTransactionIntegerRegister m_uintIndexes;
    public PerStateNeighborIndexCollectionAsString m_stringIdNeighborPerState;
    public PerStateNeighborIndexCollectionAsIndex m_uintIndexNeighborPerState;
    public TransactionNameCollisionsCollection m_transactionsCollision;

    [ContextMenu("Compute deduction")]
    public void RefreshDeductionWithSource()
    {
        StringFSMDeductionUtility.Compute(in m_source.m_stringFSM, out m_uintIndexes);
        StringFSMDeductionUtility.Compute(in m_source.m_stringFSM, out m_stringIdNeighborPerState);
        StringFSMDeductionUtility.Compute(in m_source.m_stringFSM, out m_uintIndexNeighborPerState);
        StringFSMDeductionUtility.Compute(in m_source.m_stringFSM, out m_transactionsCollision);

    }
}




public class StringFSMDeductionUtility {

    public static bool Equal(in string a, in string b)
    {
        if (a == null || b == null)
            return false;
        return a.Length == b.Length &&
            a.IndexOf(b) == 0;
    }
    public static void TrimAndLowCaseAll(ref BooleanStringStateMachine target)
    {
        target.m_machineName = target.m_machineName.ToLower().Trim();
        target.m_initialState = target.m_initialState.ToLower().Trim();
        for (byte i = 0; i < target.m_states.m_states.Length; i++)
        {
            target.m_states.m_states[i] = target.m_states.m_states[i].ToLower().Trim();

        }
        for (byte i = 0; i < target.m_transactions.m_transactions.Length; i++)
        {
            target.m_transactions.m_transactions[i].m_transactionDestination = target.m_transactions.m_transactions[i].m_transactionDestination.ToLower().Trim();
            target.m_transactions.m_transactions[i].m_transactionSource = target.m_transactions.m_transactions[i].m_transactionSource.ToLower().Trim();
            target.m_transactions.m_transactions[i].m_transactionTriggerName = target.m_transactions.m_transactions[i].m_transactionTriggerName.ToLower().Trim();
        }
    }
    public static bool IsDefined(in StringFSMDeductedScriptable source)
    {
        return source.m_source != null && IsDefined(in source.m_source) &&
            source.m_uintIndexes.m_statesCount > 0 &&
            source.m_uintIndexes.m_transactionsCount > 0;

    }
    public static bool IsDefined(in StringFSMScriptable source)
    {
            return  source.m_stringFSM.m_states.m_states.Length > 0
            && source.m_stringFSM.m_transactions.m_transactions.Length > 0;
    }
    
    public static void ConvertStateName(in string stateName, out byte index, in StringFSMDeductedScriptable source)
    {
        ConvertStateName(in stateName, out index, in source.m_source.m_stringFSM);
    }
    public static void ConvertStateName(in byte index, out string stateName, in StringFSMDeductedScriptable source)
    {
        ConvertStateName(in index, out stateName, in source.m_source.m_stringFSM);
    }
    public static void ConvertStateName(in string stateName, out byte index, in BooleanStringStateMachine source)
    {
        for (byte i = 0; i < source.m_states.m_states.Length; i++)
        {
            if (Equal(
                in source.m_states.m_states[i] 
                , in stateName))
            {
                index = i;
                return;
            }    
        }
        index = 0;
    }
    public static void ConvertStateName(in byte index, out string stateName, in BooleanStringStateMachine source)
    {
        stateName = source.m_states.m_states[index];
    }

    public static void ConvertTransitionName(in string transitionName,out byte count, out IEnumerable<byte> indexes, in StringFSMDeductedScriptable source)
    {
        TransactionNameCollisionsToIndexes[] ti= source.m_transactionsCollision.m_transactionsNameToIndexes;
        for (byte i = 0; i <ti.Length ; i++)
        {
            if (Equal(in ti[i].m_transactionName, in transitionName)) {
                indexes = ti[i].m_transactionIndexIds;
                count = (byte)ti[i].m_transactionIndexIds.Length;
                return;
            }
        }
        indexes = null;
        count = 0;

    }
    public static void ConvertTransitionName(in byte index, out string tarnsitionName, in StringFSMDeductedScriptable source)
    {

        ConvertTransitionName(in index, out tarnsitionName, in source.m_source.m_stringFSM);
    }
    public static void ConvertTransitionName(in string transitionName, out List<byte> indexes, in BooleanStringStateMachine source)
    {
        indexes = new List<byte>();
        for (byte i = 0; i < source.m_transactions.m_transactions.Length; i++)
        {
            if (source.m_transactions.m_transactions[i].m_transactionTriggerName.Length == transitionName.Length
                && source.m_transactions.m_transactions[i].m_transactionTriggerName.IndexOf(transitionName) == 0)
            {
                indexes.Add(i);
            }
        }
    }
    public static void ConvertTransitionName(in byte index, out string tarnsitionName, in BooleanStringStateMachine source)
    {
        tarnsitionName = source.m_transactions.m_transactions[index].m_transactionTriggerName;
    }

    public static void ConvertTransition(in StringTransaction transaction, out byte transactionId, in StringFSMDeductedScriptable source)
    {
        for (byte i = 0; i < source.m_source.m_stringFSM.m_transactions.m_transactions.Length; i++)
        {
            if (Equal(in transaction, in source.m_source.m_stringFSM.m_transactions.m_transactions[i])) {
                transactionId = i;
                return;
            }
        }
        throw new Exception("Transaction is not in the machine: " + transaction.m_transactionTriggerName);
    }

    private static bool Equal(in StringTransaction transaction, in  StringTransaction stringTransaction)
    {

        return transaction.m_transactionDestination.Length == stringTransaction.m_transactionDestination.Length
            && transaction.m_transactionSource.Length == stringTransaction.m_transactionSource.Length
            && transaction.m_transactionTriggerName.Length == stringTransaction.m_transactionTriggerName.Length
            && transaction.m_transactionDestination.IndexOf(stringTransaction.m_transactionDestination) == 0
            && transaction.m_transactionSource.IndexOf(stringTransaction.m_transactionSource) == 0
            && transaction.m_transactionTriggerName.IndexOf(stringTransaction.m_transactionTriggerName) == 0;
    }

    public static void ConvertTransitionSourceState(in byte index, out string stateName, in BooleanStringStateMachine source)
    {
        stateName = source.m_transactions.m_transactions[index].m_transactionSource;
    }
    public static void ConvertTransitionDestinationState(in byte index, out string stateName, in BooleanStringStateMachine source)
    {
        stateName = source.m_transactions.m_transactions[index].m_transactionDestination;
    }

    public static void Compute(in BooleanStringStateMachine stringStateMachine
        , out StateAndTransactionIntegerRegister registerAsUint)
    {
        registerAsUint = new StateAndTransactionIntegerRegister();
        registerAsUint.Init(
            (byte)stringStateMachine.m_states.m_states.Length,
            (byte)stringStateMachine.m_transactions.m_transactions.Length
            );

        byte index = 0;
        foreach (var item in stringStateMachine.m_transactions.m_transactions)
        {
            TransitionAsIntegerByte transaction = new TransitionAsIntegerByte();
            transaction.m_transitionId = index;
            transaction.m_sourceStateId = GetIndexOf(in stringStateMachine.m_states.m_states, in item.m_transactionSource);
            transaction.m_destinationStateId = GetIndexOf(in stringStateMachine.m_states.m_states, in item.m_transactionDestination);
            registerAsUint.m_transitionsAsIndex[index] = transaction;
            index++;
        }
    }
    private static byte GetIndexOf(in string[] states, in string stateName)
    {
        for (byte i = 0; i < states.Length; i++)
        {
            if (states[i].Length == stateName.Length && states[i].IndexOf(stateName) == 0)
            {
                return (byte)i;
            }
        }
        throw new System.Exception("Transition should go from one state to an other, but the state was not found.");
    }

    public static void Compute(in BooleanStringStateMachine stringStateMachine
      , out PerStateNeighborIndexCollectionAsString registerAsUint)
    {

        byte stateCount =(byte) stringStateMachine.m_states.m_states.Length;
        registerAsUint = new PerStateNeighborIndexCollectionAsString();
        registerAsUint.m_listOfStateNeighbor = new PerStateNeighborAsString[stateCount];
        for (byte i = 0; i < stateCount; i++)
        {
            PerStateNeighborAsString perState = new PerStateNeighborAsString();
            string state = stringStateMachine.m_states.m_states[i];
            List<string> transitionDestination = new List<string>();
            List<string> stateDestination       = new List<string>();
            List<string> transitionSource       = new List<string>();
            List<string> stateSource            = new List<string>();

            perState.m_stateId = state;

            foreach (var transaction in stringStateMachine.m_transactions.m_transactions)
            {
                //source if state is a destination in transaction
                if (transaction.m_transactionDestination == state)
                {
                    transitionSource.Add(transaction.m_transactionTriggerName);
                    stateSource.Add(transaction.m_transactionSource);
                }

                //is desitnation if state is source in transaction
                if (transaction.m_transactionSource == state)
                {

                    transitionDestination.Add(transaction.m_transactionTriggerName);
                    stateDestination.Add(transaction.m_transactionDestination);
                }
            }
            perState.m_transitionDestination = transitionDestination.Distinct().ToArray();
            perState.m_stateDestination      = stateDestination.Distinct().ToArray();
            perState.m_transitionSource      = transitionSource.Distinct().ToArray();
            perState.m_stateSource           = stateSource.Distinct().ToArray();
            registerAsUint.m_listOfStateNeighbor[i] = perState;
        }
    }
    public static void Compute(in BooleanStringStateMachine stringStateMachine
      , out PerStateNeighborIndexCollectionAsIndex registerAsUint)
    {

        byte stateCount =(byte) stringStateMachine.m_states.m_states.Length;
        registerAsUint = new PerStateNeighborIndexCollectionAsIndex();
        registerAsUint.m_listOfStateNeighbor = new PerStateNeighborAsIndex[stateCount];
        for (int i = 0; i < stateCount; i++)
        {
            PerStateNeighborAsIndex perState = new PerStateNeighborAsIndex();
            string state = stringStateMachine.m_states.m_states[i];
            List<byte> transitionDestination = new List<byte>();
            List<byte> stateDestination = new List<byte>();
            List<byte> transitionSource = new List<byte>();
            List<byte> stateSource = new List<byte>();

            ConvertStateName(state, out perState.m_stateId, in stringStateMachine);

            for (byte j = 0; j < stringStateMachine.m_transactions.m_transactions.Length; j++)
            {
                var transaction = stringStateMachine.m_transactions.m_transactions[j];
                //source if state is a destination in transaction
                if (transaction.m_transactionDestination == state)
                {
                    transitionSource.Add(j);
                    ConvertStateName(transaction.m_transactionSource, out byte index, in stringStateMachine);
                    stateSource.Add(index);
                }

                //is desitnation if state is source in transaction
                if (transaction.m_transactionSource == state)
                {
                    transitionDestination.Add(j);
                    ConvertStateName(transaction.m_transactionDestination, out byte index, in stringStateMachine);
                    stateDestination.Add(index);
                }
            }

            perState.m_transitionDestination = transitionDestination.Distinct().ToArray();
            perState.m_stateDestination = stateDestination.Distinct().ToArray();
            perState.m_transitionSource = transitionSource.Distinct().ToArray();
            perState.m_stateSource = stateSource.Distinct().ToArray();
            registerAsUint.m_listOfStateNeighbor[i] = perState;
        }
    }

    public  static void Compute(
        in BooleanStringStateMachine stringStateMachine, 
        out TransactionNameCollisionsCollection transactionsCollision)
    {
        List<string> names = new List<string>();
        for (byte i = 0; i < stringStateMachine.m_transactions.m_transactions.Length; i++)
        {
            StringTransaction t = stringStateMachine.m_transactions.m_transactions[i];
            names.Add(t.m_transactionTriggerName);
        }
        transactionsCollision = new TransactionNameCollisionsCollection();
        transactionsCollision.m_uniqueTransitions = names.Distinct().ToArray();
        List<TransactionNameCollisionsToIndexes> collisions = new List<TransactionNameCollisionsToIndexes>();
        for (byte i = 0; i < transactionsCollision.m_uniqueTransitions.Length; i++)
        {
            TransactionNameCollisionsToIndexes collision = new TransactionNameCollisionsToIndexes();
            string name = transactionsCollision.m_uniqueTransitions[i];
            collision.m_transactionName = name;
            List<byte> ids = new List<byte>();
            for (byte j = 0; j < stringStateMachine.m_transactions.m_transactions.Length; j++)
            {
                string transitionName = stringStateMachine.m_transactions.m_transactions[j].m_transactionTriggerName;

                if (name.Length== transitionName.Length
                    && name.IndexOf(transitionName) == 0)
                {
                    ids.Add(j);
                }
            }
            collision.m_transactionIndexIds = ids.ToArray();
            collisions.Add(collision);
        }
        transactionsCollision.m_transactionsNameToIndexes = collisions.ToArray();
    }
}