
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public float health;
    public Image healthBar;
    public GameObject canvas;

    public void setHealth(float health){
        this.health = health;
    }
    
    public void updateHealthBar(float currentHealth) {
        this.healthBar.fillAmount = (currentHealth / this.health);
    }

    public void hide(bool hide) {
        this.canvas.SetActive(hide);
    }

    private void Update() {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);    
    }
}
