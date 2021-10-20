using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffuseAsEventStateChangeWithFineStateOfInterfaceGameObject : MonoBehaviour, IFSMStateIndexChangeListener
{

    [Tooltip("Must be of emitter Inteface")]
    public GameObject m_stateChangeEmitter;
    public IFSMIndexStateChangeEmitter m_stateEmitter;
    public IContainSFSMDeductedInfo m_sfsmContainer;
    [Header("Event")]
    public StateChangeIndexWithFineStateEvent m_stateChangeBytes;
    public StateChangeIndexWithFineStateStructEvent m_stateChangeStruct;

    void Awake()
    {
        StartListening();
    }

    private void StartListening()
    {
        if (m_stateChangeEmitter != null)
        {
            m_stateEmitter = m_stateChangeEmitter.GetComponent<IFSMIndexStateChangeEmitter>();
            m_sfsmContainer = m_stateChangeEmitter.GetComponent<IContainSFSMDeductedInfo>();
            if (m_stateEmitter != null)
                m_stateEmitter.AddStateIndexChangeListener(this);
        }
    }

    private void OnDestroy()
    {
        StopListening();
    }

    private void StopListening()
    {
        if (m_stateEmitter != null)
            m_stateEmitter.RemoveStateIndexChangeListener(this);
    }
    private void OnValidate()
    {

        if (!Application.isPlaying && m_stateChangeEmitter != null)
        {
            m_stateEmitter = m_stateChangeEmitter.GetComponent<IFSMIndexStateChangeEmitter>();
            m_sfsmContainer = m_stateChangeEmitter.GetComponent<IContainSFSMDeductedInfo>();
            if (m_stateEmitter == null)
                m_stateChangeEmitter = null;
            if (m_sfsmContainer == null)
                m_stateChangeEmitter = null;
        }

    }

    public void OnStateIndexChange(byte fromStateIndex, byte toStateIndex)
    {
        m_stateChangeBytes.Invoke(m_sfsmContainer,fromStateIndex, toStateIndex);
        StateChangeWithFineState stateChange = new StateChangeWithFineState();
        stateChange.SetWith(m_sfsmContainer,fromStateIndex, toStateIndex);
        m_stateChangeStruct.Invoke(stateChange);
    }
}
