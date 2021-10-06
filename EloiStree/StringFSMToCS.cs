using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StringFSMToCS : MonoBehaviour
{
    public StringFSMDeductedScriptable m_source;

    [TextArea(0, 6)]
    public string m_asEnums;
    [TextArea(0, 20)]
    public string m_asConst;
    [TextArea(0, 20)]
    public string m_asConstCallClass;
    [TextArea(0, 20)]
    public string m_asInterface;

    [TextArea(0, 20)]
    public string m_concatOfAll;


    private void OnValidate()
    {
        StringTransaction[] transitions = m_source.m_source.m_stringFSM.m_transactions.m_transactions;
        string[] states = m_source.m_source.m_stringFSM.m_states.m_states;
        string[] uniqueTransitions = m_source.m_transactionsCollision.m_uniqueTransitions;

        GenerateEnums(in transitions, states);
        GenerateConst(in transitions, states);
        GenerateConstCallClass(in uniqueTransitions, states);
        GenerateCOnstInterface(in uniqueTransitions, states);
        m_concatOfAll = string.Join("\n\n//--------------------\n\n", new string[] {
        m_asEnums,
        m_asInterface,
        m_asConst,
        m_asConstCallClass,
        });



        /// Generate methode and const 
        /// m_source.m_source.m_stringFSM.m_machineName


    }

    private void GenerateCOnstInterface(in string [] transitionsDistinct, string[] states)
    {
        m_asInterface = "";
        string machineNameUp = EnumCompress(m_source.m_source.m_stringFSM.m_machineName.ToUpper());
        // Reminder:     public enum A : byte { A = 0, B = 1, C = 4 }

        string[] stateInput = states.Select(k => k.ToUpper()).ToArray();
        m_asInterface += "\n\npublic interface SFSM_I" + machineNameUp + " {\n";
        for (int i = 0; i < stateInput.Length; i++)
        {
            m_asInterface += "\tvoid ForceChangeTo" + stateInput[i] + "();\n";
        }
        m_asInterface += "\t\n\n";
        string[] transcationInput = transitionsDistinct.Select(k => k.ToUpper()).ToArray();
        for (int i = 0; i < transcationInput.Length; i++)
        {
            m_asInterface += "\tvoid TryTransitionThrow" + transcationInput[i] + "();\n";
        }

        m_asInterface += "\t\n\n";
        m_asInterface += "}\n";
    }

    private void GenerateConstCallClass(in string[] transitions, string[] states)
    {

        m_asConstCallClass = "";
        string machineNameUp = EnumCompress(m_source.m_source.m_stringFSM.m_machineName.ToUpper());
        // Reminder:     public enum A : byte { A = 0, B = 1, C = 4 }

        string[] stateInput = states.Select(k => k.ToUpper()).ToArray();
        m_asConstCallClass += "\n\npublic class SFSM_" + machineNameUp + " : SFSM_I"+ machineNameUp +" {\n";
        for (int i = 0; i < stateInput.Length; i++)
        {
            m_asConstCallClass += "\tpublic void ForceChangeTo" + stateInput[i]  +"(){\n";
            m_asConstCallClass += "\t//Add Your code here\n";
            m_asConstCallClass += "\tthrow new System.NotImplementedException();\n";
            m_asConstCallClass += "\t}\n";
        }
        m_asInterface += "\t\n\n";
        string[] transcationInput = transitions.Select(k => k.ToUpper()).ToArray();
        for (int i = 0; i < transcationInput.Length; i++)
        {
            m_asConstCallClass += "\tpublic  void TryTransitionThrow" + transcationInput[i] + "(){\n";
            m_asConstCallClass += "\t//Add Your code here\n";
            m_asConstCallClass += "\tthrow new System.NotImplementedException();\n";
            m_asConstCallClass += "\t}\n";
        }


        m_asConstCallClass += "\t\n\n";
        m_asConstCallClass += "}\n";

    }

    private void GenerateEnums(in StringTransaction[] transitions, string[] states)
    {

        // Reminder:     public enum A : byte { A = 0, B = 1, C = 4 }

        string[] stateInput = states.Select(k => k.ToUpper()).ToArray();
        for (int i = 0; i < stateInput.Length; i++)
        {
            stateInput[i] += " = " + i;
        }

        m_asEnums = "\npublic enum S_" + EnumCompress(m_source.m_source.m_stringFSM.m_machineName.ToUpper())
            + " : byte {" + string.Join(", ", stateInput) + "}";

        string[] transcationInput = transitions.Select(k =>
        RemoveSpace(k.m_transactionTriggerName.ToUpper())
        + "_" +
        RemoveSpace(k.m_transactionSource.ToUpper())
        + "_" +
        RemoveSpace(k.m_transactionDestination.ToUpper())
        ).ToArray();
        for (int i = 0; i < transcationInput.Length; i++)
        {
            transcationInput[i] += " = " + i;
        }

        m_asEnums += "\npublic enum T_" + EnumCompress(m_source.m_source.m_stringFSM.m_machineName.ToUpper())
            + " : byte{" + string.Join(", ", transcationInput) + "}";
    }

    public const byte VALUE_d = 5;
    private void GenerateConst(in StringTransaction[] transitions, string[] states)
    {
        m_asConst = "";
        string machineNameUp = EnumCompress(m_source.m_source.m_stringFSM.m_machineName.ToUpper());
        // Reminder:     public enum A : byte { A = 0, B = 1, C = 4 }

        string[] stateInput = states.Select(k => k.ToUpper()).ToArray();
        m_asConst += "//*";
        m_asConst += "\n\npublic partial class SFSM {\n";
        for (int i = 0; i < stateInput.Length; i++)
        {
            m_asConst += "\tpublic const byte S_" + machineNameUp + "_" + stateInput[i] + " = " + i + ";\n";
        }

        string[] transcationInput = ConcatTransactionInfoAndUp(transitions);
        for (int i = 0; i < transcationInput.Length; i++)
        {
            m_asConst += "\tpublic const byte T_" + machineNameUp + "_" + transcationInput[i] + " = " + i + ";\n";
        }
        m_asConst += "}\n";

        m_asConst += "//*/";


        m_asConst += "\n\n";
        m_asConst += "//*";
        m_asConst += "\n\npublic partial class SFSM {\n";
        m_asConst += "\n\tpublic static class " + machineNameUp + "{\n";

        for (int i = 0; i < stateInput.Length; i++)
        {
            m_asConst += "\t\tpublic const byte S_" + stateInput[i] + " = " + i + ";\n";
        }


        for (int i = 0; i < transcationInput.Length; i++)
        {
            m_asConst += "\t\tpublic const byte T_" + transcationInput[i] + " = " + i + ";\n";
        }

        m_asConst += "\t}\n";
        m_asConst += "}\n";
        m_asConst += "//*/";


    }

    private string[]  ConcatTransactionInfoAndUp(in StringTransaction[] transitions)
    {
        string[] transcationInput = transitions.Select(k =>
        RemoveSpace(k.m_transactionTriggerName.ToUpper())
        + "_" +
        RemoveSpace(k.m_transactionSource.ToUpper())
        + "_" +
        RemoveSpace(k.m_transactionDestination.ToUpper())
        ).ToArray();
        return transcationInput;
    }

    private string RemoveSpace(string value)
    {
        return value.Replace(" ", "");
    }

    private string EnumCompress(string value)
    {
        return value.Replace(" ", "");
    }
}
