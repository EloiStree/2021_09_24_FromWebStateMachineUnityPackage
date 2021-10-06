using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IContainSFSMBasicInfo
{
    public void GetFSMBasicScriptable(out StringFSMScriptable basicFSM);
}

public interface IContainSFSMDeductedInfo {

    public void GetFSMDeducted(out StringFSMDeductedScriptable deductedFSM);
}


public interface IContaintStateAsByte {
    public void GetStateAsByte(out byte stateIndex);
}

public interface IContainStateChangeIndex
{
    public void Get(out byte fromIndex, out byte toIndex);
}
public interface IContainLastTransactionIndex
{
    public void Get(out byte transactionIndex);
}


public abstract class AbstractContainStateChangeIndexMono : MonoBehaviour, IContainStateChangeIndex
{
    public abstract void Get(out byte fromIndex, out byte toIndex);
}

public interface IMonoContaintStateChangeIndex {

    public void Get(out IContainStateChangeIndex stateChangeHolder);
}