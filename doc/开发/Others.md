<h1>
<p align="center">项目结构</p>
</h1>

`菖蒲`

<!-- TOC -->

- [总览](#总览)
- [存档重生](#存档重生)
    - [流程](#流程)
    - [StaticClass/StaticStr.cs](#staticclassstaticstrcs)
    - [BallCC/Player/PlayerOnCollisionMethod.cs](#ballccplayerplayeroncollisionmethodcs)
    - [BallCC/BallPhysical/BallPhysical.cs](#ballccballphysicalballphysicalcs)
- [AAA](#aaa)
    - [流程](#流程-1)
    - [CCC.cs](#ccccs)

<!-- /TOC -->
# 总览
- CommonUse
  - MeshDestroy
  - MoveObj

- Speciallnteraction
  - Bedroom
  - GravityRotatingRoom
  - MusicLevel


---
# 存档重生

## 流程
        1成员
        成员：X
        2事件函数public void OnTriggerEnter(Collider other)
        创建标签
        switch检测碰撞体标签
        获取场景标签并检验
        存储子物体位置
        赋值位置
        重置速度

## StaticClass/StaticStr.cs
```cs
//创建标签
public static class EnvironmentObjTagStr
```
## BallCC/Player/PlayerOnCollisionMethod.cs

```cs
//存储子物体位置
SaveManager.Instance.SaveData.BallSavingPosition = other.transform.GetChild(0).transform.position;
//赋值位置
ballTransform.position = SaveManager.Instance.SaveData.BallSavingPosition;
//重置速度
ballPhysical.Reset();
```

## BallCC/BallPhysical/BallPhysical.cs

```cs
SetRigidbody(BallConfig config): 根据配置设置Rigidbody的属性，如质量、阻力和角阻力。

OnFixedUpdate(...): 在Unity的FixedUpdate事件中调用，用于根据输入和配置计算并应用力和扭矩。

ForceCalculate(...): 计算合内力（如重力、水平移动力、跳跃力）。
ExternalForceCalculate(): 将待处理的力（外力）加到合内力上。
ApplyForce(): 将计算出的合内力施加到球体上。
TorqueCalculate(...): 计算并设置水平移动的扭矩。
ApplyTorque(): 将扭矩施加到球体上。
Jump(...): 处理跳跃逻辑，包括检查是否在地面上、重置竖直速度、设置跳跃力和调整检测地面的射线长度。

AddForce(string sourceObjName, Vector3 addForce): 向待处理力的字典中添加一个力，该力将在下一帧的固定更新中施加。

CheckIsOnGround(): 通过射线检测球体是否触地。

ChangeDownVector(Vector3 vector3): 更改重力方向和射线检测的方向。

ChangeGravitationalAcceleration(): 根据配置更改重力加速度。

SetCheckGroundRayLengthToCommonLength(): 将检测地面的射线长度设置为正常值。

GetBallRigidbody(): 返回Rigidbody组件的引用。

Reset(): 重置类的状态，包括力和重力方向，并使球体进入休眠状态。

```
# AAA

## 流程
        1新建类，继承MonoBehaviour
        成员：X
        2事件函数
        Y

## CCC.cs

```cs
//固定质心
var rb = GetComponent<Rigidbody>();
rb.centerOfMass = new Vector3(0, -1, 0);
```
