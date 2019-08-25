using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Soldier : MonoBehaviour
{
    public float life = 20f;
    private bool isSelected;

    public NavMeshAgent agent;
    // Start is called before the first frame update

    private void Start() {
        agent.updateRotation = false;    
    }
    public void moveToPoint(RaycastHit hit) {
        agent.SetDestination(hit.point);
    }   

    // Update is called once per frame
      void Update()
    {
        this.onUpdate();
    }
    public virtual void onUpdate() {
 
    }

    public void selectSoldier(bool select)
    {
        this.isSelected = select;
        if (select)
        {
            Debug.Log("SELECTED");
        }
        else
        {
            Debug.Log("DES SELECTED");
        }
    }
   

}
