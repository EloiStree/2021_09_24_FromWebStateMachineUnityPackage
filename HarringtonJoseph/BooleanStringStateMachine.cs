using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BooleanStringStateMachineUtility {
    public static void TrimAndLowCaseAll(ref BooleanStringStateMachine target)
    {
        target.m_machineName = target.m_machineName.ToLower().Trim();
        target.m_initialState = target.m_initialState.ToLower().Trim();
        for (int i = 0; i < target.m_states.m_states.Length; i++)
        {
            target.m_states.m_states[i] = target.m_states.m_states[i].ToLower().Trim();

        }
        for (int i = 0; i < target.m_transactions.m_transactions.Length; i++)
        {
            target.m_transactions.m_transactions[i].m_transactionDestination = target.m_transactions.m_transactions[i].m_transactionDestination.ToLower().Trim();
            target.m_transactions.m_transactions[i].m_transactionSource = target.m_transactions.m_transactions[i].m_transactionSource.ToLower().Trim();
            target.m_transactions.m_transactions[i].m_transactionTriggerName = target.m_transactions.m_transactions[i].m_transactionTriggerName.ToLower().Trim();
        }
    }
}

public struct BooleanStringStateMachine 
{
    public string m_machineName;
    public string m_initialState;
    public StringExistingStates m_states;
    public StringExistingTransactions m_transactions;

    public BooleanStringStateMachine(string machineName, string initialTransactions, StringExistingStates states, StringExistingTransactions transactions)
    {
        m_machineName = machineName;
        m_initialState = initialTransactions;
        m_states = states;
        m_transactions = transactions;
    }
    public StringTransaction GetTransaction(uint idIndex)
    {
        return m_transactions.GetTransaction( idIndex);
    }
}

[System.Serializable]
public abstract class AbstractBooleanStringStateMachineRunning : IBooleanStringStateMachineRunning
{
    public BooleanStringStateMachine m_initialInformation;

    public AbstractBooleanStringStateMachineRunning(BooleanStringStateMachine initialInformation)
    {
        m_initialInformation = initialInformation;
        BooleanStringStateMachineUtility.TrimAndLowCaseAll(ref m_initialInformation);
    }

    public abstract void AddTransitionFailListener(TransitionFail listener);
    public abstract void ForceTransition(in string transitionName, in string stateSource, in string stateDestination);
    public abstract void GetCurrentState(out string name);
    public abstract void RemoveTransitionFailListener(TransitionFail listener);
    public abstract void TryToTriggerTransition(in string name);
    public abstract void TryToTriggerTransition(in string name, out bool scuced);
    public abstract void TryToTriggerTransition(in string name, out bool succed, out string whatHappend);
}


public interface IBooleanStringStateMachineRunning {

    public void TryToTriggerTransition(in string name);
    public void TryToTriggerTransition(in string name, out bool scuced);
    public void TryToTriggerTransition(in string name, out bool succed, out string whatHappend);

    /// <summary>
    /// At your own risk.
    /// </summary>
    public void ForceTransition(in string transitionName, in string stateSource, in string stateDestination);

    public void GetCurrentState(out string name);

    public void AddTransitionFailListener(TransitionFail listener);
    public void RemoveTransitionFailListener(TransitionFail listener);
}

public interface IBooleanStringStateMachinePlus: IBooleanStringStateMachineRunning
{
    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<StringTransaction> transactions);
    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<StringTransaction> transactions);
    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<StringTransaction> transactions);
}

public interface IJobableBooleanStringStateConvertion{
    public uint GetStateAsId(in string stateName);
    public uint GetTransactionAsId(in string stateName);
}


public delegate void TransitionFail(string transitionName, string whatHappened);
public delegate void TransitionSuccess(string transitionName, string sourceState, string destinationState);


public class ConvertBooleanStringToUINHolder {
    public static void Convert(in BooleanStringStateMachine stateMachine, out TransitionsAndExistingState uintHolder) {
        uintHolder = new TransitionsAndExistingState();
        uintHolder.Init((uint)stateMachine.m_states.m_states.Length, (uint)stateMachine.m_transactions.m_transactions.Length);


        uint index = 0;
        foreach (var item in stateMachine.m_transactions.m_transactions)
        {
            TransitionAsUInt transaction = new TransitionAsUInt();
            transaction.m_transitionId = index;
            transaction.m_sourceStateId = GetIndexOf( stateMachine.m_states.m_states, item.m_transactionSource);
            transaction.m_destinationStateId = GetIndexOf( stateMachine.m_states.m_states, item.m_transactionDestination);
            uintHolder.m_transitionsAsIndex[index] = transaction;
            index++;
        }
    }

    private static uint GetIndexOf(string[] states, string stateName)
    {
        for (int i = 0; i < states .Length; i++)
        {
            if (states[i].Length == stateName.Length && states[i].IndexOf(stateName)==0)
            {
                return (uint)i;
            }
        }
        throw new System.Exception("Transition should go from one state to an other, but the state was not found.");
    }
}


