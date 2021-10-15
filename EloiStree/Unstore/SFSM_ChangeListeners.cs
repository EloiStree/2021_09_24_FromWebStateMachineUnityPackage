using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eloi {



    public class SFSMChangeListener_AnyEnterExit<T> : MonoBehaviour
        , IStateChangeByteWithFineStateReceiver
        , IStateChangeEnumWithFineStateReceiver<T>
        where T : System.Enum
    {
        public T m_observed;
        [Header("Enter")]
        public StateChangeIndexWithFineStateEvent m_enterEvent;
        public SpecificStateChangeEvent m_enterEnumEvent;
        [Header("Exit")]
        public StateChangeIndexWithFineStateEvent m_exitEvent;
        public SpecificStateChangeEvent m_exitEnumEvent;


        [System.Serializable]
        public class SpecificStateChangeEvent : StateChangeIndexWithFineStateEnumEvent<T>
        { }
        public BehindTheScene m_behindTheScene;
        [System.Serializable]
        public class BehindTheScene
        {
            public StateEnterPassif m_enter = new StateEnterPassif();
            public StateExitPassif m_exit = new StateExitPassif();
            public T m_previous, m_current;
        }

        private void Awake()
        {
            RefreshStateObserved();
        }

        private void OnValidate()
        {
            RefreshStateObserved();
        }

        private void RefreshStateObserved()
        {
            ByteStateChangeToEnumUtility<T>.GetByteOf(in m_observed, out byte value);
            m_behindTheScene.m_enter.SetObservedID(value);
            m_behindTheScene.m_exit.SetObservedID(value);
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {
            m_behindTheScene.m_exit.NotifyNewChange(fromState);
            m_behindTheScene.m_exit.HasExitWantedState(out bool newExit);
            if (newExit)
                m_exitEvent.Invoke(source, fromState, toState);

            ByteStateChangeToEnumUtility<T>.StaticGetEnumFromSafe(in fromState, in toState,
                   out T fS, out T tS, out bool converted);
            if (converted)
            {
                m_behindTheScene.m_previous = fS;
                m_behindTheScene.m_current = tS;
                if (newExit)
                    m_enterEnumEvent.Invoke(source, m_behindTheScene.m_previous, m_behindTheScene.m_current);
            }

            m_behindTheScene.m_enter.NotifyNewChange(toState);
            m_behindTheScene.m_enter.HasEnterWantedState(out bool newEnter);
            if (newEnter)
                m_enterEvent.Invoke(source, fromState, toState);

            ByteStateChangeToEnumUtility<T>.StaticGetEnumFromSafe(in fromState, in toState,
                   out fS, out tS, out converted);
            if (converted)
            {
                m_behindTheScene.m_previous = fS;
                m_behindTheScene.m_current = tS;
                if (newEnter)
                    m_enterEnumEvent.Invoke(source, m_behindTheScene.m_previous, m_behindTheScene.m_current);
            }
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState)
        {
            ByteStateChangeToEnumUtility<T>.StaticGetByteEnumFromSafe(fromState, toState, out byte fState, out byte tState, out bool wasConveted);
            if (wasConveted)
            {
                NotifyStateChange(source, fState, tState);
                m_behindTheScene.m_previous = fromState;
                m_behindTheScene.m_current = toState;
            }
        }

        //public void NotifyStateChange(IContainSFSMDeductedInfo source, S_RGBSTATEMACHINE fromState, S_RGBSTATEMACHINE toState)
        //{
        //    NotifyStateChange(source, (byte)fromState, (byte)toState);
        //}

    }



    public class SFSMChangeListener_FromEnter<T> : MonoBehaviour
        , IStateChangeByteWithFineStateReceiver
        , IStateChangeEnumWithFineStateReceiver<T>
        where T : System.Enum
    {
        public T m_observed;
        public T[] m_fromTracked;
        [Header("N To Enter")]
        public StateChangeIndexWithFineStateEvent m_enterEvent;
        public SpecificStateChangeEvent m_enterEnumEvent;
        [System.Serializable]
        public class SpecificStateChangeEvent : StateChangeIndexWithFineStateEnumEvent<T>
        { }
        public BehindTheScene m_behindTheScene;

        [System.Serializable]
        public class BehindTheScene
        {
            public StateEnterFromMultiplePassif m_enter = new StateEnterFromMultiplePassif();
            public T m_previous, m_current;
        }

        private void Awake()
        {
            RefreshStateObserved();
        }

        private void OnValidate()
        {
            RefreshStateObserved();
        }

        private void RefreshStateObserved()
        {
            ByteStateChangeToEnumUtility<T>.GetByteOf(in m_observed, out byte value);
            m_behindTheScene.m_enter.SetObservedID(value);
            ByteStateChangeToEnumUtility<T>.GetBytesOf(in m_fromTracked, out byte[] values, out bool convert);
            if (convert)
                m_behindTheScene.m_enter.SetOrigine(values);
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {

            m_behindTheScene.m_enter.NotifyNewChange(in fromState, in toState);
            m_behindTheScene.m_enter.HasEnterWantedState(out bool hasEnter);
            if (hasEnter)
            {
                m_enterEvent.Invoke(source, fromState, toState);
            }
            ByteStateChangeToEnumUtility<T>.StaticGetEnumFromSafe(in fromState, in toState,
                   out T fS, out T tS, out bool converted);
            if (converted)
            {
                m_behindTheScene.m_previous = fS;
                m_behindTheScene.m_current = tS;
                if (hasEnter)
                    m_enterEnumEvent.Invoke(source, m_behindTheScene.m_previous, m_behindTheScene.m_current);
            }
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState)
        {
            ByteStateChangeToEnumUtility<T>.StaticGetByteEnumFromSafe(fromState, toState, out byte fState, out byte tState, out bool wasConveted);
            if (wasConveted)
            {
                NotifyStateChange(source, fState, tState);
                m_behindTheScene.m_previous = fromState;
                m_behindTheScene.m_current = toState;
            }
        }


    }


    public class SFSMChangeListener_ExitTo<T> : MonoBehaviour
        , IStateChangeByteWithFineStateReceiver
        , IStateChangeEnumWithFineStateReceiver<T>
        where T : System.Enum
    {
        public T m_observed;
        public T[] m_toTracked;
        [Header("Exit To N")]
        public StateChangeIndexWithFineStateEvent m_exitEvent;
        public SpecificStateChangeEvent m_exitEnumEvent;
        [System.Serializable]
        public class SpecificStateChangeEvent : StateChangeIndexWithFineStateEnumEvent<T>
        { }
        public BehindTheScene m_behindTheScene;

        [System.Serializable]
        public class BehindTheScene
        {
            public StateExitToMultiplePassif m_exit = new StateExitToMultiplePassif();
            public T m_previous, m_current;
        }

        private void Awake()
        {
            RefreshStateObserved();
        }

        private void OnValidate()
        {
            RefreshStateObserved();
        }

        private void RefreshStateObserved()
        {
            ByteStateChangeToEnumUtility<T>.GetByteOf(in m_observed, out byte value);
            m_behindTheScene.m_exit.SetObservedID(value);
            ByteStateChangeToEnumUtility<T>.GetBytesOf(in m_toTracked, out byte[] values, out bool convert);
            if (convert)
            {
                m_behindTheScene.m_exit.SetDestination(values);
            }
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {
            m_behindTheScene.m_exit.NotifyNewChange(in fromState, in toState);
            m_behindTheScene.m_exit.HasExitWantedState(out bool hasExit);
            if (hasExit)
            {

                m_exitEvent.Invoke(source, fromState, toState);

            }
            ByteStateChangeToEnumUtility<T>.StaticGetEnumFromSafe(in fromState, in toState,
                   out T fS, out T tS, out bool converted);
            if (converted)
            {
                m_behindTheScene.m_previous = fS;
                m_behindTheScene.m_current = tS;
                if (hasExit)
                    m_exitEnumEvent.Invoke(source, m_behindTheScene.m_previous, m_behindTheScene.m_current);
            }
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, T fromState, T toState)
        {
            ByteStateChangeToEnumUtility<T>.StaticGetByteEnumFromSafe(fromState, toState, out byte fState, out byte tState, out bool wasConveted);
            if (wasConveted)
            {
                m_behindTheScene.m_previous = fromState;
                m_behindTheScene.m_current = toState;
                NotifyStateChange(source, fState, tState);
            }
        }


    }
}