using UnityEngine;

[System.Serializable]
public class ErosionSettings
{
    [Header("General")]
    public int iterations = 50000;
    public int brushRadius = 3;

    [Header("Droplet Lifetime")]
    public int maxLifetime = 30;

    [Header("Sediment")]
    public float sedimentCapacityFactor = 3f;
    public float minSedimentCapacity = 0.01f;

    [Header("Terrain Change")]
    [Range(0f, 1f)]
    public float depositSpeed = 0.3f;

    [Range(0f, 1f)]
    public float erodeSpeed = 0.3f;

    [Header("Water")]
    [Range(0f, 1f)]
    public float evaporateSpeed = 0.01f;

    public float gravity = 4f;
    public float startSpeed = 1f;
    public float startWater = 1f;

    [Header("Movement")]
    [Range(0f, 1f)]
    public float inertia = 0.3f;

    [Header("Flow Map")]
    public float flowStrength = 1f;
    public float flowExponent = 1.5f;
}