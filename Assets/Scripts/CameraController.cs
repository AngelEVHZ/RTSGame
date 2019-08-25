using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5f;
    public float joystickSpeed = 100;
    public int cameraLimitX = 5;
    public int cameraLimitZ = 5;
    private Vector2 cameraPointA;
    private Vector2 cameraPointB;
    private bool canCameraMove;

    public RectTransform joystick_outLine;
    public RectTransform joystick;
    private Vector2 joystickPosition;

    public Transform followObject;
    // Start is called before the first frame update
    void Start()
    {
        this.canCameraMove = false;
        this.joystick.gameObject.SetActive(true);
        this.joystickPosition = new Vector2(this.joystick.localPosition.x,this.joystick.localPosition.y);
        this.joystick_outLine.gameObject.SetActive(true);
        this.followObject.position = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetMouseButtonDown(0))
        {
            if (Utils.IsMouseOverUIWithIgnores("Joystick")) {
                this.canCameraMove = true;
                this.cameraPointA = Utils.getMouseToViewportPoint();        
            }
        }
        if (Input.GetMouseButton(0)) {
            if (this.canCameraMove) {
                this.cameraPointB = Utils.getMouseToViewportPoint();
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (this.canCameraMove) {
                this.canCameraMove = false;
                this.resetJoystick();    
            }        
        }
        this.cameraMovement();
    }

    private void FixedUpdate() {
        this.cameraMovement();   
    }
    void cameraMovement() {
       if (this.canCameraMove) {
           Vector2 offset = this.cameraPointB - this.cameraPointA;
           Vector2 direction = Vector2.ClampMagnitude(offset, 1.0f);
           Vector2 directionGUI =  Vector2.ClampMagnitude(direction * this.joystickSpeed, 100f);  
            if ( !this.isOncameraLimits(1, direction.x)){
               direction.x = 0;
            }
            if ( !this.isOncameraLimits(2, direction.y)){
               direction.y = 0;
            }
            Vector3 cameraDirection = new Vector3(direction.x, 0f , direction.y);
            this.followObject.Translate(cameraDirection * cameraSpeed * Time.deltaTime);
            Camera.main.transform.position = this.followObject.transform.position;
            this.joystick.localPosition = new Vector2( 
                this.joystickPosition.x + directionGUI.x,
                this.joystickPosition.y + directionGUI.y);
       }
    }

    bool isOncameraLimits(int limit, float direction) {
        if (limit == 1 && direction > 0) {
            return (this.followObject.position.x <= this.cameraLimitX);
        } 
        if (limit == 1 && direction < 0) {
         return this.followObject.position.x >= (this.cameraLimitX * -1);
        }
        if (limit == 2 && direction > 0) {
            return (this.followObject.position.z <= this.cameraLimitZ);
        } 
        if (limit == 2 && direction < 0) {
         return this.followObject.position.z >= (this.cameraLimitZ * -1);
        }
        return true;
    }
    void resetJoystick() {
        this.joystick.localPosition = new Vector2( 
                this.joystickPosition.x,
                this.joystickPosition.y);
    }
    
}
