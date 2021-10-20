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
[System.Serializable]
public class StateChangeIndexWithFineStateStructEvent : UnityEvent<StateChangeWithFineState> { }
[System.Serializable]
public class StateChangeIndexWithFineStateEvent : UnityEvent<IContainSFSMDeductedInfo, byte, byte> { }
[System.Serializable]
public class StateChangeIndexWithFineStateEnumEvent <T>: UnityEvent<IContainSFSMDeductedInfo, T, T> where T:System.Enum
{ }