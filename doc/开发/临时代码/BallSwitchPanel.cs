using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSwitchPanel : MonoBehaviour
{
    public RectTransform parentItem;
    public RectTransform[] itemRectTransforms;
    public Dictionary<RectTransform, Vector3> itemDic = new Dictionary<RectTransform, Vector3>();

    [Header("Circle")] public Vector3 centerPos = Vector3.zero;
    public float radius = 500; //轮切半径
    public float offsetValueY = 20; //靠后图片向上偏移量

    [Range(0.1f, 0.6f)] public float minAlphaValue = 0.2f; //靠后图片最小透明度

    // Start is called before the first frame update
    void Start()
    {
        InitializationItems();
        SetItemsPosition();
    }

    // 获取所有轮切图片
    void InitializationItems()
    {
        if (parentItem == null)
        {
            //print("parentItem is null");
            return;
        }

        var rects = parentItem.GetComponentsInChildren<RectTransform>(); // 包含了父物体
        itemRectTransforms = new RectTransform[rects.Length - 1];
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            itemRectTransforms[i] = rects[i + 1]; // 去除父物体，从第2个开始
        }
    }

    // 设置轮切图片位置
    void SetItemsPosition()
    {
        //计算角度和位置
        float angle = 0;
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            angle = i * 360 / itemRectTransforms.Length;
            float radian = (angle / 180) * Mathf.PI;
            float sin_Value = radius * Mathf.Sin(radian);
            float cos_Value = radius * Mathf.Cos(radian);
            Vector3 targetPos = centerPos + new Vector3(sin_Value, 0, -cos_Value);
            //计算y的偏移量
            if (i != 0)
            {
                targetPos.y += offsetValueY * i;
                if (i > itemRectTransforms.Length / 2)
                {
                    targetPos.y = offsetValueY * (itemRectTransforms.Length - i);
                }
            }

            //存入字典
            itemDic.Add(itemRectTransforms[i], targetPos);
            //改变显示位置
            //print("targetPos:" + targetPos);
            itemRectTransforms[i].anchoredPosition3D = targetPos;

            //图层优先级排序
            SetItemsSibling();
            //改变透明度
            SetItemsAlphaValue();
        }
    }

    // 图层优先级排序
    void SetItemsSibling()
    {
        Dictionary<RectTransform, int> orderDic = new Dictionary<RectTransform, int>();
        //二层循环，每次找到z值最大的，赋值thisRect
        for (int i = 0; i < itemDic.Count; i++)
        {
            float maxValue = float.MinValue;
            RectTransform thisRect = new RectTransform();
            foreach (var dic in itemDic)
            {
                if (!orderDic.ContainsKey(dic.Key))
                {
                    if (dic.Value.z > maxValue)
                    {
                        maxValue = dic.Value.z;
                        thisRect = dic.Key;
                    }
                }
            }

            orderDic.Add(thisRect, i);
        }

        //根据字典的值，设置层级
        foreach (var dic in orderDic)
        {
            dic.Key.SetSiblingIndex(dic.Value);
        }
    }

    // 设置图片透明度
    void SetItemsAlphaValue()
    {
        float startValue = centerPos.z - radius;
        foreach (var dic in itemDic)
        {
            //根据前后距离在其中的比例，计算透明度
            float alphaValue = 1 - Mathf.Abs(dic.Value.z - startValue) / (2 * radius) * (1 - minAlphaValue);
            var rawImage = dic.Key.GetComponent<RawImage>();
            if (rawImage)
            {
                rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, alphaValue);
            }
        }
    }

    private Coroutine currentCoroutine; //当前携程

    // 点击左
    public void OnClickLeft()
    {
        StartCoroutine(MoveLeft());
    }

    // 点击右
    public void OnClickRight()
    {
        StartCoroutine(MoveRight());
    }

    IEnumerator MoveLeft()
    {
        //还有未结束的协程，等待结束
        if (currentCoroutine != null)
        {
            yield return currentCoroutine;
        }

        Vector3 firstPos = itemDic[itemRectTransforms[0]];
        for (int i = 0; i < itemRectTransforms.Length; i++)
        {
            Vector3 targetPos = itemRectTransforms[(i + 1) % itemRectTransforms.Length].anchoredPosition3D;

            if (i == itemRectTransforms.Length - 1)
            {
                targetPos = firstPos;
            }

            // 开启协程，移动到目标位置
            currentCoroutine = StartCoroutine(MoveToTargetPos(itemRectTransforms[i], targetPos));
        }
    }

    IEnumerator MoveRight()
    {
        //还有未结束的协程，等待结束
        if (currentCoroutine != null)
        {
            yield return currentCoroutine;
        }

        Vector3 firstPos = itemDic[itemRectTransforms[itemRectTransforms.Length - 1]];
        for (int i = itemRectTransforms.Length - 1; i >= 0; i--)
        {
            Vector3 targetPos = itemRectTransforms[(i + itemRectTransforms.Length - 1) % itemRectTransforms.Length]
                .anchoredPosition3D;

            if (i == 0)
            {
                targetPos = firstPos;
            }

            // 开启协程，移动到目标位置
            currentCoroutine = StartCoroutine(MoveToTargetPos(itemRectTransforms[i], targetPos));
        }
    }

    // 携程同时移动
    IEnumerator MoveToTargetPos(RectTransform rectTrans, Vector3 targetPos)
    {
        float speed = (targetPos - rectTrans.anchoredPosition3D).magnitude;

        //每一帧移动，直到目标位置
        while (rectTrans.anchoredPosition3D != targetPos)
        {
            rectTrans.anchoredPosition3D =
                Vector3.MoveTowards(rectTrans.anchoredPosition3D, targetPos, speed * Time.deltaTime);
            yield return null; //中断指令，等待函数执行
        }

        //移动完成之后更新字典
        itemDic[rectTrans] = targetPos;
        //图层优先级排序
        SetItemsSibling();
        //改变透明度
        SetItemsAlphaValue();

        yield return null; //防止连续操作的冲突
    }
}