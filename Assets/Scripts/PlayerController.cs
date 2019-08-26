using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlayerController : MonoBehaviour
{
    public Text energyText;
    public int energy = 10;
    public float timeToAddEnergy = 10f;
    private float timerToAddEnergyCounter;
    public Castle castle;
    private bool isDraggin;
    Vector3 mousePosition;
    private bool order;
    private List<GameObject> selectedSoldiersList;
    public Image cancelOrderImage;
    
    // Start is called before the first frame update
    void Start()
    {
        this.selectedSoldiersList = new List<GameObject>();
        this.timerToAddEnergyCounter = this.timeToAddEnergy;
        this.isDraggin = false;
        this.setOrder(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.timer(); 
        this.printEnergy(); 

        if (Input.GetMouseButtonDown(0)) {
            if (!Utils.IsMouseOverUI()) {
                this.mousePosition = Input.mousePosition;
                if (!this.order) {
                    this.isDraggin = true;
                } else {
                    Debug.Log("MOVEEE");
                    this.moveSoldiers();
                    
                }
            }
        } 

        if (Input.GetMouseButtonUp(0)) {
            if (this.isDraggin && !this.order) { 
                this.setSelectedSoldiers();
                this.isDraggin = false;
            }
            
        }
    }

    private void OnGUI(){
        if (this.isDraggin) {
            var rect = ScreenHelper.GetScreenRect(this.mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f,0.8f,0.95f,0.1f));
            ScreenHelper.DrawScreenRectBorder(rect, 1, Color.black);
        }
    }

    void printEnergy() {
        this.energyText.text = "Energia = " + this.energy;
    }

    void addEnergy(int energyAdded) {
        this.energy += energyAdded;
    }

    void lostEnergy(int energyCost) {
        this.energy -= energyCost;
    }

    void timer() {
        this.timerToAddEnergyCounter -= Time.deltaTime;
        if (this.timerToAddEnergyCounter <= 0) {
            this.timerToAddEnergyCounter = this.timeToAddEnergy;
            this.addEnergy(1);
        }
    }

    public bool spawn(int energyCost, string type, GameObject objToSpawn) {
        bool response = true;
        if (this.energy >= energyCost ) {
            if (type.CompareTo("Soldier") == 0 ) {
                if (this.castle.isApawnPointAvailable()) {
                    GameObject soldier = Instantiate (objToSpawn, this.castle.getSpawnPoint() , Quaternion.identity);
                    soldier.transform.parent = gameObject.transform;
                    this.lostEnergy(energyCost);
                } else {
                    Debug.Log("NOT AVAILABLE");
                    response = false;
                }
            }
        } else {
            response = false;
        }
        return response;
    }

    public void setSelectedSoldiers() {
        this.deSelectSoldiers();
        var foundObjects = GameObject.FindGameObjectsWithTag("Soldier");
        Vector3 positionStart = new Vector3 (Math.Min( this.mousePosition.x,Input.mousePosition.x),Math.Min( this.mousePosition.y,Input.mousePosition.y),0);  
        Vector3 positionEnd = new Vector3 (Math.Max( this.mousePosition.x,Input.mousePosition.x),Math.Max( this.mousePosition.y,Input.mousePosition.y),0);
        float distance = Vector3.Distance(positionStart,positionEnd);
        float minDistance = 10f;
        if (distance < minDistance) {
            positionStart += new Vector3(-1f, -1f, 0) * (minDistance - distance) * 2f; 
            positionEnd += new Vector3(+1f, +1f, 0) * (minDistance - distance) * 2f;
        }
        foreach (var soldier in foundObjects) {
            if ( !soldier.GetComponent<Soldier>().isEnemy &&
                Utils.isWithinSelectionBounds(soldier.transform, 
                positionStart, 
                positionEnd) 
               ) {
                soldier.GetComponent<Soldier>().selectSoldier(true);
                this.selectedSoldiersList.Add(soldier);
            }
        }
        if (this.selectedSoldiersList.Count > 0) {
            this.setOrder(true);
        }
        
    }
    public void deSelectSoldiers() {
        foreach (var soldier in this.selectedSoldiersList) {
            if (soldier) {
                soldier.GetComponent<Soldier>().selectSoldier(false);    
            }
        }
        this.selectedSoldiersList = new List<GameObject>();
    }


    public void moveSoldiers() {
        Ray ray = Utils.getScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            foreach (var soldier in this.selectedSoldiersList) {
               if (soldier) {
                    soldier.GetComponent<Soldier>().moveToPoint(hit, this.selectedSoldiersList.Count);    
               }
            }
        }
    }

    public void setOrder(bool ord) {
        this.order = ord;
        this.cancelOrderImage.gameObject.SetActive(ord);
    }
    public void cancelOrder() {
        this.cancelOrderImage.gameObject.SetActive(false);
        this.order = false;
        this.deSelectSoldiers();
    }

    

}
