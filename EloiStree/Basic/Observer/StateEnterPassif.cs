using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi {

    [System.Serializable]
    public struct StateEnterPassif
    {
        public byte m_observeStateId;
        public bool m_isEnteredState;
        public void SetObservedID(byte stateId)
        {
            m_observeStateId = stateId;
        }
        public void NotifyNewChange(in StateChange change)
        {
            m_isEnteredState = change.m_toStateIndex == m_observeStateId;
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
            bool isGoodState = change.m_toStateIndex == m_observeStateId;
            if (!isGoodState)
            {
                m_isEnteredState = false;
                return;
            }
            bool isGoodOrigine = false;
            for (int i = 0; i < m_fromStateIds.Length; i++)
            {
                if (m_fromStateIds[i] == change.m_fromStateIndex)
                {
                    isGoodOrigine = true;
                    break;
                }

            }
            m_isEnteredState = isGoodOrigine && isGoodState;
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
            bool isGoodState = change.m_fromStateIndex == m_observeStateId;
            if (!isGoodState) { 
                m_isExitingState = false;
                return;
            }
            bool isGoodDestination = false;
            for (int i = 0; i < m_toStateIds.Length; i++)
            {
                if (m_toStateIds[i] == change.m_toStateIndex)
                {
                    isGoodDestination = true;
                    break;
                }
            }
            m_isExitingState = isGoodDestination && isGoodState;
        }

    }

}
