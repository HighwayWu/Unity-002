using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TurretData {

    public GameObject turretPrefab;
    public GameObject turretPrefabUp;
    public int cost;
    public int costUp;
    public TurretType type;

}

public enum TurretType
{
    LaserTurret,
    MissileTurret,
    StandardTurret
}
