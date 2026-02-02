using System;
using UnityEngine;

[CreateAssetMenu(fileName = "building data", menuName = "youpzdev/scriptables/building")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GainResources[] gainResources; // для разных уровней массив
}

[Serializable]
public class GainResources
{
    public ResourceType resourceType;
    public ResourceSlot upgradePrice;
    public int gainTime;
    public int gainLimit;
}