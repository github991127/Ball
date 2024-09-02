using System.Collections.Generic;
using Script.Manager.StateManager;
using UnityEngine;

namespace Script.Level.SpecialInteraction.Twomix.TwomixLevelState
{
    public class TwomixLevelStateManager:StateManager
    {
        public TwomixLevelBlackboard Blackboard=>(TwomixLevelBlackboard)blackboard;
        public TwomixLevelStateManager(BaseBlackboard blackboard) : base(blackboard)
        {
            stateDic = new Dictionary<object, BaseState>();
            AddLevel(new TwomixDefaultLevel(this));
            AddLevel(new TwomixReversedLevel(this));
            SwitchState();
            EventManager.Instance.AddEvent(ClientEvent.Goal,SwitchState);
        }

        private void SwitchState()
        {
            if (curState != null)
            {
                curState.OnExit();
                if (Blackboard.CurConfig)
                {
                    curState = stateDic[Blackboard.CurConfig.LevelType];
                    ((TwomixBaseLevel)curState).OnEnter();
                }
            }
            else
            {
                curState = stateDic[Blackboard.CurConfig.LevelType];
                ((TwomixBaseLevel)curState).OnStart();
            }
        }

        public void OnDestroy()
        {
            EventManager.Instance.RemoveEvent(ClientEvent.Goal);
        }
    }
}