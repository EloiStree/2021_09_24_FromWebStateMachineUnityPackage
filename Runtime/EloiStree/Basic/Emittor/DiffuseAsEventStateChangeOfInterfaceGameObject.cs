using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffuseAsEventStateChangeOfInterfaceGameObject : MonoBehaviour, IFSMStateIndexChangeListener
{

    [Tooltip("Must be of emitter Inteface")]
    public GameObject m_stateChangeEmitter;
    public IFSMIndexStateChangeEmitter m_stateEmitter;
    [Header("Event")]
    public StateChangeFromToIndexEvent m_stateChangeBytes;
    public StateChangeIndexStuctEvent  m_stateChangeStruct;
    //public StateChangeFromToIndexWithSourceEvent m_stateChangeBytesWithSource;


    void Awake()
    {
        StartListening();
    }

    private void StartListening()
    {
        if (m_stateChangeEmitter != null)
        {
            m_stateEmitter = m_stateChangeEmitter.GetComponent<IFSMIndexStateChangeEmitter>();
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
            if (m_stateEmitter == null)
                m_stateChangeEmitter = null;
        }

    }

    public void OnStateIndexChange(byte fromStateIndex, byte toStateIndex)
    {
        m_stateChangeBytes.Invoke(fromStateIndex, toStateIndex);
        StateChange stateChange = new StateChange();
        stateChange.SetWith(fromStateIndex, toStateIndex);
        m_stateChangeStruct.Invoke(stateChange);
    }
}
