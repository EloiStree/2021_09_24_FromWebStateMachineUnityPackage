using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooleanStringStateMachineMono : MonoBehaviour, IBooleanStringStateMachineRunning, IBooleanStringStateMachinePlus
{



    [Header("In")]
    public StringFSMDeductedScriptable m_stringStateMachine;

    public void TryToTriggerTransitionWithState(string stateName)
    {
        m_firstBooleanType.GetNextTransactions(out IEnumerable<StringTransaction> transactionsName);
        foreach (var item in transactionsName)
        {
            if (item.m_transactionDestination == stateName) { 
                TryToTriggerTransition(item.m_transactionTriggerName);
                return;
            }
        }
    }


    [Header("Debug")]
    public FirstExperimentBoolStrSM m_firstBooleanType;
    public AbstractBooleanStringStateMachineRunning m_stateMachine;

    public TransitionRequestFail m_failListeners;
    public TransitionRequestSuccess m_successListeners;


    public void GetAllTransactions(out IEnumerable<string> transactionsName)
    {
        m_firstBooleanType.GetAllTransitionsDistinctName(out transactionsName);
    }
    internal void GetNextTransactions(out IEnumerable<string> transactionsName)
    {
        m_firstBooleanType.GetNextTransactions(out transactionsName);
    }


    public void GetAllTransactions(out IEnumerable<StringTransaction> transactionsName)
    {
        m_firstBooleanType.GetAllTransitions(out transactionsName);
    }
    internal void GetNextTransactions(out IEnumerable<StringTransaction> transactionsName)
    {
        m_firstBooleanType.GetNextTransactions(out transactionsName);
    }



    public void ForceTransition(in string transitionName, in string stateSource, in string stateDestination)
    {
        throw new System.NotImplementedException();
    }

    public void GetAllTransitionDestinationOfState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {

        m_firstBooleanType.GetAllTransitionDestinationOfState(in stateName, out transactions);
    }

    public void GetAllTransitionLinkedToState(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        m_firstBooleanType.GetAllTransitionLinkedToState(in stateName, out transactions);
    }

   
    public void GetAllTransitionSourceOf(in string stateName, out IEnumerable<StringTransaction> transactions)
    {
        m_firstBooleanType.GetAllTransitionSourceOf(in stateName, out transactions);
    }

    public void GetCurrentState(out string name)
    {
        m_firstBooleanType.GetCurrentState(out name);
    }

    internal void GetPreviousState(out string previousState)
    {
        m_firstBooleanType.GetPreviousState(out previousState);
    }
   
    public void TryToTriggerTransition(in string name)
    {
        m_firstBooleanType.TryToTriggerTransition(in name);
    }

    public void TryToTriggerTransition(in string name, out bool succed)
    {
        m_firstBooleanType.TryToTriggerTransition(in name, out succed);
    }

    public void TryToTriggerTransition(in string name, out bool succed, out string whatHappend)
    {
        m_firstBooleanType.TryToTriggerTransition(in name, out succed, out whatHappend);
    }

    void Start()
    {
        m_firstBooleanType.Init(m_stringStateMachine);
        m_stateMachine = m_firstBooleanType;
        


    }

    public void AddTransitionFailListener(TransitionRequestFail listener)
    {
        m_failListeners += listener;
    }
    public void RemoveTransitionFailListener(TransitionRequestFail listener)
    {
        m_failListeners -= listener;
    }


}
