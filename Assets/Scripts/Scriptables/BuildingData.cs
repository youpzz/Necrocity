using System;
using UnityEngine;

[CreateAssetMenu(fileName = "building data", menuName = "youpzdev/scriptables/building")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GainResources[] gainResources;
}

[Serializable]
public class GainResources
{
    public ResourceType resourceType;
    public int gainTime;
    public int gainLimit;
}