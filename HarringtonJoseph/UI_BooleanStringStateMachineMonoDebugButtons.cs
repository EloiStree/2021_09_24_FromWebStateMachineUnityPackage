using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_BooleanStringStateMachineMonoDebugButtons : MonoBehaviour
{
    public BooleanStringStateMachineMono m_linked;

    public Text m_previousState;
    public Text m_currentState;
    public Button[] m_nextState;
    public Button[] m_nextTransaction;

    public string[] m_stateName;
    public string[] m_stateTransaction;
    public void TryToMakeTransitionFromStateName(string stateName)
    {
        m_linked.TryToTriggerTransitionWithState(stateName);
    }
    public void TryToMakeTransitionFromTransitionName(string transitionName)
    {
        m_linked.TryToTriggerTransition(transitionName);
    }
    private void Awake()
    {
        for (int i = 0; i < m_nextState.Length; i++)
        {
            int index = i;
            m_nextState[i].onClick.AddListener(() => {
                PushButtonState(index);
            });
        }
        for (int i = 0; i < m_nextTransaction.Length; i++)
        {
            int index = i;
            m_nextTransaction[i].onClick.AddListener(() => {
                PushButtonTransition(index);
            });
        }
    }

    private void PushButtonState(int index)
    {
        TryToMakeTransitionFromStateName(m_nextState[index].GetComponentInChildren<Text>().text);
    }
    private void PushButtonTransition(int index)
    {
       TryToMakeTransitionFromTransitionName( m_nextTransaction[index].GetComponentInChildren<Text>().text);
    }

    public void Update()
    {
        m_linked.GetCurrentState(out string currentState);
        if(m_currentState)
            m_currentState.text = currentState;

        m_linked.GetPreviousState(out string previousState);
        if (m_previousState)
            m_previousState.text = previousState;

        m_linked.GetNextTransactions(out IEnumerable<StringTransaction> transactions);
     
        m_stateName = transactions.Select(k => k.m_transactionDestination).Distinct().ToArray();
        m_stateTransaction = transactions.Select(k => k.m_transactionTriggerName).Distinct().ToArray();

        for (int i = 0; i < m_nextState.Length; i++)
        {
            if (i < m_stateName.Length)
            {

                m_nextState[i].GetComponentInChildren<Text>().text = m_stateName[i];
                m_nextState[i].gameObject.SetActive(true);
            }
            else {
                m_nextState[i].gameObject.SetActive(false);
            }

        }
        for (int i = 0; i < m_nextTransaction.Length; i++)
        {
            if (i < m_stateTransaction.Length)
            {
                m_nextTransaction[i].GetComponentInChildren<Text>().text = m_stateTransaction[i];
                m_nextTransaction[i].gameObject.SetActive(true);
            }
            else
            {
                m_nextTransaction[i].gameObject.SetActive(false);
            }
        }

    }

}
