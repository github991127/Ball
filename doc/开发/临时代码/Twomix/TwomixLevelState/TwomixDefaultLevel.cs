using System;
using Script;
using Script.BallState;
using Script.Level.SpecialInteraction.Twomix.TwomixLevelState;
using UnityEngine;

public class TwomixDefaultLevel:TwomixBaseLevel
    {
        public TwomixDefaultLevel(TwomixLevelStateManager soccerLevelStateManager) : base(soccerLevelStateManager)
        {
            StateKey = TwomixLevelType.默认;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Blackboard.HitBall.gameObject.SetActive(true);
            EventManager.Instance.TriggerEvent(ClientEvent.BallType_Change, BallStateType.SoccerBall);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
