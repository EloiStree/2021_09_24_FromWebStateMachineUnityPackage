using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi;


namespace Eloi { 
    public class SFSMChangeListener_AnyEnterExit : MonoBehaviour, IStateChangeByteWithFineStateReceiver
    {
   
        public StateChangeIndexWithFineStateEvent m_enterEvent;
        public StateChangeIndexWithFineStateEvent m_exitEvent;

        public BehindTheScene m_behindTheScene;
        [System.Serializable]
        public class BehindTheScene
        {
            public StateEnterPassif m_enterPassif = new StateEnterPassif();
            public StateExitPassif m_enterExit = new StateExitPassif();
        }

        public void NotifyStateChange(IContainSFSMDeductedInfo source, byte fromState, byte toState)
        {
            m_behindTheScene.m_enterExit.NotifyNewChange(fromState);
            m_behindTheScene.m_enterExit.HasExitWantedState(out bool newExit);
            if (newExit)
                m_exitEvent.Invoke(source, fromState, toState);

            m_behindTheScene.m_enterPassif.NotifyNewChange(toState);
            m_behindTheScene.m_enterPassif.HasEnterWantedState(out bool newEnter);
            if (newEnter)
                m_enterEvent.Invoke(source, fromState, toState);
        }

    
    }
}
