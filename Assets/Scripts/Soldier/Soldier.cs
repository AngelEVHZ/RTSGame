using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class Soldier : MonoBehaviour
{
    enum State { IDLE = 0, ATTACK = 1, MOVING = 2 };
    public float life = 20f;
    public float distanceToAtack = 1f;
    public bool isEnemy = false;
    public float attackPower = 2f;
    private bool isSelected;
    public float timeToAttack = 1;
    private float timeToAttackCounter;
    public NavMeshAgent agent;
    private State state = State.IDLE;
    private GameObject attackTarget;
    public float distanceToHelp = 5f;
    
    // Start is called before the first frame update

    private void Start() {
        agent.updateRotation = false;    
        this.timeToAttackCounter = timeToAttack;
    }
    public void moveToPoint(RaycastHit hit, int count) {
        GameObject obj = hit.transform.gameObject;
       // agent.stoppingDistance = (count / 10f) + 1;
        switch (obj.tag) {
            case "Soldier":
                if (obj.GetComponent<Soldier>().isEnemy != this.isEnemy) {
                    this.state = State.ATTACK;
                    this.attackTarget = obj;
                    agent.SetDestination(obj.transform.position);
                } else {
                    this.state = State.MOVING; 
                    agent.SetDestination(hit.point);    
                }
            break;
            default :
                this.state = State.MOVING;
                agent.SetDestination(hit.point);
                break;

        }
        Debug.Log("TAG  " +hit.transform.gameObject.tag);
    }   

    public void moveToHelp(GameObject enemy) {
        agent.SetDestination(enemy.transform.position);
        if (this.state != State.ATTACK) {
            Debug.Log("HELPP + " + this.state);
            this.attackTarget = enemy;
            this.state = State.ATTACK;
        }
    }

    // Update is called once per frame
      void Update()
    {
        this.onUpdate();
    }
    public virtual void onUpdate() {
        switch (this.state) {
            case State.IDLE: //iddle
                Debug.Log("iddle " + this.isEnemy);
            break;
            case State.ATTACK: //Attack
                Debug.Log("ATACK " + this.isEnemy);
                this.attackTimer();
            break;
            case State.MOVING: //just moving
                Debug.Log("Moving " + this.isEnemy);
            break;
        }
       
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

    public void attack() {
        if (this.attackTarget.tag.CompareTo("Soldier") == 0){ 
            this.attackTarget.GetComponent<Soldier>().addDamage(this.attackPower, gameObject);
            Debug.Log("ATACK!!");
        }
    }

    void attackTimer()
    {
        if (this.attackTarget)
        {
            if (this.getDistanceToAttack(this.attackTarget) <= this.distanceToAtack)
            {
                this.timeToAttackCounter -= Time.deltaTime;
                if (this.timeToAttackCounter <= 0)
                {
                    this.timeToAttackCounter = this.timeToAttack;
                    this.attack();
                }
            } 
        }
        else
        {
            this.timeToAttackCounter = this.timeToAttack;
            this.state = State.IDLE;
        }
    }
    
    public float getDistanceToAttack (GameObject target) {
        float dis = Vector3.Distance(target.transform.position, gameObject.transform.position);
        return dis;
    }


    public void addDamage(float damage, GameObject attacker) {
        this.life -= damage;
        if (this.life <= 0 ){
            Destroy(gameObject);
        }

        if ( this.state != State.ATTACK && getDistanceToAttack(attacker) <= this.distanceToAtack) {
            this.attackTarget = attacker;
            this.state = State.ATTACK;
            var foundObjects = GameObject.FindGameObjectsWithTag("Soldier");
            foreach (var soldier in foundObjects) {
                if ( soldier.GetComponent<Soldier>().isEnemy == this.isEnemy &&
                    Vector3.Distance(soldier.transform.position, gameObject.transform.position) <= this.distanceToHelp
                ) {
                   soldier.GetComponent<Soldier>().moveToHelp(attacker);
                }
            }
        }
    }

}
