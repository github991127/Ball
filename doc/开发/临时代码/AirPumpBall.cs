using System;
using Script;
using Script.BallState;
using Script.Manager;
using Script.StaticClass;
using UnityEngine;

public class AirPumpBall : BaseBall
{
    private float originScale;
    private float maxScale;
    private int pumpCountDownTimeIndex = -1;
    private float shiftDownCount = 0;
    private Rigidbody ballRigidbody;
    private float originMass;
    private int PumpForceFactor = 800;

    private float percentage => (ballStateManager.Blackboard.BallTransform.localScale.x - originScale) /
                                (maxScale - originScale);

    public AirPumpBall(BallStateManager ballStateManager) : base(ballStateManager)
    {
        StateKey = BallStateType.AirPumpBall;
        coolTime = 0;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        TimeTool.Instance.RemoveTimeEvent(pumpCountDownTimeIndex);
        pumpCountDownTimeIndex = -1;
        originScale = 0;
        ReductionBall();
    }

    protected override void ShiftDownMethod()
    {
        if (originScale == 0)
        {
            originScale = ballStateManager.Blackboard.BallTransform.localScale.x;
            maxScale = originScale * 3;
            ballRigidbody = ballStateManager.Blackboard.BallPhysical.GetBallRigidbody();
            originMass = 2;
        }
        Pump();
    }

    private void Pump()
    {
        var localScale = ballStateManager.Blackboard.BallTransform.localScale;
        if (localScale.x > maxScale) return;

        //如果球体在下落，则球体不能变大
        if (ballRigidbody.velocity.y < -0.1) return;

        localScale *= 1.2f;
        ballRigidbody.mass = originMass - originMass * percentage / 2;
        ballStateManager.Blackboard.BallTransform.localScale = localScale;
        MusicManager.Instance.ChangeAndPlaySound("BumpPillow", true);
        shiftDownCount++;
        if (pumpCountDownTimeIndex != -1) return;

        pumpCountDownTimeIndex = TimeTool.Instance.Countdown(0.05f, (() =>
        {
            if (ballStateManager.Blackboard.BallTransform.localScale.x < originScale)
            {
                TimeTool.Instance.RemoveTimeEvent(pumpCountDownTimeIndex);
                pumpCountDownTimeIndex = -1;
                ReductionBall();
                return;
            }

            ballStateManager.Blackboard.BallTransform.localScale *= 0.98f;
            ballRigidbody.mass = originMass - originMass * percentage / 2;
            PumpForce();
        }));
    }

    private void PumpForce()
    {
        ballRigidbody.AddForce(Vector3.up * PumpForceFactor * shiftDownCount);
        // ballRigidbody.transform.GetComponent<Rigidbody>().velocity =
        //     StaticMethod.CalculateJumpVelocity(ballRigidbody.transform.position,
        //         StaticData.Player.PlayerBallController.transform.position
        //         , 5f * shiftDownCount);
        // Debug.Log("shiftDownCount = " + shiftDownCount);
        shiftDownCount = 0;
    }

    private void ReductionBall()
    {
        ballStateManager.Blackboard.BallTransform.localScale = new Vector3(originScale, originScale, originScale);
        // ballRigidbody.mass = originMass;
    }
}