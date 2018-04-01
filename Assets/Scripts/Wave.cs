using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 每一波敌人的属性
[System.Serializable]
public class Wave {
    public GameObject enemyPrefab;
    public int count;
    public float rate;

}
