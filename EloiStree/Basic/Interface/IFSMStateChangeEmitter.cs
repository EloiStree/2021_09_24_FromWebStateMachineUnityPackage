using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFSMIndexStateChangeEmitter
{

    public void AddStateIndexChangeListener(IFSMStateIndexChangeListener listener);
    public void RemoveStateIndexChangeListener(IFSMStateIndexChangeListener listener);

}

public interface IFSMStateIndexChangeListener
{
    void OnStateIndexChange( byte fromStateIndex,  byte toStateIndex);
}

public interface IFSMStringStateChangeEmitter
{

    public void AddStateChangeListener(IFSMStateStringChangeListener listener);
    public void RemoveStateChangeListener(IFSMStateStringChangeListener listener);

}


public interface IFSMStateStringChangeListener
{
    void OnStateStringStateIndex( string fromStateName,  string toStateName);
}

public interface IFSMStringStateChangeEmitter<T> where T : System.Enum
{

    public void AddStateChangeListener(IFSMStateEnumChangeListener<T> listener);
    public void RemoveStateChangeListener(IFSMStateEnumChangeListener<T> listener);

}
public interface IFSMStateEnumChangeListener<T> where T : System.Enum
{
    void OnStateEnumIndexChange( T fromState,  T toState);
}



public interface IFSMIndexTransactionChangeEmitter
{

    public void AddIndexTranscationChangeListener(IFSMTransactionListener listener);
    public void RemoveIndexTransactionChangeListener(IFSMTransactionListener listener);
}
public interface IFSMTransactionListener
{
    void OnTransactionIndexChange(out byte transactionId);
}
