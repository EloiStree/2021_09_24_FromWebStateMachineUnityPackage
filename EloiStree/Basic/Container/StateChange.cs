using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Eloi {

    [System.Serializable]
    public struct StateChange 
    {
        public byte m_fromStateIndex;
        public byte m_toStateIndex;

        public void SetWith(in byte fromStateIndex, in byte toStateIndex)
        {
            m_fromStateIndex = fromStateIndex;
            m_toStateIndex = toStateIndex;
        }

        public void SetWith(in StateChange change)
        {
            m_fromStateIndex = change.m_fromStateIndex;
            m_toStateIndex = change.m_toStateIndex;
        }
    }


    [System.Serializable]
    public struct StateChangeValide
    {
        public byte m_fromStateIndex;
        public byte m_toStateIndex;
        public byte m_transactionIndex;

        public void SetWith(in byte fromStateIndex, in byte toStateIndex, in byte transactionIndex)
        {
            m_fromStateIndex = fromStateIndex;
            m_toStateIndex = toStateIndex;
            m_transactionIndex = transactionIndex;
        }
    }

    [System.Serializable]
    public struct StateChangeForced
    {
        public byte m_fromStateIndex;
        public byte m_toStateIndex;

        public void SetWith(in byte fromStateIndex, in byte toStateIndex)
        {
            m_fromStateIndex = fromStateIndex;
            m_toStateIndex = toStateIndex;
        }
    }


    public class StateChangeUtility
    {

        public void CreateStateChange(in byte fromStateIndex,
            in byte toStateIndex, 
            out StateChange change)
        {
            change = new StateChange();
            change.SetWith(in fromStateIndex, in toStateIndex);
        }

    }
}

