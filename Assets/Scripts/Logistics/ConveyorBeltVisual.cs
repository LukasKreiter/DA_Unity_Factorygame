using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveyorBeltVisual : MonoBehaviour
{
    private ConveyorBelt belt;

    [SerializeField] private GameObject itemPrefab;

    private Dictionary<BeltItem, Transform> cubes = new Dictionary<BeltItem, Transform>();
    private List<BeltItem> toDelete = new List<BeltItem>();

    private void Awake()
    {
        belt = GetComponent<ConveyorBelt>();
    }
    private void Update()
    {
        foreach (BeltItem item in belt.Items) {
            if (!cubes.ContainsKey(item))
            {
                Transform cube = Instantiate(itemPrefab, transform).transform;
                cubes.Add(item, cube);
            }
            cubes[item].position = Vector3.Lerp(belt.StartPoint, belt.EndPoint, item.position / belt.Length);
        }

        foreach (KeyValuePair<BeltItem, Transform> entry in cubes)
        {
            if (!belt.Items.Contains(entry.Key))
            {
                toDelete.Add(entry.Key);
            }
        }

        foreach (BeltItem item in toDelete) {
            Destroy(cubes[item].gameObject);
            cubes.Remove(item);
        }
        toDelete.Clear();
    }
}
