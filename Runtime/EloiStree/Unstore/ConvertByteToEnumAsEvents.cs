using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Eloi;
using System.Linq;


namespace Eloi { 

public class AbstractStateChangeToEnumMono<T> : MonoBehaviour,
    IStateChangeByteReceiver,
    IStateChangeReceiver,
    IStateChangeWithFineStateReceiver, IStateChangeByteWithFineStateReceiver
    where T : System.Enum
{
    public ByteStateChangeToEnumUtility<T> m_converterEnum = new ByteStateChangeToEnumUtility<T>();
    public UnityEventStateChangeEnum<T> m_eventRGB = new UnityEventStateChangeEnum<T>();
    public UnityEventStateChangeWithFineStateEnum<T> m_eventRGBWithSource = new UnityEventStateChangeWithFineStateEnum<T>();


    public void NotifyStateChange(byte fromState, byte toState)
    {

        m_converterEnum.GetEnumFromSafe(in fromState, in toState,
            out T fromResult, out T toResult, out bool succed);
        if (succed)
            m_eventRGB.Invoke(fromResult, toResult);
    }

    public void NotifyStateChangeFineState(StateChangeWithFineState stateChange)
    {
        m_converterEnum.GetEnumFromSafe(in stateChange.m_fromStateIndex, in stateChange.m_toStateIndex,
             out T fromResult, out T toResult, out bool hasConverted);
        if (hasConverted)
        {
            m_eventRGBWithSource.Invoke(stateChange.m_sourceRef, fromResult, toResult);
        }
    }

    public void NotifyStateChange(StateChange stateChange)
    {
        NotifyStateChange(stateChange.m_fromStateIndex, stateChange.m_toStateIndex);
    }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {
            m_converterEnum.GetEnumFromSafe(in fromState, in toState,
                out T fromResult, out T toResult, out bool succed);
            if (succed)
                m_eventRGBWithSource.Invoke(source,fromResult, toResult);
        }
    }

    public interface IStateChangeWithFineStateReceiver
    {
        void NotifyStateChangeFineState(StateChangeWithFineState stateChange);
    }
    public interface IStateChangeReceiver
    {
        void NotifyStateChange(StateChange stateChange);
    }
    public interface IStateChangeByteReceiver
    {
        void NotifyStateChange(byte fromState, byte toState);
    }
    public interface IStateChangeByteWithFineStateReceiver
    {
        void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState);
    }
    public interface IStateChangeEnumReceiver<T> where T : System.Enum
    {
        void NotifyStateChange(T fromState, T toState);
    }
    public interface IStateChangeEnumWithFineStateReceiver<T> where T : System.Enum
    {
        void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState);
    }
    public interface IStateChangeNameIdReceiver
    {
        void NotifyStateChange(string fromState, string toState);
    }
    public interface IStateChangeNameIdWithFineStateReceiver
    {
        void NotifyStateChange(IContainSFSMDeductedInfo sfsmSource, string fromState, string toState);
    }
    public interface IStateChangeFullReceiver : IStateChangeByteReceiver, IStateChangeReceiver, IStateChangeWithFineStateReceiver, IStateChangeNameIdReceiver
{}


[System.Serializable]
public class UnityEventStateChangeEnum<Y> : UnityEvent<Y, Y> where Y : System.Enum
{ }
[System.Serializable]
public class UnityEventStateChangeWithFineStateEnum<Y> : UnityEvent<IContainSFSMDeductedInfo, Y, Y> where Y : System.Enum
{ }



public abstract class AbstractStateChangeToEnum<T> where T : System.Enum
{

    public abstract void GetEnumFromSafe(in byte fromState, in byte toState,
        out T fromStateResult, out T toStateResult,
        out bool wasConvertable);
    public abstract void GetEnumFrom(in byte fromState, in byte toState,
        out T fromStateResult, out T toStateResult
       );
}

public class ByteStateChangeToEnumUtility<T> : AbstractStateChangeToEnum<T> where T : System.Enum
{
        public static ByteStateChangeToEnumUtility<T> m_instance = new ByteStateChangeToEnumUtility<T>();

        public static void StaticGetIsByteCouldBeEnum(in byte statusIndex, out bool couldBe)
        {
            m_instance.GetIsByteCouldBeEnum(in statusIndex, out couldBe);
        }
        public static void StaticGetEnumFrom(in byte fromState, in byte toState,
            out T fromStateResult, out T toStateResult
           )
        {
            m_instance.GetEnumFrom(in fromState, in toState, out fromStateResult, out toStateResult);
        }
        public static void StaticGetEnumFromSafe(in byte fromState, in byte toState,
            out T fromStateResult, out T toStateResult, out bool wasConvertable
           )
        {
            m_instance.GetEnumFromSafe(in fromState, in toState, out fromStateResult, out toStateResult, out wasConvertable);
        }



