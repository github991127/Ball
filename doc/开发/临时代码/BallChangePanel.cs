using System.Collections.Generic;
using Framework.Core;
using Script;
using Script.BallState;
using Script.StaticClass;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BallChangePanel : PanelBase
{
    private Transform ballTypeButtonGroup;
    private TextMeshProUGUI nameText;
    private TextMeshProUGUI detailText;
    private int topBallIndex; //QE切换指向的球
    private Transform progressText;
    private Animator backgroundAnimator;
    private static readonly int R = Animator.StringToHash("R");

    private static readonly int L = Animator.StringToHash("L");

    // ballTypeButtonGroup数量的bool数组
    private bool[] isUnLockBallGroup;


    private List<BallConfig> ballConfigList => StaticData.Ball.BallConfigData.BallConfigList;

    public override void Init()
    {
        topBallIndex = (int)StaticData.Player.BallStateType;
    }

    public override void Bind()
    {
        ballTypeButtonGroup = TransformUtil.Find(PanelObj.transform, "BallTypeButtonGroup");
        progressText = TransformUtil.Find(PanelObj.transform, "ProgressText");
        nameText = TransformUtil.Find(PanelObj.transform, "NameText").GetComponent<TextMeshProUGUI>();
        detailText = TransformUtil.Find(PanelObj.transform, "DetailText").GetComponent<TextMeshProUGUI>();
        backgroundAnimator = TransformUtil.Find(PanelObj.transform, "BallTypeBackground").GetComponent<Animator>();

        CheckAllTypeIsUnlock();
        SetChangeButton();
    }

    private void CheckAllTypeIsUnlock()
    {
        isUnLockBallGroup = new bool[ballTypeButtonGroup.childCount];
        for (int i = 0; i < ballTypeButtonGroup.childCount; i++)
        {
            isUnLockBallGroup[i] = CheckBallTypeIsUnlock(i);
        }
    }

    public override void Show()
    {
        SetBallTexturePos();
        SetCenterText((int)StaticData.Player.BallStateType);
        SetProgressText();
        EventManager.Instance.TriggerEvent(ClientEvent.BallType_ChangeEnabled, false);
    }

    private void SetBallTexturePos()
    {
        var groupRect = ballTypeButtonGroup.GetComponent<RectTransform>().rect;
        var vector = new Vector2(0, groupRect.height / 2); //指向正上方圆周的向量
        float eachAngle = 360 / ballConfigList.Count;
        for (int i = 0; i < ballConfigList.Count; i++)
        {
            var curTypeButton = ballTypeButtonGroup.GetChild(i).gameObject;
            curTypeButton.SetActive(true);
            curTypeButton.GetComponent<RawImage>().texture = ballConfigList[i].changePanelConfig.Texture;
            curTypeButton.name = ballConfigList[i].BallName;
            var buttonRectTransform = curTypeButton.GetComponent<RectTransform>();
            var angle = -eachAngle * (i - topBallIndex);
            buttonRectTransform.Rotate(Vector3.forward, angle);
            buttonRectTransform.anchoredPosition = StaticMethod.RotateVectorByAngle(
                vector - new Vector2(0, buttonRectTransform.rect.height / 2), angle);
        }
    }

    private void SetCenterText(int typeIndex, bool isLock = true)
    {
        var curConfig = ballConfigList[typeIndex];
        if (isLock)
        {
            nameText.text = curConfig.BallNameChinese;
            detailText.text = curConfig.changePanelConfig.Text;
        }
        else
        {
            nameText.text = curConfig.changePanelConfig.BallChangePanelUnLockConfig.Name;
            detailText.text = curConfig.changePanelConfig.BallChangePanelUnLockConfig.Text;
        }
    }

    private void SetChangeButton()
    {
        for (int i = 0; i < ballTypeButtonGroup.childCount; i++)
        {
            int numIndex = i;
            bool isLock = isUnLockBallGroup[i];
            if (isLock)
            {
                OnClick($"BallTypeButton.{numIndex}", () => { OnChangeBallTypeButtonClick(numIndex); });
            }
            else
            {
                ballTypeButtonGroup.GetChild(i).GetComponent<RawImage>().color = Color.gray;
            }

            OnPointerEnter($"BallTypeButton.{numIndex}", (ss) => { SetCenterText(numIndex, isLock); });
        }

        void OnChangeBallTypeButtonClick(int index)
        {
            nameText.text = ballConfigList[index].BallNameChinese;
            detailText.text = ballConfigList[index].changePanelConfig.Text;
            EventManager.Instance.TriggerEvent(ClientEvent.BallType_Change, (BallStateType)index);
            Hide();
        }
    }

    private bool CheckBallTypeIsUnlock(int typeIndex)
    {
        if (typeIndex >= ballConfigList.Count) return false;
        return ballConfigList[typeIndex].解锁需要的奖杯数量 <=
               SaveManager.Instance.SaveData.CollectedStarList.Count;
    }

    private void SetProgressText()
    {
        progressText.GetComponent<TextMeshProUGUI>().text =
            SaveManager.Instance.SaveData.CollectedStarList.Count.ToString();
    }

    public override void OnTabDown()
    {
        if (CheckBallTypeIsUnlock(topBallIndex))
        {
            EventManager.Instance.TriggerEvent(ClientEvent.BallType_Change, (BallStateType)topBallIndex);
        }

        Hide();
    }

    public override void OnKeyQDown()
    {
        base.OnKeyQDown();
        topBallIndex--;
        if (topBallIndex == -1)
        {
            topBallIndex = ballConfigList.Count - 1;
        }

        SetCenterText(topBallIndex, CheckBallTypeIsUnlock(topBallIndex));
        backgroundAnimator.SetTrigger(L);
        //如果未解锁，继续递归调用OnKeyQDown()
        // if (!CheckBallTypeIsUnlock(topBallIndex))
        // {
        //     OnKeyQDown();
        // }
    }

    public override void OnKeyEDown()
    {
        base.OnKeyEDown();
        topBallIndex++;
        if (topBallIndex == ballConfigList.Count)
        {
            topBallIndex = 0;
        }

        SetCenterText(topBallIndex, CheckBallTypeIsUnlock(topBallIndex));
        backgroundAnimator.SetTrigger(R);
        //如果未解锁，继续递归调用OnKeyEDown()
        // if (!CheckBallTypeIsUnlock(topBallIndex))
        // {
        //     OnKeyEDown();
        // }
    }

    public override void BeforeHide()
    {
        base.BeforeHide();
        InputManager.Instance.MoveInputEffective = true;
        StaticData.Player.CameraGroup.ChangePanelCameraFocusSwitch(false);
        EventManager.Instance.TriggerEvent(ClientEvent.BallType_ChangeEnabled, true);
    }
}