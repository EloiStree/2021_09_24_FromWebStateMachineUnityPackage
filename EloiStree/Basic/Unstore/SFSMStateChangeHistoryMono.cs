using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;
using System;
namespace Eloi
{

    public class SFSMStateChangeHistoryMono : MonoBehaviour, IStateChangeByteReceiver, IStateChangeByteWithFineStateReceiver
    {
        [SerializeField] SFSMStateHistoryByte m_history = new SFSMStateHistoryByte();

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {
            m_history.Append(fromState, toState);
        }
        public void NotifyStateChange(byte fromState, byte toState)
        {
            m_history.Append(fromState, toState);
        }
        public void Clear()
        {
            m_history.Clear();
        }
        public void Get(out SFSMStateHistoryByte history)
        {
            history = m_history;

        }
    }

    [System.Serializable]
    public class SFSMStateHistoryByte : SFSMStateHistory<byte>
    { }


    [System.Serializable]
    public class SFSMStateHistory<T>
    {

        [SerializeField] List<T> m_queue = new List<T>();
        [SerializeField] uint m_maxSize = 10;


        public void Get(out List<T> history)
        {
            history = m_queue;
        }
        public void Get(out uint maxSize)
        {
            maxSize = m_maxSize;
        }

        public void Append(T from, T to)
        {

            if (m_queue.Count == 0)
            {
                m_queue.Insert(0, from);
                m_queue.Insert(0, to);
            }
            else if (m_queue.Count > 0)
            {

                m_queue.Insert(0, to);
            }

            if (m_queue.Count >= m_maxSize)
            {
                m_queue.RemoveAt(m_queue.Count - 1);
            }
        }
        public void Clear()
        {
            m_queue.Clear();
        }
    }

    public class SFSMStateChangeHistoryMono<T> : MonoBehaviour,
        IStateChangeEnumReceiver<T>,
        IStateChangeEnumWithFineStateReceiver<T> where T : System.Enum
    {
        [SerializeField] StateHistoryGeneric m_history;

        [System.Serializable]
        public class StateHistoryGeneric : SFSMStateHistory<T>
        { }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState)
        {
            m_history.Append(fromState, toState);
        }

        public void NotifyStateChange(T fromState, T toState)
        {
            m_history.Append(fromState, toState);
        }
        public void Clear()
        {
            m_history.Clear();
        }
        public void Get(out StateHistoryGeneric history)
        {
            history = m_history;
        }
    }
}