[System.Serializable]
public class FirstExperimentBoolStrSM : AbstractBooleanStringStateMachineRunning, IBooleanStringStateMachinePlus
{
    public BooleanStringStateMachine m_givenStateMachine;
    public TransitionsAndExistingState m_createdIndexes;
    public Dictionary<string, List<uint>> m_transitionsIndex = new Dictionary<string, List<uint>>();
    public Dictionary<string, uint> m_statesIndex = new Dictionary<string, uint>();
    public string m_currentState;
    public string m_previousState;

    public FirstExperimentBoolStrSM(BooleanStringStateMachine initialInformation) : base(initialInformation)
    {
        m_givenStateMachine = initialInformation;
        m_currentState = initialInformation.m_initialState;
        m_previousState = initialInformation.m_initialState;
        m_createdIndexes = new TransitionsAndExistingState();
        ConvertBooleanStringToUINHolder.Convert(in initialInformation, out m_createdIndexes);
        uint index = 0;
        foreach (var item in m_initialInformation.m_states.m_states)
        {
            m_statesIndex.Add(item, index);
            index++;
        }
        index = 0;
        foreach (var item in m_initialInformation.m_transactions.m_transactions)
        {
            if (!m_transitionsIndex.ContainsKey(item.m_transactionTriggerName)) {
                m_transitionsIndex.Add(item.m_transactionTriggerName, new List<uint>());
            }
            m_transitionsIndex[item.m_transactionTriggerName].Add(index);
            index++;
        }
    }

    public override void AddTransitionFailListener(TransitionFail listener)
    {
        throw new System.NotImplementedException();
    }

    public override void ForceTransition(in string transitionName, in string stateSource, in string stateDestination)
    {
        throw new System.NotImplementedException();
    }
    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<uint> transactions)
    {
        uint state = GetStateIdOf(stateName);
        m_createdIndexes.GetDestinationTransactionOfState(state, out transactions);

    }

    private uint GetStateIdOf(string stateName)
    {
        return m_statesIndex[stateName.ToLower().Trim()];
    }

    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionDestinationOfState(in stateName, out IEnumerable<uint> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<uint> transactions)
    {
        uint state = GetStateIdOf(stateName);
        m_createdIndexes.GetSourceTransactionsOfState(state, out IEnumerable<uint> transactionsIdsDestination);
        m_createdIndexes.GetDestinationTransactionOfState(state, out IEnumerable<uint> transactionsIdsSource);

        transactions = Enumerable.Concat(transactionsIdsSource, transactionsIdsDestination);
    }
    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {

        GetAllTransitionLinkedToState(in stateName, out IEnumerable<uint> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

    private IEnumerable<StringTransaction> GetTransactionsFromUints(IEnumerable<uint> tIds)
    {
        IEnumerable<StringTransaction> transactions;
        List<StringTransaction> t = new List<StringTransaction>();
        foreach (uint idIndex in tIds)
        {
            t.Add(m_givenStateMachine.GetTransaction(idIndex));
        }
        transactions = t;
        return transactions;
    }

    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<uint> transactions)
    {
        uint state = GetStateIdOf(stateName);
        m_createdIndexes.GetSourceTransactionsOfState(state, out transactions);

    }
    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionSourceOf(in stateName, out IEnumerable<uint> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

    public override void GetCurrentState(out string currentState)
    {
        currentState = m_currentState;
    }
    internal void GetPreviousState(out string previousState)
    {
        previousState = m_previousState;
    }

    public override void RemoveTransitionFailListener(TransitionFail listener)
    {
        throw new System.NotImplementedException();
    }

    public override void TryToTriggerTransition(in string name)
    {
        TryToTriggerTransition(in name, out bool succeed, out string t);
    }

    public override void TryToTriggerTransition(in string name, out bool succeed)
    {
        TryToTriggerTransition(in name, out succeed, out string t);
    }

    public override void TryToTriggerTransition(in string name, out bool succed, out string whatHappend)
    {


        GetAllTransitionDestinationOfState(in m_currentState, out IEnumerable<StringTransaction> transactions);
     
        if (transactions == null || transactions.Count() == 0)
        {
            whatHappend = "No destination found";
        }
        string transactionNameToLook = name.Trim().ToLower();
        //Debug.Log("ST>:" + m_currentStateD+" - "+name);
        foreach (var item in transactions)
        {
          //  Debug.Log("T>:" + item.m_transactionTriggerName + " - " + transactionNameToLook);
            if (item.m_transactionTriggerName == transactionNameToLook) {
                m_previousState = m_currentState;
                m_currentState = item.m_transactionDestination;
                succed = true;
                whatHappend = "";
                return;
            }
        }

        succed = false;
        whatHappend = "Transaction not found";
    }

    public void GetAllTransitions( out IEnumerable< StringTransaction> transactions)
    {
        transactions=m_givenStateMachine.m_transactions.m_transactions;
    }

    public void GetAllTransitions( out IEnumerable<string> transactions)
    {
        transactions = m_givenStateMachine.m_transactions.m_transactions
            .Select(k=>k.m_transactionTriggerName);

    }

    public void GetNextTransactions(out IEnumerable<string> transactionsName)
    {
        GetAllTransitionDestinationOfState(in m_currentState, out IEnumerable<StringTransaction> transactions);
        transactionsName = transactions.Select(k => k.m_transactionTriggerName);
    }
    public void GetNextTransactions(out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionDestinationOfState(in m_currentState, out transactions);
    }

   
}