        public void GetIsByteCouldBeEnum(in byte statusIndex, out bool couldBe)
    {
        CheckThatEnumListIsBuild();
        couldBe = statusIndex < m_enumList.Length;
    }
    public override void GetEnumFrom(in byte fromState, in byte toState,
        out T fromStateResult, out T toStateResult
       )
    {
        GetTypeOfByte(in fromState, out fromStateResult);
        GetTypeOfByte(in toState, out toStateResult);
    }
    public override void GetEnumFromSafe(in byte fromState, in byte toState,
        out T fromStateResult, out T toStateResult, out bool wasConvertable
       )
    {
        Eloi.E_CodeTag.SideEffect.Info(
            "If you provide a enum with order that you customed, then my code don't work. Enum must be 012345...");

        GetIsByteCouldBeEnum(in fromState, out bool makeSenseFrom);
        GetIsByteCouldBeEnum(in toState, out bool makeSenseTo);
        wasConvertable = makeSenseFrom && makeSenseTo;
        if (!wasConvertable)
        {
            toStateResult = fromStateResult = GetFirstEnum();
            return;
        }
        GetTypeOfByte(in fromState, out fromStateResult);
        GetTypeOfByte(in toState, out toStateResult);
        wasConvertable = makeSenseFrom && makeSenseTo;
    }
    public static T[] m_enumList = null;
    public static void CheckThatEnumListIsBuild()
    {
        if (m_enumList == null)
        {
            m_enumList = Enum.GetValues(typeof(T))
               .Cast<T>().ToArray();
        }
    }
        public static void GetByteOf(in byte byteIndex,  out T value, out bool converted)
        {
            CheckThatEnumListIsBuild();

            if (byteIndex >= m_enumList.Length)
            {
                value = m_enumList[byteIndex];
                converted = true;

            }
            else
            {
                value = GetFirstEnum();
                converted = false;
            }
        }
        public static void GetBytesOf(in byte[] byteIndex, out T[] result, out bool converted)
        {
            converted = true;
            CheckThatEnumListIsBuild();
            result = new T[byteIndex.Length];
            for (int i = 0; i < byteIndex.Length; i++)
            {
                GetByteOf(in byteIndex[i], out result[i], out converted);
                if (!converted)
                    return;
            }
        }
        public static void GetBytesOf( in T[] enumValue, out byte[] result, out bool converted)
        {
            converted = true;
            CheckThatEnumListIsBuild();
            result = new byte[enumValue.Length];
            for (int i = 0; i < enumValue.Length; i++)
            {
                try { 
                GetByteOf(in enumValue[i] , out result[i]);
                }
                catch
                {
                    converted = false;
                    return;

                }
            }
        }

        private static T GetFirstEnum()
    {
        return m_enumList[0];
    }
        public static void GetTypeOfByte(in byte byteIndex, out T value)
        {
            CheckThatEnumListIsBuild();

            if (byteIndex < m_enumList.Length)
            {
                value = m_enumList[byteIndex];
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("You try to convert an enum that don't existe from a byte");
            }
        }
        public static void GetByteOf( in T value, out byte byteIndex)
        {
            CheckThatEnumListIsBuild();
            for (byte i = 0; i < m_enumList.Length; i++)
            {
                E_CodeTag.QualityAssurance.TestedState(E_CodeTag.QualityAssurance.TestedStateType.NotAtAll);
                if ( m_enumList[i].Equals( value) )
                {
                    byteIndex = i;
                    return;
                }
            }
            byteIndex = 0;
            
                throw new System.ArgumentOutOfRangeException("You try to convert an enum that don't existe from a byte");
            
        }

        public static void StaticGetByteEnumFromSafe(T fromState, T toState, out byte fromStateResult, out byte toStateResult, out bool wasConveted)
        {
            try
            {
                GetByteOf(in fromState, out fromStateResult);
                GetByteOf(in toState, out toStateResult);
                wasConveted = true;
                return;
            }
            catch (Exception)
            {

            }
            fromStateResult = 0;
            toStateResult = 0;
            wasConveted = false;
        }
        public static void StaticGetByteEnumFrom(T fromState, T toState, out byte fromStateResult, out byte toStateResult)
        {
            
                GetByteOf(in fromState, out fromStateResult);
                GetByteOf(in toState, out toStateResult);
            
        }



        public static void GetAllEnum(out T [] all)
        {
            CheckThatEnumListIsBuild();
            all = m_enumList;
        }
    }
}
