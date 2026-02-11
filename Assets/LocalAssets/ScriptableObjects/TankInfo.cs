using UnityEngine;

[CreateAssetMenu(fileName = "TankInfo", menuName = "Scriptable Objects/TankInfo")]
public class TankInfo : ScriptableObject
{
    public GameObject TankPrefab;

    public float life, damage, speed, fireRate;

}
