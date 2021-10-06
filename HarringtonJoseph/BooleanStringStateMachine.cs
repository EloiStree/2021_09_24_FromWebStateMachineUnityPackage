using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
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
    [SerializeField]
    protected StringFSMDeductedScriptable m_initialInformation;

    public AbstractBooleanStringStateMachineRunning(StringFSMDeductedScriptable initialInformation)
    {
        m_initialInformation = initialInformation;
    }

    public abstract void AddTransitionFailListener(TransitionRequestFail listener);
    public abstract void ForceTransition(in string transitionName, in string stateSource, in string stateDestination);
    public abstract void GetCurrentState(out string name);
    public abstract void RemoveTransitionFailListener(TransitionRequestFail listener);
    public abstract void TryToTriggerTransition(in string name);
    public abstract void TryToTriggerTransition(in string name, out bool scuced);
    public abstract void TryToTriggerTransition(in string name, out bool succed, out string whatHappend);
}


public interface IBooleanStringStateMachineRunning {

     void TryToTriggerTransition(in string name);
     void TryToTriggerTransition(in string name, out bool scuced);
     void TryToTriggerTransition(in string name, out bool succed, out string whatHappend);

    /// <summary>
    /// At your own risk.
    /// </summary>
     void ForceTransition(in string transitionName, in string stateSource, in string stateDestination);

     void GetCurrentState(out string name);

     void AddTransitionFailListener(TransitionRequestFail listener);
     void RemoveTransitionFailListener(TransitionRequestFail listener);
}

public interface IBooleanStringStateMachinePlus: IBooleanStringStateMachineRunning
{
     void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<StringTransaction> transactions);
     void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<StringTransaction> transactions);
     void GetAllTransitionSourceOf(in string stateName, out IEnumerable<StringTransaction> transactions);
}

public interface IJobableBooleanStringStateConvertion{
     uint GetStateAsId(in string stateName);
     uint GetTransactionAsId(in string stateName);
}




[System.Serializable]
public class FirstExperimentBoolStrSM : AbstractBooleanStringStateMachineRunning, IBooleanStringStateMachinePlus
{
    public StringFSMAccess m_fsm;
    public string m_currentState;
    public byte m_currentStateId;

    public string m_previousState;
    public byte m_previousStateId;

   

    public string m_previousTransaction;
    public byte m_previousTransactionId;

    public TransitionRequestSuccess     m_transitionSuccess;
    public TransitionRequestFail        m_transitionFail;

    public UnitySFSM_State2StateName    m_state2StateChange;
    public UnitySFSM_Transition         m_transitionCalled;

    internal void Init(StringFSMDeductedScriptable initialInformation)
    {
        m_initialInformation = initialInformation;
        m_fsm = new StringFSMAccess(initialInformation);
        m_fsm.GetInitState(out m_currentState);
        m_currentStateId = GetStateIdOf(m_currentState);

        m_previousState = m_currentState;
        m_previousStateId = m_currentStateId;
    }

    public FirstExperimentBoolStrSM(StringFSMDeductedScriptable initialInformation) : base(initialInformation)
    {
        Init(initialInformation);

    }

    public bool HasPreviousState() { return string.IsNullOrEmpty(m_previousState); }
    public bool HasPreviousTransition() { return string.IsNullOrEmpty(m_previousTransaction); }

    public void SetCurrentState(in byte stateId) {
        m_currentStateId = stateId;
        m_fsm.GetNameOfState(in stateId, out m_currentState);
    }
    public void SetPreviousState(in byte stateId)
    {
        m_previousStateId = stateId;
        m_fsm.GetNameOfState(in stateId, out m_previousState);

    }
    public void SetPreviousTransaction(in byte transactionId)
    {
        m_previousTransactionId = transactionId;
        m_fsm.GetNameOfTransaction(in transactionId, out m_previousTransaction);

    }

    public override void AddTransitionFailListener(TransitionRequestFail listener)
    {
        m_transitionFail += listener;
    }
    public override void ForceTransition(in string transitionName, in string stateSource, in string stateDestination)
    {
        throw new System.NotImplementedException();
    }
    private byte GetStateIdOf(string stateName)
    {
        m_fsm.GetIdOfState(in stateName, out byte id);
        return id;
    }

    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<byte> transactions)
    {
        m_fsm.GetIdOfState(in stateName, out byte stateId);
        m_fsm.GetStateNextTransactionsIndex( stateId, out transactions);
    }

  
    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionDestinationOfState(in stateName, out IEnumerable<byte> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<byte> transactions)
    {
        m_fsm.GetIdOfState(in stateName, out byte stateId);
        m_fsm.GetStatePreviousAndNextTransactionsIndex(in stateId, out transactions);
     

    }
    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {

        GetAllTransitionLinkedToState(in stateName, out IEnumerable<byte> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

   
    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<byte> transactions)
    {
        m_fsm.GetIdOfState(in stateName, out byte stateId);
        m_fsm.GetStatePreviousTransactionsIndex(in stateId, out transactions);
    }
    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionSourceOf(in stateName, out IEnumerable<byte> tIds);
        transactions = GetTransactionsFromUints(tIds);
    }

    private IEnumerable<StringTransaction> GetTransactionsFromUints(IEnumerable<byte> tIds)
    {
        IEnumerable<StringTransaction> transactions;
        List<StringTransaction> t = new List<StringTransaction>();
        foreach (byte idIndex in tIds)
        {
            t.Add(m_fsm.GetTransactionInfo(idIndex));

        }
        transactions = t;
        return transactions;
    }


    public override void GetCurrentState(out string currentState)
    {
        currentState = m_currentState;
    }
    internal void GetPreviousState(out string previousState)
    {
        previousState = m_previousState;
    }

    public override void RemoveTransitionFailListener(TransitionRequestFail listener)
    {
        m_transitionFail -= listener;
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
                if (m_transitionSuccess != null)
                    m_transitionSuccess(in transactionNameToLook, in m_previousState,in  m_currentState);
                m_state2StateChange?.Invoke(m_previousState, m_currentState);
                m_transitionCalled?.Invoke(m_fsm.GetTransactionInfo(transactionNameToLook));
                succed = true;
                whatHappend = "";
                m_fsm.GetIdOfState(in m_previousState, out byte pId);
                m_fsm.GetIdOfState(in m_currentState, out byte nId);
                m_fsm.GetIdOfTransaction(in item, out byte tId);
                SetPreviousState(in pId);
                SetCurrentState(in nId);
                SetPreviousTransaction(in tId);
                return;
            }
        }

       
        succed = false;
        whatHappend = "Transaction not found"; 
        if (m_transitionFail != null)
            m_transitionFail(in transactionNameToLook, in whatHappend);
        m_state2StateChange?.Invoke(m_previousState, m_currentState);
    }

    public void GetAllTransitions( out IEnumerable< StringTransaction> transactions)
    {
        m_fsm.GetAllTransitions(out transactions);
    }

    public void GetAllTransitionsDistinctName( out IEnumerable<string> transactions)
    {
        m_fsm.GetAllTransitionsDistinctName(out transactions);
    }

    public void GetNextTransactions(out IEnumerable<string> transactionsName)
    {
        m_fsm.GetStateNextTransactionsName(in m_currentStateId, out transactionsName);
    }
    public void GetPreviousTransactions(out IEnumerable<string> transactionsName)
    {
        m_fsm.GetStatePreviousTransactionsName(in m_currentStateId, out transactionsName);
    }
    public void GetNextTransactions(out IEnumerable<StringTransaction> transactions)
    {
        GetAllTransitionDestinationOfState(in m_currentState, out transactions);
    }

   
}