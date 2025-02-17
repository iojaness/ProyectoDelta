/*using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class IngredientData
{
public string name; // Nombre del ingrediente (Cura, Fuerza, Veloz)
public string cookState; // Estado del ingrediente (Crudo, Completo, Quemado)
}
[System.Serializable]
public class IngredientList
{
public List<IngredientData> ingredients = new List<IngredientData>();
}
public class EffectCalculator : MonoBehaviour
{
public string loadPath = "ingredientData.json"; // Nombre del archivo JSON
private Dictionary<string, float> stateBonuses = new Dictionary<string, float>
{
{ "Crudo", 1f },
{ "Completo", 1.5f },
{ "Quemado", 0f }
};
private void Start()
{
string filePath = Path.Combine(Application.persistentDataPath, loadPath);
if (File.Exists(filePath))
{
string json = File.ReadAllText(filePath);
IngredientList ingredientList = JsonUtility.FromJson<IngredientList>(json);
if (ingredientList.ingredients.Count > 0)
{
ProcessCombination(ingredientList.ingredients);
}
else
{
Debug.Log("No hay ingredientes en el archivo.");
}
}
else
{
Debug.Log("Archivo JSON no encontrado en " + filePath);
}
}
private void ProcessCombination(List<IngredientData> ingredients)
{
if (ingredients.Count < 2)
{
Debug.Log("Se requiere al menos dos ingredientes para calcular un efecto.");
return;
}
// Obtener los nombres y estados de los ingredientes
string ingredient1 = ingredients[0].name;
string state1 = ingredients[0].cookState;
string ingredient2 = ingredients[1].name;
string state2 = ingredients[1].cookState;
// Calcular bonificador por estado
float bonus1 = stateBonuses.ContainsKey(state1) ? stateBonuses[state1] : 0f;
float bonus2 = stateBonuses.ContainsKey(state2) ? stateBonuses[state2] : 0f;
// Generar el efecto según la combinación
string effect = GetEffect(ingredient1, ingredient2, bonus1, bonus2);
// Mostrar en consola
Debug.Log($"Combinación: {ingredient1} ({state1}) + {ingredient2} ({state2})");
Debug.Log($"Efecto: {effect}");
}
private string GetEffect(string ingredient1, string ingredient2, float bonus1, float bonus2)
{
// Combina ingredientes para calcular el efecto
if ((ingredient1 == "Fuerza" && ingredient2 == "Veloz") || (ingredient1 == "Veloz" &&
ingredient2 == "Fuerza"))
{
if (bonus1 == 0 || bonus2 == 0)
return "El plato está quemado. Aplica un efecto negativo: Pierdes 10 puntos de
vida por segundo durante 5 segundos.";
return $"Más velocidad de movimiento (+{5 * bonus2}%) y un aumento de {5 *
bonus1} en el daño, pero resta -5 de vida por segundo durante 10 segundos.";
}
if ((ingredient1 == "Cura" && ingredient2 == "Veloz") || (ingredient1 == "Veloz" &&
ingredient2 == "Cura"))
{
if (bonus1 == 0 || bonus2 == 0)
return "El plato está quemado. Aplica un efecto negativo: Velocidad reducida en
un 50% durante 5 segundos.";
return $"Otorga más velocidad (+{10 * bonus2}%) y cura {20 * bonus1} puntos de
vida, pero no puedes atacar por 3 segundos.";
}
if ((ingredient1 == "Cura" && ingredient2 == "Fuerza") || (ingredient1 == "Fuerza" &&
ingredient2 == "Cura"))
{
if (bonus1 == 0 || bonus2 == 0)
return "El plato está quemado. Aplica un efecto negativo: -10 de vida inmediata.";
return $"Aumenta el daño un {10 * bonus1}% y cura {15 * bonus2} puntos de vida.";
}
return "Combinación no reconocida. No se genera efecto.";
}
}*/