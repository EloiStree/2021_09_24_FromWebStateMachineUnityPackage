using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLToStringFSMMono : MonoBehaviour
{

    
    public const string m_instructin = "Use Context Menu To Apply";
    public StringFSMScriptable m_toAffect;
    [TextArea(0, 10)]
    public string m_smallText;



    public void OnValidate()
    {
        if (m_toAffect != null)
        {
            StringFSM2XML.Convert(in m_smallText, out bool succed, ref m_toAffect);
        }
    }
}
