using Eloi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void StateChangeIndexDelegate(in byte fromStateIndex, in byte toStateIndex);
public delegate void StateChangeIndexStuctDelegate(in StateChange stateChange);
[System.Serializable]
public class StateChangeFromToIndexEvent : UnityEvent<byte, byte> { }
[System.Serializable]
public class StateChangeIndexStuctEvent : UnityEvent<StateChange> { }