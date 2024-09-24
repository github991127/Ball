<h1>
<p align="center">项目结构</p>
</h1>

`菖蒲`

<!-- TOC -->

- [总览](#总览)
- [一般交互CommonUse-MeshDestroy](#一般交互commonuse-meshdestroy)
    - [打碎检测](#打碎检测)
        - [流程](#流程)
        - [MeshDestroyAble.cs](#meshdestroyablecs)
- [一般交互CommonUse-MoveObj](#一般交互commonuse-moveobj)
    - [平移旋转](#平移旋转)
        - [流程](#流程-1)
        - [MoveObj.cs](#moveobjcs)
- [一般交互CommonUse-CommonUse](#一般交互commonuse-commonuse)
    - [切面瞬时弹射](#切面瞬时弹射)
        - [流程](#流程-2)
        - [EjectionObj.cs](#ejectionobjcs)
    - [竖直延时弹射](#竖直延时弹射)
        - [流程](#流程-3)
        - [EjectionTrigger.cs](#ejectiontriggercs)
    - [多次碰撞](#多次碰撞)
        - [流程](#流程-4)
        - [GameButton.cs](#gamebuttoncs)
    - [星星系统](#星星系统)
        - [流程](#流程-5)
        - [RewardObj.cs](#rewardobjcs)
    - [风场](#风场)
        - [流程](#流程-6)
        - [WindZoneObj.cs](#windzoneobjcs)
- [特殊交互Speciallnteraction-Bedroom](#特殊交互speciallnteraction-bedroom)
    - [石球击碎瓶子](#石球击碎瓶子)
        - [流程](#流程-7)
        - [Bottle.cs](#bottlecs)
    - [切相机视角](#切相机视角)
        - [流程](#流程-8)
        - [DoorCameraTrigger.cs](#doorcameratriggercs)
    - [毁坏IPAD](#毁坏ipad)
        - [流程](#流程-9)
        - [IPAD.cs](#ipadcs)
    - [触发音乐](#触发音乐)
        - [流程](#流程-10)
        - [MusicBoxTrigger.cs](#musicboxtriggercs)
    - [点亮台灯](#点亮台灯)
        - [流程](#流程-11)
        - [TableLamp.cs](#tablelampcs)
    - [启动火车](#启动火车)
        - [流程](#流程-12)
        - [TrainTriggerZone.cs](#traintriggerzonecs)
    - [不倒翁](#不倒翁)
        - [流程](#流程-13)
        - [Tumbler.cs](#tumblercs)
- [足球关-Soccer](#足球关-soccer)
    - [总览](#总览-1)
    - [关卡加载和切换](#关卡加载和切换)
        - [流程](#流程-14)
        - [SoccerBaseLevel.cs](#soccerbaselevelcs)
        - [SoccerLevelStateManager.cs](#soccerlevelstatemanagercs)
    - [关卡分类](#关卡分类)
        - [SoccerDefaultLevel](#soccerdefaultlevel)
        - [SoccerReversedLevel](#soccerreversedlevel)
    - [重生管理](#重生管理)
        - [流程](#流程-15)
        - [SoccerLevelBlackboard.cs](#soccerlevelblackboardcs)
    - [常用](#常用)
        - [位置](#位置)
        - [数据](#数据)
        - [交互](#交互)
    - [AAA](#aaa)
        - [流程](#流程-16)
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
# 一般交互CommonUse-MeshDestroy

## 打碎检测

### 流程
        1新建类，继承MonoBehaviour
        成员：cutCascades，力，碎片属性继承布尔
        2事件函数
        检查球，冲力，碎片数
        碎片继承
### MeshDestroyAble.cs
```cs
//检查球
if (!other.transform.CompareTag(StaticStr.EnvironmentObjTagStr.Tag_BALL))
//检查冲力
if (StaticMethod.VectorDot(ballVelocity,other.contacts[0].normal)<4f)
```
# 一般交互CommonUse-MoveObj
## 平移旋转

### 流程
        1新建类，继承MonoBehaviour
        成员：原始中心点位置
        2事件函数
        平移往返
        旋转
        公转检测
### MoveObj.cs

# 一般交互CommonUse-CommonUse
## 切面瞬时弹射

### 流程
        1新建类，继承MonoBehaviour
        成员：力，动画字符串，冷却时间
        2事件函数
        检查球和冷却
        延迟恢复冷却
        播放音乐，动画
        检查玩家的球的类型，施加不同大小的力
### EjectionObj.cs
```cs
//延迟恢复冷却
TimeTool.Instance.Delay(CoolTime, (() => isInCooling = false));
//播放音乐
MusicManager.Instance.ChangeAndPlaySound("specialduang",true);
//播放动画
private static readonly int Trigger = Animator.StringToHash("Trigger");
aniObj.GetComponent<Animator>().SetTrigger(Trigger);
```

## 竖直延时弹射

### 流程
        1新建类，继承MonoBehaviour
        成员：力，等待时间，冷却时间
        2事件函数
        检查球和冷却
        延迟恢复冷却
        延迟调用，施加向上力
        检查玩家的球的类型，施加不同大小的力
### EjectionTrigger.cs
```cs
//检查当前物体的边界，实参球体的边界，若相交needTime后调用紧跟其后的回调函数，施加向上力
StaticMethod.DelayCheckIsIntersects(
    transform.GetComponent<Collider>().bounds,
    other.bounds, EjectionNeedTime, (() =>
    {
        isInCooling = true;
        ...
    }
```
## 多次碰撞

### 流程
        1新建类，继承MonoBehaviour
        成员：速度条件，质量条件，是否只能触发一次，是否已经被触发过，所需碰撞次数
        2事件函数
        检查球和冷却
        检查是否有碰撞方法
        检查是否满足碰撞条件
        声明事件，调用碰撞方法，或最终碰撞方法
### GameButton.cs
```cs
//检查是否满足碰撞条件：物体的质量、速度和碰撞方向
var mass=other.transform.GetComponent<Rigidbody>().mass;
var ballCurrentVelocity = StaticData.Player.PlayerBallController.ballVelocity.CurrentVelocity;
if (CheckIsCanInvoke(mass,ballCurrentVelocity,other.contacts[0].normal))
{
}

//声明事件，调用碰撞方法
public Action ButtonMethod;
ButtonMethod.Invoke();
```

## 星星系统

### 流程
        1新建类，继承MonoBehaviour
        成员：父物体
        2事件函数
        检查是否已经收集
        收集星星
        检查是否解锁新球体
        
### RewardObj.cs
```cs
//检查已收集列表，若包含当前星星则隐藏
if (saveDataCollectedStrList!=null && 
    saveDataCollectedStrList.Exists((starName)=>starName==rootGameObject.name))
{
    rootGameObject.SetActive(false);
}

//收集星星，存入已收集列表
SaveManager.Instance.SaveData.CollectedStarList.Add(transform.name);
SaveManager.Instance.WriteSaveData();
```

## 风场

### 流程
        1新建类，继承MonoBehaviour
        成员：风力大小
        2事件函数
        检查球
        施加风力
        
### WindZoneObj.cs
```cs
//施加风力
StaticMethod.AddForceToBall(transform.name,force);
```
---
# 特殊交互Speciallnteraction-Bedroom

## 石球击碎瓶子
### 流程
        1新建类，继承MonoBehaviour
        成员：碎掉的瓶子
        2事件函数
        检查球，是否有标签“Ball”
        检查球的类型，是否为RockBall
        播放，消失，激活
### Bottle.cs
```cs
//other为实参，碰撞的物体
if (!other.transform.CompareTag("Ball")) return;
//静态数据，球的类型
if(StaticData.Player.BallType!=BallTypeEnum.RockBall)return;
//碎片出现，预设的成员物体
BrokenBottle.SetActive(true);
//瓶子消失，当前物体
transform.gameObject.SetActive(false);
```

## 切相机视角

### 流程
        1新建类，继承MonoBehaviour
        成员：固定虚拟相机
        2事件函数
        检查数据触发历史
        保存数据触发历史
        改变相机和操作权限，延时改回

### DoorCameraTrigger.cs
```cs
//保存卡门数据触发历史-SaveManager
SaveManager.Instance.SaveData.TriggedDoorCameraSwitch = true;
SaveManager.Instance.WriteSaveData();
//延时改回相机和操作权限-TimeTool
TimeTool.Instance.Delay(5, (() =>
{
    CloseUpCamera.SetActive(false);
    EventManager.Instance.TriggerEvent(ClientEvent.PlayerKeyInput_Effective_Change);
}));
```
## 毁坏IPAD

### 流程
        1新建类，继承MonoBehaviour
        可序列化成员：渲染器组件（可改变材质），三种屏幕材质图像
        2事件函数
        按钮关联事件
        延时改回材质

### IPAD.cs
```cs
//延时改回材质
TimeTool.Instance.Delay(0.1f, (() =>
            {
                SetScreenMaterial(OriginMaterial);
                MusicManager.Instance.ChangeAndPlaySound("specialshoot",true);
            }));
```

## 触发音乐

### 流程
        1新建类，继承MonoBehaviour
        成员：具象音符
        2事件函数
        激活，播放

### MusicBoxTrigger.cs
```cs
//激活，播放
MusicNote.SetActive(true);
MusicManager.Instance.ChangeAndPlaySound("specialspeak",true);
```

## 点亮台灯

### 流程
        1新建类，继承MonoBehaviour
        可序列化成员：按钮，光源
        2事件函数
        冷却检测，更改光源，播放声音，冷却重置

### TableLamp.cs

## 启动火车

### 流程
        1新建类，继承MonoBehaviour
        成员：火车，烟雾，奖励，烟雾时间，奖励时间
        2事件函数
        延迟检测碰撞启动火车，延迟开启奖励，离开时删除事件

### TrainTriggerZone.cs
```cs
//延迟触发，且检测碰撞后触发（可优化为StaticMethod.DelayCheckIsIntersects）
TimeTool.Instance.Delay(trainAniNeedTime, (() =>
{
}));
```
## 不倒翁

### 流程
        1新建类，继承MonoBehaviour
        成员：
        2事件函数
        固定质心

### Tumbler.cs
```cs
//固定质心
var rb = GetComponent<Rigidbody>();
rb.centerOfMass = new Vector3(0, -1, 0);
```

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







