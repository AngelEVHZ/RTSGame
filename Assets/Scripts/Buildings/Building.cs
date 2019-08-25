using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public float life = 100f;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.onUpdate();
    }
    public virtual void onUpdate() {
       
    }

    public virtual void addDamage(float damage) {
        this.life -= damage;
    }    

}
