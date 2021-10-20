using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

public class StringFSMToXMLMono : MonoBehaviour
{
    //https://www.codeproject.com/Articles/2063/XML-Finite-State-Machine-in-C

    public const string m_instructin = "Use Context Menu To Apply";
    public StringFSMScriptable m_source;
    [TextArea(0, 10)]
    public string m_xmlResult;

    [TextArea(0, 10)]
    public string m_smallText;

    public void OnValidate()
    {
        if (m_source != null) { 
            StringFSM2XML.Convert(in m_source, out m_xmlResult);
            StringFSM2SmallText.Convert(in m_source, out m_smallText);
        }
    }

}

public class StringFSM2SmallText {

    public static void Convert(in StringFSMScriptable toConvert,  out string smalltext)
    {
        string[] states = toConvert.m_stringFSM.m_states.m_states;
        smalltext = "";
        smalltext += "name:" + toConvert.m_stringFSM.m_machineName;
        smalltext += "\ninit:" + toConvert.m_stringFSM.m_initialState;
        smalltext += "\nstates:" +string.Join(",", toConvert.m_stringFSM.m_states.m_states);

        for (int i = 0; i < states.Length; i++)
        {
            for (int j = 0; j < toConvert.m_stringFSM.m_transactions.m_transactions.Length; j++)
            {
                if (toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionSource.Length == states[i].Length
                    &&
                    toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionSource.IndexOf(states[i]) == 0)
                {
                    smalltext += string.Format("\n{0}>{1}>{2}"
                        , toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionSource
                        , toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionTriggerName
                        , toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionDestination);
                }
            }
        }
    }
    public static void Convert(in string smalltext, out bool succed, ref  StringFSMScriptable toConvert)
    {
        if (toConvert == null) {
            succed = false;
            return;
        }

        string machine="", init="";
        string[] states= new string[0];
        List<StringTransaction > transactions = new List<StringTransaction>();
        foreach (string line in smalltext.Split('\n'))
        {
            string l = line.Trim().ToLower();
            if (l.IndexOf("name:") == 0) {
                machine = l.Substring("name:".Length);
            }
            else if (l.IndexOf("init:") == 0)
            {
                init = l.Substring("init:".Length);
            }
            else if (l.IndexOf("ini:") == 0)
            {
                init = l.Substring("ini:".Length);
            }
            else if (l.IndexOf("state:") == 0 || l.IndexOf("states:") == 0  )
            {
                int doubleDot = l.IndexOf(":");
                if (doubleDot >=0)
                {
                    l = l.Substring(doubleDot+1);
                    states =l.Split(',');
                }

            }
            else {
                string [] tokensLine = l.Split('>');
                if (tokensLine.Length == 3) {
                    StringTransaction t = new StringTransaction();
                    t.m_transactionSource = tokensLine[0];
                    t.m_transactionTriggerName = tokensLine[1];
                    t.m_transactionDestination = tokensLine[2];
                    transactions.Add(t);
                }
            }
        }
        StringExistingStates ss = new StringExistingStates();
        ss.InitWith(states);
        StringExistingTransactions st = new StringExistingTransactions();
        st.InitWith(transactions.ToArray());
        BooleanStringStateMachine newFSM = new BooleanStringStateMachine(machine, init, ss, st);

        toConvert.m_stringFSM = newFSM;
        toConvert.TrimAndLowerCaseText();
//#if UNITY_EDITOR
//        EditorUtility.SetDirty(toConvert);
//        AssetDatabase.SaveAssets();
//        AssetDatabase.Refresh();
//#endif
        succed = true;
    }
}


public class StringFSM2XML {

