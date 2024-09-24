<h1>
<p align="center">项目结构</p>
</h1>

`菖蒲`

<!-- TOC -->

- [总览](#总览)
- [足球关-Soccer](#足球关-soccer)
    - [总览](#总览-1)
    - [关卡加载和切换](#关卡加载和切换)
        - [流程](#流程)
        - [SoccerBaseLevel.cs](#soccerbaselevelcs)
        - [SoccerLevelStateManager.cs](#soccerlevelstatemanagercs)
    - [关卡分类](#关卡分类)
        - [SoccerDefaultLevel](#soccerdefaultlevel)
        - [SoccerReversedLevel](#soccerreversedlevel)
    - [重生管理](#重生管理)
        - [流程](#流程-1)
        - [SoccerLevelBlackboard.cs](#soccerlevelblackboardcs)
    - [常用](#常用)
        - [位置](#位置)
        - [数据](#数据)
        - [交互](#交互)
    - [AAA](#aaa)
        - [流程](#流程-2)
        - [BBB.cs](#bbbcs)

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
# 足球关-Soccer

## 总览
关卡信息
BaseState-SoccerBaseLevel
关卡信息管理
StateManager-SoccerLevelStateManager
关卡数据
BaseBlackboard-SoccerLevelBlackboard

## 关卡加载和切换

### 流程
        1新建类，SoccerBaseLevel:BaseState
        成员：
        2事件函数
        Y
### SoccerBaseLevel.cs
```cs
//获取足球关数据
protected SoccerLevelBlackboard Blackboard=>soccerLevelStateManager.Blackboard;

//构造函数赋值StateKey和soccerLevelStateManager
protected SoccerBaseLevel(SoccerLevelStateManager soccerLevelStateManager)

//进入下一关逻辑：保存当前关卡索引到SaveManager的SoccerData
public override void OnEnter()

//退出当前关逻辑：显示目标UI，切换场景
public override void OnExit()

//重置关卡数据并加载足球
public virtual void OnStart()

//进入下一关逻辑：显示关卡开始的UI，重置球的位置和碰撞计数，并加载关卡内容
protected virtual void LevelStart()
```

### SoccerLevelStateManager.cs
```cs
stateDic字典存储不同关卡类型
SwitchState方法来初始化当前关卡状态
EventManager.Instance.AddEvent注册事件（ClientEvent.Goal）
```


## 关卡分类

### SoccerDefaultLevel
SoccerDefaultLevel:SoccerBaseLevel
### SoccerReversedLevel
SoccerReversedLevel:SoccerBaseLevel

## 重生管理

### 流程
        1新建类，SoccerLevelBlackboard:BaseBlackboard
        成员：
        BallTransform：足球的位置和旋转

        CurConfig：当前关卡的配置

        每关进球后延时时间下一关

        CurRespawnTransform：查找名为"Respawn"的Transform

        SoccerPanel

        HitBall

        CurLevelIndex：当前关卡索引

        CurWorldObj：当前关卡的世界对象

        HitBallRigidBody
        
        2事件函数
        重生ResetBallPos，ResetHitBall
        异步加载
### SoccerLevelBlackboard.cs
```cs

```








---

## 常用
### 位置
//作弊键
Assets/Script/Tools/EnvironmentTest.cs

### 数据

//物体是球
StaticStr.EnvironmentObjTagStr.Tag_BALL
//球的类型
StaticData.Player.BallType
//球的类型枚举
case BallTypeEnum.WoodBall:
//球的质量
var mass=other.transform.GetComponent<Rigidbody>().mass;
//球的速度
var ballCurrentVelocity = StaticData.Player.PlayerBallController.ballVelocity.CurrentVelocity;
//球的解锁星星数
int neededStarCount = StaticData.BallConfigData.BallConfigList[i].NeededStatueCount;
                
//X碰撞的物体
X.transform.CompareTag("Ball")
//已收集的星星
var saveDataCollectedStrList = SaveManager.Instance.SaveData.CollectedStarList;

### 交互

//播放音乐
MusicManager.Instance.ChangeAndPlaySound("specialduang",true);
//延迟恢复冷却
isInCooling = true;
TimeTool.Instance.Delay(CoolTime, (() => isInCooling = false));
//球施加力
StaticMethod.AddForceToBall(transform.name,force);


## AAA

### 流程
        1新建类，继承MonoBehaviour
        成员：X
        2事件函数
        Y

### BBB.cs

```cs
//固定质心
var rb = GetComponent<Rigidbody>();
rb.centerOfMass = new Vector3(0, -1, 0);
```







