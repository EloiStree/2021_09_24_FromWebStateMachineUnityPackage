using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(FiniteStateMachineBuilder))]
public class FiniteStateMachineBuilderEditor :Editor
{
    public string m_urlStateBuilder = "";

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("Quick FSM")){
            Application.OpenURL("http://madebyevan.com/fsm/");
        }
    }
}