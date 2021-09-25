using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TDD_BooleanStringStateMachineMono : MonoBehaviour
{
    public BooleanStringStateMachineMono m_target;
    public RandomTryType m_randomType;
    public enum RandomTryType { AllTransactions, NextTransaction }
    public string m_previousState;
    public string m_currentState;
    public string[] m_allTransactions;

    public float m_timeBetweenRandomTrigger=0.5f;
    public string m_lastPush;

    void Start()
    {
        InvokeRepeating("PushRandom", m_timeBetweenRandomTrigger, m_timeBetweenRandomTrigger);
    }


    void PushRandom()
    {
       

          
                 m_target.GetCurrentState(out m_previousState);
                IEnumerable<string> t=null;
                if (m_randomType == RandomTryType.AllTransactions)
                    m_target.GetAllTransactions(out t);
                if (m_randomType == RandomTryType.NextTransaction)
                    m_target.GetNextTransactions(out t);

                t = t.Distinct();
                m_allTransactions = t.ToArray();
            
            m_lastPush = GetRandomTransaction();
            m_target.TryToTriggerTransition(m_lastPush);
        m_target.GetCurrentState(out m_currentState);



    }

    private string GetRandomTransaction()
    {
        return m_allTransactions[UnityEngine.Random.Range(0, m_allTransactions.Length)];
    }
}
