<h1>
<p align="center">项目结构</p>
</h1>

`菖蒲`

<!-- TOC -->

- [总览](#总览)
- [打碎检测✔️MeshDestroy](#打碎检测meshdestroy)
    - [流程](#流程)
    - [MeshDestroyAble.cs](#meshdestroyablecs)
- [平移旋转✔️MoveObj](#平移旋转moveobj)
    - [流程](#流程-1)
    - [MoveObj.cs](#moveobjcs)
- [切面瞬时弹射✔️CommonUse](#切面瞬时弹射commonuse)
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
- [石球击碎瓶子✔️Bedroom](#石球击碎瓶子bedroom)
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
- [足球关卡管理✔️Soccer](#足球关卡管理soccer)
    - [流程](#流程-14)
    - [SoccerMono.cs](#soccermonocs)
- [AAA](#aaa)
    - [流程](#流程-15)
    - [CCC.cs](#ccccs)
- [常用](#常用)
    - [结构](#结构)
    - [数据源](#数据源)
    - [交互](#交互)

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
一般交互CommonUse

# 打碎检测✔️MeshDestroy

## 流程
        1新建类，继承MonoBehaviour
        成员：cutCascades，力，碎片属性继承布尔
        2事件函数
        检查球，冲力，碎片数
        碎片继承
## MeshDestroyAble.cs
```cs
//检查球
if (!other.transform.CompareTag(StaticStr.EnvironmentObjTagStr.Tag_BALL))
//检查冲力
if (StaticMethod.VectorDot(ballVelocity,other.contacts[0].normal)<4f)
```
# 平移旋转✔️MoveObj

## 流程
        1新建类，继承MonoBehaviour
        成员：原始中心点位置
        2事件函数
        平移往返
        旋转
        公转检测
## MoveObj.cs

# 切面瞬时弹射✔️CommonUse

## 流程
        1新建类，继承MonoBehaviour
        成员：力，动画字符串，冷却时间
        2事件函数
        检查球和冷却
        延迟恢复冷却
        播放音乐，动画
        检查玩家的球的类型，施加不同大小的力
## EjectionObj.cs
```cs
//延迟恢复冷却
TimeTool.Instance.Delay(CoolTime, (() => isInCooling = false));
//播放音乐
MusicManager.Instance.ChangeAndPlaySound("specialduang",true);
//播放动画
private static readonly int Trigger = Animator.StringToHash("Trigger");
aniObj.GetComponent<Animator>().SetTrigger(Trigger);
```

# 竖直延时弹射

## 流程
        1新建类，继承MonoBehaviour
        成员：力，等待时间，冷却时间
        2事件函数
        检查球和冷却
        延迟恢复冷却
        延迟调用，施加向上力
        检查玩家的球的类型，施加不同大小的力
## EjectionTrigger.cs
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
# 多次碰撞

## 流程
        1新建类，继承MonoBehaviour
        成员：速度条件，质量条件，是否只能触发一次，是否已经被触发过，所需碰撞次数
        2事件函数
        检查球和冷却
        检查是否有碰撞方法
        检查是否满足碰撞条件
        声明事件，调用碰撞方法，或最终碰撞方法
## GameButton.cs
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

# 星星系统

## 流程
        1新建类，继承MonoBehaviour
        成员：父物体
        2事件函数
        检查是否已经收集
        收集星星
        检查是否解锁新球体
        
## RewardObj.cs
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

# 风场

## 流程
        1新建类，继承MonoBehaviour
        成员：风力大小
        2事件函数
        检查球
        施加风力
        
## WindZoneObj.cs
```cs
//施加风力
StaticMethod.AddForceToBall(transform.name,force);
```
---
特殊交互Speciallnteraction

# 石球击碎瓶子✔️Bedroom
## 流程
        1新建类，继承MonoBehaviour
        成员：碎掉的瓶子
        2事件函数
        检查球，是否有标签“Ball”
        检查球的类型，是否为RockBall
        播放，消失，激活
## Bottle.cs
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

# 切相机视角

## 流程
        1新建类，继承MonoBehaviour
        成员：固定虚拟相机
        2事件函数
        检查数据触发历史
        保存数据触发历史
        改变相机和操作权限，延时改回

## DoorCameraTrigger.cs
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
# 毁坏IPAD

## 流程
        1新建类，继承MonoBehaviour
        可序列化成员：渲染器组件（可改变材质），三种屏幕材质图像
        2事件函数
        按钮关联事件
        延时改回材质

## IPAD.cs
```cs
//延时改回材质
TimeTool.Instance.Delay(0.1f, (() =>
            {
                SetScreenMaterial(OriginMaterial);
                MusicManager.Instance.ChangeAndPlaySound("specialshoot",true);
            }));
```

# 触发音乐

## 流程
        1新建类，继承MonoBehaviour
        成员：具象音符
        2事件函数
        激活，播放

## MusicBoxTrigger.cs
```cs
//激活，播放
MusicNote.SetActive(true);
MusicManager.Instance.ChangeAndPlaySound("specialspeak",true);
```



# 点亮台灯

## 流程
        1新建类，继承MonoBehaviour
        可序列化成员：按钮，光源
        2事件函数
        冷却检测，更改光源，播放声音，冷却重置

## TableLamp.cs

# 启动火车

## 流程
        1新建类，继承MonoBehaviour
        成员：火车，烟雾，奖励，烟雾时间，奖励时间
        2事件函数
        延迟检测碰撞启动火车，延迟开启奖励，离开时删除事件

## TrainTriggerZone.cs
```cs
//延迟触发，且检测碰撞后触发（可优化为StaticMethod.DelayCheckIsIntersects）
TimeTool.Instance.Delay(trainAniNeedTime, (() =>
{
}));
```
# 不倒翁

## 流程
        1新建类，继承MonoBehaviour
        成员：
        2事件函数
        固定质心

## Tumbler.cs
```cs
//固定质心
var rb = GetComponent<Rigidbody>();
rb.centerOfMass = new Vector3(0, -1, 0);
```

---
# 足球关卡管理✔️Soccer

## 流程
        
        

## SoccerMono.cs

字段和属性
HitBallAddress: 存储用于加载“击中球”（可能是游戏中的足球）的预制件（Prefab）的地址。

SoccerLevelConfigList: 存储关卡配置信息的列表。

hitBallObj 和 hitBall: 分别用于存储“击中球”的GameObject和组件引用。

worldList: 存储每个关卡GameObject的数组。

soccerSaveData: 存储游戏保存数据的引用。

curLevelIndex: 当前关卡索引。

方法
Awake(): 这是一个Unity生命周期方法，在GameObject激活时自动调用。它初始化世界列表，从保存数据中获取当前关卡索引，并调用InitLevel()方法来加载关卡和“击中球”。

InitLevel(): 这是一个异步方法，用于加载从当前关卡索引开始的所有关卡以及“击中球”。它使用LoadManager.Instance.LoadAndShowPrefabAsync方法来异步加载预制件，并等待所有关卡加载完成后再加载“击中球”。

OnHitBallCollisionWithBall(): 当“击中球”与游戏中的球发生碰撞时调用的方法。这里只检查了碰撞次数是否达到预设值，但没有实现透明度更改的逻辑。

NextLevel(): 用于加载下一个关卡的方法。它更新保存数据中的关卡索引，隐藏当前关卡，并重置“击中球”的碰撞次数和位置。

ResetBallPos(): 触发一个事件来重置球的位置。具体的重置逻辑可能在其他地方实现。

ResetHitBallPos(): 根据当前关卡中名为“HitBallRespawn”的标记物（可能是Transform或GameObject）来重置“击中球”的位置。

关键点
单例模式: MonoSingleton<SoccerMono>可能是一个用于确保SoccerMono类在整个游戏中只有一个实例的基类。

异步加载: 使用AsyncOperationHandle和await关键字来实现异步加载关卡，以提高游戏的加载速度和用户体验。

事件和委托: 使用委托来处理加载完成后的回调，以及使用EventManager来触发和监听事件。

资源管理: 通过LoadManager和SaveManager来管理游戏的资源加载和保存，这有助于保持代码的整洁和可维护性。

潜在的问题和改进点
碰撞透明度更改未实现: OnHitBallCollisionWithBall方法中的透明度更改逻辑未实现。

错误处理: 加载过程中可能会遇到错误，但没有看到任何错误处理逻辑。

性能优化: 加载所有关卡而不是仅当前关卡可能会影响游戏的加载时间。

代码注释: 虽然代码有一定的自解释性，但添加更多的注释可以帮助其他开发者更快地理解代码的目的和逻辑。





---
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

# 常用
## 结构
通用交互，按功能分配（破坏；移动）
特殊交互，按场景分类（卧室）


## 数据源

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

## 交互

//播放音乐
MusicManager.Instance.ChangeAndPlaySound("specialduang",true);
//延迟恢复冷却
isInCooling = true;
TimeTool.Instance.Delay(CoolTime, (() => isInCooling = false));
//球施加力
StaticMethod.AddForceToBall(transform.name,force);