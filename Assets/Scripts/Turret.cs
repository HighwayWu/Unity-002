using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private List<GameObject> enemys = new List<GameObject>();
    public float attackRateTime = 1;  //攻击时间间隔
    public Transform head;
    private float timer = 0;
    public GameObject weaponPrefab;
    public Transform firePosition;
    public bool useLaser = false;
    public float laserRate = 70;
    public LineRenderer laserRender;
    public GameObject laserEffect;

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Enemy")
        {
            enemys.Remove(col.gameObject);
        }
    }

    void Start()
    {
        timer = attackRateTime;//炮台一创建可立即攻击
    }

    void Update()
    {
        //炮台转向
        if (enemys.Count > 0 && enemys[0] != null)
        {
            Vector3 targetPosition = enemys[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        if (useLaser == false)
        {
            //非激光攻击
            timer += Time.deltaTime;
            if (enemys.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        else if(enemys.Count > 0) //激光攻击
        {
            laserRender.enabled = true;
            laserEffect.SetActive(true);
            if (enemys[0] == null)
                UpdateEnemys();
            if(enemys.Count > 0)
            {
                laserRender.SetPositions(new Vector3[] { firePosition.position, enemys[0].transform.position });
                enemys[0].GetComponent<Enemy>().TakeDamage(laserRate * Time.deltaTime);
                laserEffect.transform.position = enemys[0].transform.position;
                Vector3 pos = transform.position;
                pos.y = enemys[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        }else
        {
            laserRender.enabled = false;
            laserEffect.SetActive(false);
        }
    }

    private void Attack()
    {
        if (enemys[0] == null)
            UpdateEnemys();
        if (enemys.Count > 0)
        {
            GameObject weapon = GameObject.Instantiate(weaponPrefab, firePosition.position, firePosition.rotation);
            weapon.GetComponent<Weapon>().SetTarget(enemys[0].transform);
        }
        else
        {
            timer = attackRateTime;//重置炮台为可攻击状态
        }
    }

    void UpdateEnemys()
    {
        List<int> emptyIndex = new List<int>();
        for(int i = 0; i < enemys.Count; i++)
        {
            if (enemys[i] == null)
            {
                emptyIndex.Add(i);
            }
        }

        for(int i = 0; i < emptyIndex.Count; i++)
        {
            enemys.RemoveAt(emptyIndex[i] - i);
        }
    }
}
