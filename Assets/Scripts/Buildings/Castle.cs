using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : Building
{
    // Start is called before the first frame update
    [SerializeField]
    public Transform[] spawnPoint;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        base.onUpdate();
        
    }

    public bool isApawnPointAvailable() {
        bool available = false;
        foreach (var spawn in spawnPoint)
        {
            if (spawn.GetComponent<SpawnPoint>().getIsAvailable()) {
                available = true;
                break;
            }
        }
        return available;
    }
    public Vector3 getSpawnPoint() {
        Transform spawnSelected = null;
        foreach (var spawn in spawnPoint)
        {
            if (spawn.GetComponent<SpawnPoint>().getIsAvailable()) {
                spawnSelected = spawn.GetComponent<Transform>();
                break;
            }
        }
       return new Vector3(spawnSelected.position.x, spawnSelected.position.y, spawnSelected.position.z);
    }
    

}
