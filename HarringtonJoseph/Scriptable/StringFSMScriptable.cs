using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StringFSMBasic", menuName = "ScriptableObjects/String FSM/SFSM Basic", order = 1)]
public class StringFSMScriptable : ScriptableObject
{
    public BooleanStringStateMachine m_stringFSM;

    [ContextMenu("Apply Trim And Lower the case")]
    public void TrimAndLowerCaseText()
    {

        StringFSMDeductionUtility.TrimAndLowCaseAll(ref m_stringFSM);
    }

    [ContextMenu("Check For obvious mistake")]
    public void CheckForObviousHumanMistake() { 
    
        //Transition can't pointe at not existing state

        //They can't be two state with the same name


        //Check for dyslexian mistakes where a word is very close to an other one.
    }
}