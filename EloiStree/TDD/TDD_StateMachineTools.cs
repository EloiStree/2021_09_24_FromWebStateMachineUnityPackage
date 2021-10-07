using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TDD_StateMachineTools : MonoBehaviour, IContainSFSMDeductedInfo,
    IContainSFSMBasicInfo,
    IContaintStateAsByte,
    IFSMIndexStateChangeEmitter
{
    public StringFSMDeductedScriptable m_source;


    public byte m_currentState;

    public StateChange m_lastChange;
    public StateChangeValide m_lastValide;
    public StateChangeForced m_lastForced;


    [Header("Observer")]
    public StateEnterPassif m_listentToStateEntering;
    public StateExitPassif m_listentToStateExiting;
    public StateEnterFromMultiplePassif m_listenToMutipleOrigneEntering;
    public StateExitToMultiplePassif m_listenToMutipleOrigneExit;

    [Header("Event")]
    public StateChangeFromToIndexEvent m_stateChangeBytes;
    public StateChangeIndexStuctEvent m_stateChangeStruct;
   // public FSMColorEvent m_stateChangeFsmEnum;


    private StringFSMAccess m_access;
    public GroupOfStateIndexChangeListener m_changeStateListeners = new GroupOfStateIndexChangeListener();
    private void Awake()
    {
        m_access = new StringFSMAccess(m_source);


    }
    public void GoNextState()
    {
        m_access.GetStateNextStatesIndex(in m_currentState, out IEnumerable<byte> states);
        E_UnityRandomUtility.GetRandomOf(out byte newStateValue, states);
        m_lastChange.m_fromStateIndex = m_currentState;
        m_lastChange.m_toStateIndex = newStateValue;
        m_access.HasTransactionBetweenStates(in m_currentState, in newStateValue, out bool found, out byte transactionId);
        if (found)
            m_lastValide.SetWith(m_currentState, newStateValue, transactionId);
        else
            m_lastForced.SetWith(m_currentState, newStateValue);

        m_currentState = newStateValue;
        m_listentToStateEntering.NotifyNewChange(in m_lastChange);
        m_listentToStateExiting.NotifyNewChange(in m_lastChange);
        m_listenToMutipleOrigneEntering.NotifyNewChange(in m_lastChange);
        m_listenToMutipleOrigneExit.NotifyNewChange(in m_lastChange);


        m_changeStateListeners.NotifyNewChange(in m_lastChange.m_fromStateIndex, in m_lastChange.m_toStateIndex);
        m_stateChangeBytes.Invoke(m_lastChange.m_fromStateIndex, m_lastChange.m_toStateIndex);
        m_stateChangeStruct.Invoke(m_lastChange);
        //m_stateChangeFsmEnum.Invoke((S_RGBSTATEMACHINE)m_lastChange.m_fromStateIndex,
        //    (S_RGBSTATEMACHINE)m_lastChange.m_toStateIndex);
    }



    public void GetFSMBasicScriptable(out StringFSMScriptable basicFSM)
    {
        basicFSM = m_source.m_source;
    }

    public void GetFSMDeducted(out StringFSMDeductedScriptable deductedFSM)
    {
        deductedFSM = m_source;
    }
    public void GetStateAsByte(out byte stateIndex)
    {
        stateIndex = m_currentState;
    }


    public void AddStateIndexChangeListener(IFSMStateIndexChangeListener listener)
    {
        m_changeStateListeners.AddStateIndexChangeListener(listener);
    }

    public void RemoveStateIndexChangeListener(IFSMStateIndexChangeListener listener)
    {
        m_changeStateListeners.RemoveStateIndexChangeListener(listener);
    }
    public void NotifyNewChange(in StateChange change)
    {
        m_changeStateListeners.NotifyNewChange(change.m_fromStateIndex, change.m_toStateIndex);
    }
    public void NotifyNewChange(in byte fromState, in byte toState)
    {

        m_changeStateListeners.NotifyNewChange(fromState, toState);
    }

    //[System.Serializable]
    //public class FSMColorEvent : UnityEvent<S_RGBSTATEMACHINE, S_RGBSTATEMACHINE>{}
}
