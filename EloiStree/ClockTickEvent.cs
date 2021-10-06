using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClockTickEvent : MonoBehaviour
{
    public float m_tickTime=0.5f;
    public UnityEvent m_tick;

    IEnumerator Start()
    {
        while (true) {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_tickTime);
            m_tick.Invoke();

        }
    }

}
