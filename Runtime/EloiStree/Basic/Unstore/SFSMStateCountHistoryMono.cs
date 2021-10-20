using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System;

public class SFSMStateCountHistoryMono : MonoBehaviour, IStateChangeByteReceiver, IStateChangeByteWithFineStateReceiver
{
    public SFSMStateCountByte m_countState = new SFSMStateCountByte();

    public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
    {
        m_countState.Push( toState);
    }
    public void NotifyStateChange(byte fromState, byte toState)
    {
        m_countState.Push(toState);
    }
    public void Clear()
    {
        m_countState.Clear();
    }
    public void Get(out SFSMStateCountCollection<byte> countState)
    {
        countState = m_countState;
    }
}


[System.Serializable]
public class SFSMStateCountByte : SFSMStateCountCollection<byte>
{

}
[System.Serializable]
public class SFSMStateCount<T>
{
    public T m_value;
    public uint m_count;

    public SFSMStateCount(T stateType)
    {
        this.m_value = stateType;
        m_count = 0;
    }
}
[System.Serializable]
public class SFSMStateCountCollection<T>
{
    public List<SFSMStateCount<T>> m_collection = new List<SFSMStateCount<T>>();
    public void Clear()
    {
        m_collection.Clear();
    }
    public void Push( T state) {

        for (int i = 0; i < m_collection.Count; i++)
        {
            if (state.Equals(m_collection[i].m_value))
            {
                m_collection[i].m_count++;
                return;
            }
        }

            SFSMStateCount<T> v = new SFSMStateCount<T>(state);
            v.m_count++;
            m_collection.Add(v);
    }
}
public class SFSMStateCountHistoryGenericMono<T> : MonoBehaviour,
   IStateChangeEnumReceiver<T>,
   IStateChangeEnumWithFineStateReceiver<T> where T : System.Enum
{
    public SFSMStateCountRGB m_countState = new SFSMStateCountRGB();

    public void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState)
    {
        m_countState.Push(toState);
    }
    public void NotifyStateChange(T fromState, T toState)
    {
        m_countState.Push(toState);
    }
    public void Clear()
    {
        m_countState.Clear();
    }
    public void Get(out SFSMStateCountCollection<T> countState)
    {
        countState = m_countState;
    }
    [System.Serializable]
    public class SFSMStateCountRGB : SFSMStateCountCollection<T>
    {

    }
}
