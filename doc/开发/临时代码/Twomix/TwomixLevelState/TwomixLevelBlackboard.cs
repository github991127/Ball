using System;
using Script.BallState;
using Script.Manager.StateManager;
using Script.StaticClass;
using UnityEngine;

namespace Script.Level.SpecialInteraction.Twomix.TwomixLevelState
{
    public class TwomixLevelBlackboard:BaseBlackboard
    {
        private Transform BallTransform=>StaticData.Player.PlayerBallController.transform;
        public SoccerLevelConfig CurConfig => CurLevelIndex-1<StaticData.Soccer.SoccerLevelConfigs.Count ? 
            StaticData.Soccer.SoccerLevelConfigs[CurLevelIndex-1] : null;

        public float 每关进球后延时时间下一关=>CurConfig.FinishTextTime;
        
        public Transform CurRespawnTransform=>TransformUtil.Find(CurWorldObj.transform,"Respawn");
        
        public SoccerPanel SoccerPanel;
        public HitBall HitBall;
        
        public int CurLevelIndex=1;//从1开始
        
        public GameObject CurWorldObj;
        
        private Rigidbody HitBallRigidBody;

        public void ResetBallPos()
        {
            BallTransform.position = CurRespawnTransform.position;
        }
        
        public void ResetHitBall()
        {
            var hitBallRespawn = TransformUtil.Find(CurWorldObj.transform, "HitBallRespawn");
            HitBall.transform.position = hitBallRespawn.position;
            HitBall.CollisionCount = CurConfig.MaxCollisionTimes;
            HitBall.CurCollisionCount = 0;
            HitBall.SetTransparency(1);
            HitBallRigidBody.Sleep();
            HitBall.gameObject.layer = LayerMask.NameToLayer("Environment");
        }

        public void LoadLevel(Action callBack=null)
        {
            LoadManager.Instance.LoadAndShowPrefabAsync($"World{CurLevelIndex}", CurConfig.LevelAddress,
                null, (o =>
                {
                    CurWorldObj = o;
                    callBack?.Invoke();
                }));
        }
        public void LoadHitBall(Action callBack=null)
        {
            LoadManager.Instance.LoadAndShowPrefabAsync("HitBall", StaticStr.PrebLoadPath.HitBall,
                null, (o =>
                {
                    StaticData.Soccer.HitBall = o;
                    HitBall = o.AddComponent<HitBall>();
                    HitBall.Config = CurConfig;
                    HitBallRigidBody = o.GetComponent<Rigidbody>();
                    callBack?.Invoke();
                }));
        }
    }
}