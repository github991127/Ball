<h1>
<p align="center">UI</p>
</h1>

`菖蒲`

<!-- TOC -->

- [BallChangePanel.cs](#ballchangepanelcs)
    - [属性](#属性)
    - [方法](#方法)
        - [BeforeShow()](#beforeshow)
        - [AfterHide()](#afterhide)
        - [Hide()](#hide)
        - [OnClick()OnPointerEnter()](#onclickonpointerenter)
        - [AddCloseCall()](#addclosecall)
    - [功能](#功能)
- [BallChangePanel.cs](#ballchangepanelcs-1)
    - [属性](#属性-1)
    - [方法](#方法-1)
    - [功能](#功能-1)
- [AAA.cs](#aaacs)
    - [属性](#属性-2)
    - [方法](#方法-2)
    - [功能](#功能-2)

<!-- /TOC -->

<div STYLE="page-break-after: always;"></div>

# BallChangePanel.cs
一个UI面板的基类，用于在游戏或应用程序中提供面板的基本行为和功能。这个类包含了一系列保护字段、虚拟方法以及用于处理UI交互和面板生命周期的辅助方法。下面是对代码中关键部分的详细解释：
## 属性
PanelObj：代表面板的GameObject，是UI面板在Unity场景中的实际表示。

CanHideByEsc、IsDisablePlayerMove、IsDisablePlayerCameraActivation、IsHideCurse、IsStopTime：这些布尔字段用于控制面板的行为，如是否可以通过Esc键关闭、是否禁用玩家移动、是否禁用玩家镜头操作、是否隐藏鼠标光标、是否暂停时间等。

## 方法
### BeforeShow()
```csharp
//在面板显示之前调用，是否禁用玩家移动、是否禁用玩家镜头操作、是否隐藏鼠标光标、是否暂停时间
//InputManager单例方法
if (IsDisablePlayerMove) InputManager.Instance.MoveInputEffective = false;
if (IsDisablePlayerCameraActivation)
    EventManager.Instance.TriggerEvent(ClientEvent.PlayerCamera_MoveInputEffective_Change, false);
//StaticMethod类中包含了经常使用的静态方法
StaticMethod.LockCursor(IsHideCurse);
//Time.timeScale设置为趋近于0的无限小，模拟时间暂停的效果，同时又避免了一些可能由完全停止时间（即时间缩放比例为0）引起的问题，比如物理模拟的完全停止。
if (IsStopTime) StaticMethod.SetTimeScale(0.000001f);
```
### AfterHide()
退出后显示次级UI

### Hide()
恢复之前可能因面板显示而被禁用的功能（如玩家移动、镜头操作等），并隐藏面板。

### OnClick()OnPointerEnter()
```csharp
用于为面板内的按钮添加点击和鼠标移入事件监听器。
TransformUtil.Find(PanelObj.transform, objName).GetComponent<Button>().onClick.AddListener(callback);
```

### AddCloseCall()
```csharp
//添加一个延迟调用，以便在指定的时间后自动关闭面板。通过TimeTool.Instance.Delay方法实现
timeIndex = TimeTool.Instance.Delay(closeTime, (() => { UIManager.Instance.HidePanel(GetType()); }));
```

## 功能



# BallChangePanel.cs
用于处理与球类切换相关的UI面板的逻辑。这个面板可能用于游戏中，允许玩家查看可切换的球类信息，并选择切换当前使用的球类。下面是对代码中关键部分的解释：

## 属性
ballTypeButtonGroup：存储球类按钮组的Transform，这些按钮用于表示不同的球类。

isUnLockBallGroup：一个布尔数组，表示每个球类是否已解锁。

ballConfigList和ballConfigListAble：分别存储所有球类配置和可解锁球类配置的列表。

## 方法

## 功能



方法
Init：初始化方法，设置IsHideCurse为false，检查所有球类是否可用和是否已解锁，并设置topBallIndex为当前玩家使用的球类的索引。

Bind：绑定UI元素到成员变量，并设置切换球类的按钮点击事件和鼠标移入事件。

Show：显示面板时调用的方法，设置球类纹理位置、中心文本和进度文本，并禁用球类切换功能。

SetBallTexturePos：根据topBallIndex设置球类按钮的纹理和位置，使其围绕中心点分布。

SetCenterText：设置中心文本显示的球类名称和详细信息，根据球类是否已解锁显示不同的文本。

SetChangeButton：为每个球类按钮设置点击事件和鼠标移入事件，如果球类已解锁则添加点击事件，否则将按钮颜色设置为灰色。

CheckAllTypeIsAble和CheckAllTypeIsUnlock：分别检查所有球类是否可用和是否已解锁，并将结果存储在ballConfigListAble和isUnLockBallGroup中。

CheckBallTypeIsUnlock：检查指定索引的球类是否已解锁。

SetProgressText：设置进度文本显示的奖杯数量。

OnTabDown、OnKeyQDown和OnKeyEDown：处理键盘按下事件，用于在球类之间切换。OnTabDown直接切换并隐藏面板，OnKeyQDown和OnKeyEDown分别用于向左和向右切换球类。

BeforeHide：在隐藏面板之前调用，恢复玩家移动输入有效性，切换相机焦点，并启用球类切换功能。

关键点
事件系统：代码中使用了EventManager.Instance.TriggerEvent来触发事件，这表明游戏使用了一个事件系统来管理不同组件之间的通信。

动画和UI：通过Animator和UI元素（如TextMeshProUGUI和RawImage）来实现动画和界面显示。

状态管理：通过StaticData.Player.BallStateManager.Blackboard.BallConfig等静态变量来管理游戏状态，如当前球类配置。

输入处理：通过重写OnKeyQDown、OnKeyEDown等方法来处理键盘输入，允许玩家通过键盘快捷键来切换球类。






# AAA.cs
## 属性

## 方法

## 功能


