using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CookingManager : MonoBehaviour
{
public Transform grill; // Transform de la parrilla
public Transform board; // Transform de la tabla
private int boardItemCount = 0;
void Update()
{
if (Input.GetMouseButtonDown(0)) // Clic izquierdo: Cocinar
{
RaycastHit hit;
if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
{
if (hit.collider.CompareTag("Cura") || hit.collider.CompareTag("Fuerza") ||
hit.collider.CompareTag("Veloz"))
{
Ingredient ingredient = hit.collider.GetComponent<Ingredient>();
if (ingredient != null)
{
ingredient.transform.position = grill.position;
ingredient.StartCooking();
}
}
}
}
else if (Input.GetMouseButtonDown(1)) // Clic derecho: Colocar
{
RaycastHit hit;
if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
{
if (hit.collider.CompareTag("Cura") || hit.collider.CompareTag("Fuerza") ||
hit.collider.CompareTag("Veloz"))
{
Ingredient ingredient = hit.collider.GetComponent<Ingredient>();
if (ingredient != null)
{
ingredient.transform.position = board.position + new Vector3(0, 0.1f *
boardItemCount, 0);
ingredient.StopCooking();
boardItemCount++;
}
}
}
}
}
}