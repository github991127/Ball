<h1>
<p align="center">UI</p>
</h1>

`菖蒲`

<!-- TOC -->

- [切球](#切球)
    - [流程](#流程)
    - [BallChangePanel.cs](#ballchangepanelcs)
    - [KeyboardFunction.cs](#keyboardfunctioncs)
    - [InputManager.cs](#inputmanagercs)
- [开场](#开场)
    - [流程](#流程-1)
    - [ComicPanel.cs](#comicpanelcs)
- [退出](#退出)
    - [流程](#流程-2)
    - [ExitPanel.cs](#exitpanelcs)
    - [KeyboardFunction.cs](#keyboardfunctioncs-1)
    - [InputManager.cs](#inputmanagercs-1)
- [提示切换](#提示切换)
    - [流程](#流程-3)
    - [HintChangePanel.cs](#hintchangepanelcs)
- [设置](#设置)
    - [流程](#流程-4)
    - [SettingPanel.cs](#settingpanelcs)
- [字幕](#字幕)
    - [流程](#流程-5)
    - [SubtitlePanel.cs](#subtitlepanelcs)
    - [SubtitleManager.cs](#subtitlemanagercs)
    - [StaticData.cs](#staticdatacs)

<!-- /TOC -->

<div STYLE="page-break-after: always;"></div>

# 切球
## 流程
        事件绑定单击方法
        改变相机和操作
        检测球体解锁
        切换球体
## BallChangePanel.cs
```csharp
//设置球类型选择按钮的显示（如果未解锁则变灰）和点击事件
SetChangeButton();
//切换相机
EventManager.Instance.TriggerEvent(ClientEvent.Switch_Camera);
//更改玩家输入控制的有效性
EventManager.Instance.TriggerEvent(ClientEvent.Change_Player_KeyInput_Effective);
//获取星星数量
SetProgressText();
//复原Show中的方法：切换相机，更改玩家输入控制的有效性
public override void BeforeHide()
```
## KeyboardFunction.cs
```csharp
//TAB事件
public void OpenChangeBallPanel()
//ESC事件
public void OnEscKeyDown()
```
## InputManager.cs
```csharp
//事件调用
private void HandlerEachFrameKeyMethod(bool isOnGround)
//按下TAB，地面状态
if (Input.GetKeyDown(KeyCode.Tab) && isOnGround) eachFrameKeyMethod.Add(keyboardFunction.OpenChangeBallPanel);
//按下ESC
if (Input.GetKeyDown(KeyCode.Escape)) eachFrameKeyMethod.Add(keyboardFunction.OnEscKeyDown);
```
# 开场
## 流程
        事件绑定单击方法
        加载预制体
        激活分镜画面
## ComicPanel.cs
```csharp
//加载预制体
LoadManager.Instance.LoadAndShowPrefabAsync(comicGroupName,StaticStr.PrebLoadPath.ComicGroup,PanelObj.transform,(o =>{
//将某个名为ClickArea的UI元素（可能是按钮或可点击区域）的点击事件绑定到OnComicPanelClick方法上
OnClick("ClickArea",OnComicPanelClick);
//单击一次，激活一个分镜画面，全部激活后消失
private void OnComicPanelClick()
ComicGroup.GetChild(ComicIndex).gameObject.SetActive(true);
```

# 退出
## 流程
        事件绑定单击方法
        改变相机和操作，时间暂停
        退出游戏
## ExitPanel.cs
```csharp
//更改相机激活状态
EventManager.Instance.TriggerEvent(ClientEvent.ChangeAllPlayer_CameraActivation);
//0暂停1开始
StaticMethod.SetTimeScale(0);
//退出游戏
private void OnExitButtonClick()
{
#if UNITY_EDITOR
    EditorApplication.isPlaying = false;
    return;
#else
    Application.Quit();
#endif
}
```

## KeyboardFunction.cs
```csharp
//调用条件
public void OnEscKeyDown()
```

## InputManager.cs
```csharp
//事件调用
private void HandlerEachFrameKeyMethod(bool isOnGround)
//按下ESC
if (Input.GetKeyDown(KeyCode.Escape)) eachFrameKeyMethod.Add(keyboardFunction.OnEscKeyDown);
```

# 提示切换
## 流程
        改变相机和操作，时间暂停
        保存设置
## HintChangePanel.cs
```csharp
//更新持久化数据并保存
SaveManager.Instance.SaveData.isUnLockChangePanel = true;
SaveManager.Instance.WriteSaveData();
```

# 设置
## 流程
        事件绑定单击方法
        查询滑块位置，绑定监听
        改变相机和操作，时间暂停
        保存音量
        保存设置

## SettingPanel.cs

- 成员
```csharp
private Slider volumeSettingSlider;
private Slider soundSettingSlider;
```
- 函数
```csharp
//为这两个Slider组件的onValueChanged事件添加了监听器
volumeSettingSlider.onValueChanged.AddListener(SetMusicVolume);
soundSettingSlider.onValueChanged.AddListener(SetSoundVolume);

//调整并保存音量
volumeSettingSlider.value = SaveManager.Instance.SaveData.MusicVolume;
soundSettingSlider.value = SaveManager.Instance.SaveData.SoundVolume;

//其他监听这个事件的部分就可以根据音量值的变化做出相应的处理
EventManager.Instance.TriggerEvent(ClientEvent.MusicVolume_Setting_Change,volume);
EventManager.Instance.TriggerEvent(ClientEvent.SoundVolume_Setting_Change,volume);

//保存设置
private void OnSaveSettingClick()
```
# 字幕
## 流程
        timeIndex延时关闭
        取消已经安排的事件
        碰撞和触发调用
        检测取出并删去字典内容
## SubtitlePanel.cs

- 成员
```csharp
//引用字幕文本组件,Transform
private Transform subtitleText;
//配置：目标物，文本，时间
public SingleSubtitleConfig CurSubtitleConfig;
//延迟存在时间
private int timeIndex;
```
- 函数
```csharp
//面板延迟消失，消失后记录
timeIndex=AddCloseCall();
//查找文本物体
subtitleText=TransformUtil.Find(PanelObj.transform,"SubtitleText");
//获取文本物体的文本
subtitleText.GetComponent<TextMeshProUGUI>().text = CurSubtitleConfig.Text;
//面板已消失，取消已经安排的事件
TimeTool.Instance.RemoveTimeEvent(timeIndex);
```
## SubtitleManager.cs
```csharp
//展示面板
private void ShowSubtitlePanel(SingleSubtitleConfig config)
        {
           CheckSubtitlePanelShowing();
           UIManager.Instance.ShowPanel<SubtitlePanel>().CurSubtitleConfig=config;
        }
//已达成触发条件，先删去物体记录，再调用ShowSubtitlePanel
public void OnCollision(string objName)
//先检测指定时间后，两物体是否重叠
public void OnTrigger(Collider other)
```

## StaticData.cs
```csharp
//全字幕字典
public static Dictionary<string, SingleSubtitleConfig> SubtitleConfigsDic;
```

