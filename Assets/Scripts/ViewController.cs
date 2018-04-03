using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

    public float speed = 25;
    public float speedMouse = 500;

	// Use this for initialization
	void Start () {
        Input.multiTouchEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");

        // 按照世界坐标系进行移动
        transform.Translate(new Vector3(h * speed, mouse * speedMouse, v * speed) * Time.deltaTime, Space.World);

        // 移动端触屏操作
        if (Input.touchCount <= 0)
            return;
        if (Input.touchCount == 1) // 单点触碰移动摄像机
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                m_screenPos = Input.touches[0].position;
            if(Input.touches[0].phase == TouchPhase.Moved)
            {
                transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime, Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
            }
        }
        else
        {
            // 手指位置
            Vector2 npos1 = new Vector2();
            Vector2 npos2 = new Vector2();
            // 手指每帧移动距离
            Vector2 deltaDis1 = new Vector2();
            Vector2 deltaDis2 = new Vector2();

            for(int i = 0; i < 2; i++)
            {
                Touch touch = Input.touches[i];
                if (touch.phase == TouchPhase.Ended)
                    break;
                if (touch.phase == TouchPhase.Moved)
                {
                    if (i == 0)
                    {
                        npos1 = touch.position;
                        deltaDis1 = touch.deltaPosition;
                    }
                    else
                    {
                        npos2 = touch.position;
                        deltaDis2 = touch.deltaPosition;

                        if (isEnlarge(opos1, opos2, npos1, npos2))
                            isForward = 1;
                        else
                            isForward = -1;
                    }
                    opos1 = npos1;
                    opos2 = npos2;
                }
                Camera.main.transform.Translate(isForward * Vector3.forward * Time.deltaTime
                    * (Mathf.Abs(deltaDis1.x + deltaDis2.x) + Mathf.Abs(deltaDis1.y + deltaDis2.y)));
            }
        }
    }

    private int isForward; // 摄像机移动方向
    private Vector2 opos1 = new Vector2(); // 手指旧位置
    private Vector2 opos2 = new Vector2();
    private Vector2 m_screenPos = new Vector2();    // 手指触碰的位置

    bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        // 判断两手指之间的距离是变大还是变小
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));

        if (leng1 < leng2)
            return true;
        else
            return false;
    }
}
