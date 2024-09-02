using Script;
using Script.BallState;
using Script.Level.SpecialInteraction.Twomix.TwomixLevelState;
using Script.StaticClass;

public class TwomixReversedLevel:TwomixBaseLevel
    {
        public TwomixReversedLevel(TwomixLevelStateManager soccerLevelStateManager) : base(soccerLevelStateManager)
        {
            StateKey = TwomixLevelType.翻转;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void LevelStart()
        {
            base.LevelStart();
            Blackboard.HitBall.gameObject.SetActive(false);
            LoadManager.Instance.LoadAndShowPrefabAsync("ModelBall",StaticStr.PrebLoadPath.ModelBall,
                null, (o => StaticData.Soccer.ModelBall=o));
            EventManager.Instance.TriggerEvent(ClientEvent.BallType_Change, BallStateType.FootBall);
        }
    }
