using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {
    public int energyCost = 2;
    public string type;
    public string cardName;
    public GameObject obj;
    public PlayerController playerController;   

       void Start()
    {
        
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawn() {
        bool resp = this.playerController.spawn(this.energyCost, this.type, this.obj);
        Debug.Log(resp);
    }
}