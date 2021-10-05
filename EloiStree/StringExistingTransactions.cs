using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StringExistingTransactions
{
    public StringTransaction [] m_transactions;

    public void InitWith(StringTransaction[] transactions)
    {
        m_transactions = transactions;
    }

    public StringTransaction GetTransaction(uint idIndex)
    {
        return m_transactions[idIndex];
    }
}
[System.Serializable]
public struct StringTransaction
{
    public string m_transactionTriggerName;
    public string m_transactionSource;
    public string m_transactionDestination;

    public void InitWith(string stateSourceName, string transactionName, string stateDestinationName)
    {
        m_transactionTriggerName= transactionName;
        m_transactionSource = stateSourceName;
        m_transactionDestination = stateDestinationName;
    }
}
