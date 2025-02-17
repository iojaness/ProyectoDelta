using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ingredient : MonoBehaviour
{
public Material rawMaterial; // Material crudo
public Material cookedMaterial; // Material cocido
public Material burntMaterial; // Material quemado
private Renderer renderer;
private float cookTimer = 0f;
private bool isCooking = false;
public string cookState = "Crudo"; // Estado inicial del ingrediente
void Start()
{
renderer = GetComponent<Renderer>();
renderer.material = rawMaterial;
}
void Update()
{
if (isCooking)
{
cookTimer += Time.deltaTime;
if (cookTimer >= 5f && cookTimer < 7f)
{
renderer.material = cookedMaterial;
cookState = "Completo";
}
else if (cookTimer >= 7f)
{
renderer.material = burntMaterial;
cookState = "Quemado";
}
}
}
public void StartCooking()
{
isCooking = true;
}
public void StopCooking()
{
isCooking = false;
}
}
