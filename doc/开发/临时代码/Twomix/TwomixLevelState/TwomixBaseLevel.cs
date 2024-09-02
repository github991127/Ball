using System;
using Script.Manager.StateManager;
using Script.StaticClass;
using UnityEngine;

namespace Script.Level.SpecialInteraction.Twomix.TwomixLevelState
{
    public class TwomixBaseLevel:BaseState
    {
        private TwomixLevelStateManager soccerLevelStateManager;
        protected TwomixLevelBlackboard Blackboard=>soccerLevelStateManager.Blackboard;
        protected TwomixBaseLevel(TwomixLevelStateManager soccerLevelStateManager)
        {
            this.soccerLevelStateManager = soccerLevelStateManager;
            StateKey = this.ToString();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            SaveManager.Instance.SaveData.SoccerData.LevelIndex = Blackboard.CurLevelIndex;
            SaveManager.Instance.WriteSaveData();
            
            TimeTool.Instance.Delay(Blackboard.每关进球后延时时间下一关, (() =>
            {
                LoadManager.Instance.DestroyObj(Blackboard.CurWorldObj);
                LevelStart();
            }));
        }

        public override void OnExit()
        {
            base.OnExit();
            Blackboard.SoccerPanel.ShowGoalUI(Blackboard.CurConfig);
            if (Blackboard.CurLevelIndex == StaticData.Soccer.SoccerLevelConfigs.Count)//关卡计数从1开始
            {
                TimeTool.Instance.Delay(Blackboard.CurConfig.FinishTextTime, (() =>
                    EventManager.Instance.TriggerEvent(ClientEvent.Scene_Switch, StaticStr.SceneName.Room)));
            }
            Blackboard.CurLevelIndex++;
        }

        public virtual void OnStart()
        {
            SaveManager.Instance.SaveData.SoccerData.LevelIndex = Blackboard.CurLevelIndex;
            SaveManager.Instance.WriteSaveData();
            Blackboard.LoadHitBall(LevelStart);
        }

        protected virtual void LevelStart()
        {
            Blackboard.HitBall.Config = Blackboard.CurConfig;
            Blackboard.SoccerPanel.ShowStartLevelUI(Blackboard.CurConfig);
            EventManager.Instance.TriggerEvent(ClientEvent.BallPos_Reset);
            Blackboard.HitBall.CollisionCount = Blackboard.CurConfig.MaxCollisionTimes;
            Blackboard.HitBall.CurCollisionCount = 0;
            Blackboard.LoadLevel((() =>
            {
                Blackboard.ResetBallPos();
                Blackboard.ResetHitBall();
            }));
        }
    }
}