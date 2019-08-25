using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public  static class Utils {
    public static bool isWithinSelectionBounds(Transform transform, Vector3 point1, Vector3 point2)
    {
        var camera = Camera.main;
        var viewPortBounds = ScreenHelper.GetViewportBounds(camera, point1, point2);
        return viewPortBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }

    public static bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public static bool IsMouseOverUIWithIgnores(string ignores) {
        if (ignores.Length <= 0 ) {
            ignores = "Interactive";
        }
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResultList);
        for (int i = 0; i < raycastResultList.Count; i++) {
            if (raycastResultList[i].gameObject.tag.CompareTo(ignores) != 0 ){
                raycastResultList.RemoveAt(i);
                i--;
            }
        }
        return raycastResultList.Count > 0;
    }

    public static Vector2 getMouseToViewportPoint(){
         return Camera.main.ScreenToViewportPoint( new Vector3( 
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.transform.position.z));
    }

}