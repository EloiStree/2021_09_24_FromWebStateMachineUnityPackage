using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnitySFSM_Transition : UnityEvent<StringTransaction> { }

[System.Serializable]
public class UnitySFSM_TransitionIndex : UnityEvent<uint> { }

[System.Serializable]
public class UnitySFSM_State2StateName : UnityEvent<string, string> { }

[System.Serializable]
public class UnitySFSM_State2StateIndex : UnityEvent<uint, uint> { }



public delegate void TransitionRequestFail(in string transitionName, in string whatHappened);
public delegate void TransitionRequestSuccess(in string transitionName, in string sourceStateName, in string destinationStateName);
public delegate void TransitionRequestFailId(in uint transitionIdIndex, in string whatHappened);
public delegate void TransitionRequestSuccessId(in uint transitionIdIndex, in uint sourceStateIdIndex, in uint destinationStateIdIndex);


public delegate void ForceStateChangeRequestFail(in string stateName, in string whatHappened);
public delegate void ForceStateChangeRequestSuccess(in string stateName, in string sourceStateName, in string destinationStateName);
public delegate void ForceStateChangeRequestFailId(in uint stateIdIndex, in string whatHappened);
public delegate void ForceStateChangeRequestSuccessId(in uint stateIdIndex, in uint sourceStateIdIndex, in uint destinationStateIdIndex);

