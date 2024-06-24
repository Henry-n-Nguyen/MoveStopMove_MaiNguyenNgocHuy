using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SciptableObjects/Others/NameDataSO")]
public class NameDataSO : ScriptableObject
{
    // Name
    [SerializeField] private string[] names =
    {
        "An",
        "Alexander",
        "Bao",
        "Amelia",
        "Bin",
        "Ava",
        "Chaud",
        "Bella",
        "Chi",
        "Benjamin",
        "Cat",
        "Charlotte",
        "Dunk",
        "Chloe",
        "Doom",
        "Christopher",
        "Duck",
        "Daniel",
        "Damn",
        "David",
        "Ethan",
        "Giang",
        "Emily",
        "Grace",
        "Heity",
        "Harper",
        "Hamber",
        "Hayden",
        "Isabella",
        "Parfume",
        "Jackson",
        "James",
        "Kennedy",
        "Leighton",
        "Logan",
        "Lucas",
        "Mason",
        "Michael",
        "Natalie",
        "Olivia",
        "Emeral"
    };

    public string GetRandomName()
    {
        int randomNumber = Random.Range(0, names.Length);

        return names[randomNumber];
    }
}
