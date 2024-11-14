using UnityEngine;

public class CookingInteraction : MonoBehaviour
{
    public GameObject cookingStation;
    private bool isNearStation = false;

    void Update()
    {
        
        if (isNearStation && Input.GetKeyDown(KeyCode.E))
        {
            StartCookingMinigame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearStation = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNearStation = false;
        }
    }

    void StartCookingMinigame()
    {
        Debug.Log("Minijuego de cocina iniciado. ¡Saca los ingredientes!");
        // Lógica para iniciar el minijuego y mostrar los ingredientes
    }
}
