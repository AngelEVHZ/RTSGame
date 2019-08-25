using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isAvailable = true;

    private void OnTriggerEnter(Collider other) {
        this.isAvailable = false;
    }
    private void OnTriggerExit(Collider other) {
          this.isAvailable = true;
    }
    public bool getIsAvailable() {
        return this.isAvailable;
    }
          
}
