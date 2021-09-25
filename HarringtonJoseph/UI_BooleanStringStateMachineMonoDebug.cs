using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UI_BooleanStringStateMachineMonoDebug : MonoBehaviour
{
    public BooleanStringStateMachineMono m_source;
    public Text m_currentState;
    public Text m_sourceTransitionsLog;
    public Text m_destinationTransitionsLog;
    public Text m_allStateTransitionsLog;
    public RawImage m_imageDebugger;
    public StateToTexture[] m_stateToImage;

    
    void Update()
    {
        RefreshStateUI();
    }

    private void RefreshStateUI()
    {
        m_source.GetCurrentState(out string state);
        if (m_currentState != null)
        {
            m_currentState.text = state;
        }
        if (m_sourceTransitionsLog != null)
        {
            m_source.GetAllTransitionSourceOf(in state,
                out IEnumerable< StringTransaction> ts);
            m_sourceTransitionsLog.text =
                string.Join("\n",ts.Select(k => GetDescriptionOf(k)) );
        }
        if (m_destinationTransitionsLog != null)
        {
            m_source.GetAllTransitionDestinationOfState(in state,
                 out IEnumerable<StringTransaction> ts);
            m_destinationTransitionsLog.text =
                string.Join("\n", ts.Select(k => GetDescriptionOf(k)));
        }
        if (m_allStateTransitionsLog != null)
        {
            m_source.GetAllTransitionLinkedToState(in state,
                 out IEnumerable<StringTransaction> ts);
            m_allStateTransitionsLog.text =
                string.Join("\n", ts.Select(k => GetDescriptionOf(k)));
        }

        if (m_imageDebugger != null) { 
            for (int i = 0; i < m_stateToImage.Length; i++)
            {
                if (m_stateToImage[i].m_stateName.ToLower().Trim() == state.ToLower().Trim()) {
                    m_imageDebugger.texture = m_stateToImage[i].m_debugImage;
                }
            }
        }


    }

    private string GetDescriptionOf(StringTransaction k)
    {
        return string.Format("{0}>{1}>{2}", k.m_transactionSource, k.m_transactionTriggerName, k.m_transactionDestination);
    }

   

    [System.Serializable]
    public class StateToTexture {
        public string m_stateName;
        public Texture2D m_debugImage;
    }
}

