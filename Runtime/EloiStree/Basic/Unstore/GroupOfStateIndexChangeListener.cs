using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOfStateIndexChangeListener : IFSMIndexStateChangeEmitter
{
    public List<IFSMStateIndexChangeListener> m_listeners = new List<IFSMStateIndexChangeListener>();

    public void AddStateIndexChangeListener(IFSMStateIndexChangeListener listener)
    {
        if (m_listeners.Contains(listener))
            m_listeners.Remove(listener);
        m_listeners.Add(listener);
    }

    public void RemoveStateIndexChangeListener(IFSMStateIndexChangeListener listener)
    {
        if (m_listeners.Contains(listener))
            m_listeners.Remove(listener);
    }

    public void NotifyNewChange(in byte fromState, in byte toState) {
        for (int i = 0; i < m_listeners.Count; i++)
        {
            m_listeners[i].OnStateIndexChange( fromState,  toState);
        }
    }
}