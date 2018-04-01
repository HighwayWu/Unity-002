using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

    public float speed = 25;
    public float speedMouse = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float mouse = Input.GetAxis("Mouse ScrollWheel");

        // 按照世界坐标系进行移动
        transform.Translate(new Vector3(h * speed, mouse * speedMouse, v * speed) * Time.deltaTime, Space.World);
	}
}
