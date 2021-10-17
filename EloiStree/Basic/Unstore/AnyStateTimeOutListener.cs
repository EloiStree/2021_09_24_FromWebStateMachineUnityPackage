using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Eloi;
using UnityEngine.Events;

public class AnyStateTimeOutListener : MonoBehaviour, IStateChangeByteReceiver
    {
        public float m_maxTimeInSeconds = 500;
        public UnityEvent m_timeoutEvent;

        [Header("Debug")]
        public float m_currentTime = 0;
        public byte m_current;
   
    public void NotifyStateChange(byte fromState, byte toState)
    {
        m_current = toState;
        m_currentTime = 0;
    }

    void Update()
        {
           
                float previousTime = m_currentTime;
                m_currentTime += Time.deltaTime;
                if (previousTime < m_maxTimeInSeconds && m_currentTime >= m_maxTimeInSeconds)
                {
                    m_timeoutEvent.Invoke();
                }
            
        }
    }