    public static void Convert(in StringFSMScriptable toConvert, out string xml)
    {
        string[] states = toConvert.m_stringFSM.m_states.m_states;

        xml = "<?xml version=\"1.0\" ?>";
        xml += "\n<fsm name=\"" + toConvert.m_stringFSM.m_machineName + "\" initialState=\"" + toConvert.m_stringFSM.m_initialState + "\" >";
        xml += "\n\t<states>";
        for (int i = 0; i < states.Length; i++)
        {
            xml += "\n\t\t<state name=\"" + states[i]+ "\">";

            for (int j = 0; j < toConvert.m_stringFSM.m_transactions.m_transactions.Length ; j++)
            {
                if (toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionSource.Length==states[i].Length
                    &&
                    toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionSource.IndexOf(states[i])==0) {
                    xml += string.Format("\n\t\t\t<transition name=\"{0}\" nextState =\"{1}\"  />"
                        , toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionTriggerName
                        , toConvert.m_stringFSM.m_transactions.m_transactions[j].m_transactionDestination);
                }
            }
          
            xml += "\n\t\t</state>";
        }


        xml += "\n\t</states>";
        xml += "\n</fsm>";

    }
    public static void Convert(in string xml,out bool succed, ref StringFSMScriptable toConvert )
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(StringFSMAsXMLState.Fsm));
            using (StringReader reader = new StringReader(xml))
            {
                var test = (StringFSMAsXMLState.Fsm)serializer.Deserialize(reader);

                toConvert.m_stringFSM.m_machineName = test.Name;
                toConvert.m_stringFSM.m_initialState = test.InitialState;
                toConvert.m_stringFSM.m_states.InitWith( test.States.State.Select(k=>k.Name).ToArray());

                List<StringTransaction> transactions = new List<StringTransaction>();
                foreach (var state in test.States.State)
                {
                    foreach (var transaction in state.Transition)
                    {
                        StringTransaction t = new StringTransaction();
                        t.InitWith(state.Name.Trim().ToLower(), transaction.Name.Trim().ToLower(), transaction.NextState.Trim().ToLower());
                        transactions.Add(t);

                    }

                }
                toConvert.m_stringFSM.m_transactions.m_transactions =transactions.ToArray();
                toConvert.TrimAndLowerCaseText();

                succed = true;
            }
        }
        catch (Exception e) {

            Debug.Log("XML Convertion Failed:"+e.StackTrace);
        }

        succed = false;

    }



    public class StringFSMAsXMLState {
        // converter source/ used: https://json2csharp.com/xml-to-csharp


        [XmlRoot(ElementName = "transition")]
        public class Transition
        {

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "nextState")]
            public string NextState { get; set; }
        }

        [XmlRoot(ElementName = "state")]
        public class State
        {

            [XmlElement(ElementName = "transition")]
            public List<Transition> Transition { get; set; }

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }
        }

        [XmlRoot(ElementName = "states")]
        public class States
        {

            [XmlElement(ElementName = "state")]
            public List<State> State { get; set; }
        }

        [XmlRoot(ElementName = "fsm")]
        public class Fsm
        {

            [XmlElement(ElementName = "states")]
            public States States { get; set; }

            [XmlAttribute(AttributeName = "name")]
            public string Name { get; set; }

            [XmlAttribute(AttributeName = "initialState")]
            public string InitialState { get; set; }
        }
    }


}

/*
 
 <?xml version="1.0" ?>

<fsm name="Vending Machine">
    <states>
        <state name="start">
            <transition input="nickel" next="five" />
            <transition input="dime" next="ten" />
            <transition input="quarter" next="start" action="dispense" />
        </state>
        <state name="five">
            <transition input="nickel" next="ten" />
            <transition input="dime" next="fifteen" />
            <transition input="quarter" next="start" action="dispense" />
        </state>
        <state name="ten">
            <transition input="nickel" next="fifteen" />
            <transition input="dime" next="twenty" />
            <transition input="quarter" next="start" action="dispense" />
        </state>
        <state name="fifteen">
            <transition input="nickel" next="twenty" />
            <transition input="dime" next="start" action="dispense" />
            <transition input="quarter" next="start" action="dispense" />
        </state>
        <state name="twenty">
            <transition input="nickel" next="start" action="dispense" />
            <transition input="dime" next="start" action="dispense" />
            <transition input="quarter" next="start" action="dispense" />
        </state>
    </states>
</fsm>
 * */