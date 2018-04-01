using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;
    public GameObject upgradeCanvas;
    public Button buttonUpgrade;
    private MapCube selectedMapCube; //已建造的炮台对应的mapcub
    private TurretData selectedTurret = null; // 选择的炮台(要建造的炮台)

    public Text moneyText;
    private int money = 1000;
    public Animator moneyAnimator;
    private Animator upgradeCanvasAnimator;

    void Start()
    {
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //检测是否点击到了UI
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //检测是否点击到了mapCube
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray,out hit, 1000, LayerMask.GetMask("MapCube"));

                if (isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>(); // 点击的mapCube
                    if(mapCube.turret == null && selectedTurret != null)
                    {
                        //创建炮台
                        if(money >= selectedTurret.cost)
                        {
                            ChangeMoney(-selectedTurret.cost);
                            mapCube.BuildTurret(selectedTurret);
                        }else
                        {
                            moneyAnimator.SetTrigger("Flick");
                        }
                    }else if(mapCube.turret != null)
                    {
                        //显示升级UI
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                            StartCoroutine(ButtonHide());
                        else
                            ButtonShow(mapCube.transform.position, mapCube.isUpgraded);
                        selectedMapCube = mapCube;
                    }
                }
            }
        }
    }

    void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "$ " + money;
    }

    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
            selectedTurret = laserTurretData;
    }

    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
            selectedTurret = missileTurretData;
    }

    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
            selectedTurret = standardTurretData;
    }

    void ButtonShow(Vector3 pos, bool isDisableUpgrade = false)
    {
        //StopCoroutine("ButtonHide");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos + new Vector3(0,4.5f,0);
        buttonUpgrade.interactable = !isDisableUpgrade;//禁用升级按钮
    }

    IEnumerator ButtonHide()
    {
        upgradeCanvasAnimator.SetTrigger("hide");
        yield return new WaitForSeconds(0.3f);
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeButtonDown()
    {
        if(money >= selectedMapCube.turretData.costUp)
        {
            ChangeMoney(-selectedMapCube.turretData.costUp);
            selectedMapCube.UpgradeTurret();
            StartCoroutine(ButtonHide());
        }else
        {
            moneyAnimator.SetTrigger("Flick");
        }        
    }

    public void OnDestroyButtonDown()
    {
        selectedMapCube.DestroyTurret();
        StartCoroutine(ButtonHide());
    }
}
