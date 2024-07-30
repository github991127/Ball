using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switch2D : MonoBehaviour
{
    public RawImage item; // ԭʼͼƬ
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void OnClick1()
    {
        if (item != null)
        {
            item.transform.localPosition = new Vector3(400, 150, 0);
        }
    }


}