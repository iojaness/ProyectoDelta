/*using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class IngredientData
{
public string name;
public string cookState;
}
[System.Serializable]
public class IngredientList
{
public List<IngredientData> ingredients = new List<IngredientData>();
}
public class SaveManager : MonoBehaviour
{
public Transform board; // Referencia a la tabla
public string savePath = "ingredientData.json";
public void SaveIngredients()
{
IngredientList ingredientList = new IngredientList();
foreach (Transform child in board)
{
Ingredient ingredient = child.GetComponent<Ingredient>();
if (ingredient != null)
{
ingredientList.ingredients.Add(new IngredientData
{
name = ingredient.gameObject.name,
cookState = ingredient.cookState
});
}
}
string json = JsonUtility.ToJson(ingredientList, true);
File.WriteAllText(Path.Combine(Application.persistentDataPath, savePath), json);
Debug.Log("Data saved to " + Path.Combine(Application.persistentDataPath,
savePath));
}
}*/