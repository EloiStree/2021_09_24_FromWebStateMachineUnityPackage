using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi {

    [System.Serializable]
    public struct StateEnterPassif
    {
        [SerializeField] byte m_observeStateId;
        [SerializeField] bool m_isEnteredState;
        public void SetObservedID(byte stateId)
        {
            m_observeStateId = stateId;
        }
        public void NotifyNewChange(in StateChange change)
        {
            m_isEnteredState = change.m_toStateIndex == m_observeStateId;
        }
        public void NotifyNewChange(in byte toState)
        {
            m_isEnteredState = toState == m_observeStateId;
        }

        public void GetCurrentState(out byte state)
        {
            state = m_observeStateId;
        }
        public void HasEnterWantedState(out bool hasEnter)
        {
            hasEnter = m_isEnteredState;
        }
    }
    [System.Serializable]
    public struct StateExitPassif
    {
        public byte m_observeStateId;
        public bool m_isExitingState;
        public void SetObservedID(byte stateId)
        {
            m_observeStateId = stateId;
        }
        public void NotifyNewChange(in StateChange change)
        {
            m_isExitingState = change.m_fromStateIndex == m_observeStateId;
        }
        public void NotifyNewChange(in byte fromState)
        {
            m_isExitingState = fromState == m_observeStateId;
        }
        public void GetCurrentState(out byte state)
        {
            state = m_observeStateId;
        }
        public void HasExitWantedState(out bool hasEnter)
        {
            hasEnter = m_isExitingState;
        }
    }
    [System.Serializable]
    public struct StateEnterFromMultiplePassif
    {
        public byte m_observeStateId;
        public byte[] m_fromStateIds;
        public bool m_isEnteredState;
        public void SetObservedID(byte stateId)
        {
            m_observeStateId = stateId;
        }
        public void SetOrigine(byte [] origineStateId)
        {
            m_fromStateIds = origineStateId;
        }
        public void NotifyNewChange(in StateChange change)
        {
            NotifyNewChange(in change.m_fromStateIndex, in change.m_toStateIndex);
        }
            public void NotifyNewChange(in byte fromStateIndex, in byte toStateIndex)
        {
            bool isGoodState = toStateIndex == m_observeStateId;
            if (!isGoodState)
            {
                m_isEnteredState = false;
                return;
            }
            bool isGoodOrigine = false;
            for (int i = 0; i < m_fromStateIds.Length; i++)
            {
                if (m_fromStateIds[i] == fromStateIndex)
                {
                    isGoodOrigine = true;
                    break;
                }

            }
            m_isEnteredState = isGoodOrigine && isGoodState;
        }
        public void GetCurrentState(out byte state)
        {
            state = m_observeStateId;
        }
        public void HasEnterWantedState(out bool hasEnter)
        {
            hasEnter = m_isEnteredState;
        }
    }
    [System.Serializable]
    public struct StateExitToMultiplePassif
    {
        public byte m_observeStateId;
        public byte[] m_toStateIds;
        public bool m_isExitingState;
        public void SetObservedID(byte stateId)
        {
            m_observeStateId = stateId;
        }
        public void SetDestination(byte[] destinationStateId)
        {
            m_toStateIds = destinationStateId;
        }
        public void NotifyNewChange(in StateChange change)
        {
            NotifyNewChange(in change.m_fromStateIndex, in change.m_toStateIndex);
        }
        public void NotifyNewChange(in byte fromStateIndex, in byte toStateIndex)
        {
            bool isGoodState = fromStateIndex == m_observeStateId;
            if (!isGoodState)
            {
                m_isExitingState = false;
                return;
            }
            bool isGoodDestination = false;
            for (int i = 0; i < m_toStateIds.Length; i++)
            {
                if (m_toStateIds[i] == toStateIndex)
                {
                    isGoodDestination = true;
                    break;
                }
            }
            m_isExitingState = isGoodDestination && isGoodState;
        }
        public void GetCurrentState(out byte state)
        {
            state = m_observeStateId;
        }
        public void HasExitWantedState(out bool hasEnter)
        {
            hasEnter = m_isExitingState;
        }
    }
  

}
