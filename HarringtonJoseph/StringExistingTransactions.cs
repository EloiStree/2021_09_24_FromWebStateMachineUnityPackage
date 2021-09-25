using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StringExistingTransactions
{
    public StringTransaction [] m_transactions;



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

}
