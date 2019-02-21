using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Phylum : MonoBehaviour
{
    public GameObject[] animals;        // Animal prefabs belonging to this Phylum
    public string name;                 // The name of this phylum

    public GameObject GetRandomAnimalUnique(GameObject[] existing)
    {
        GameObject animal;
        int count = 0;                                                  // Breaker for inf loops
        do
        {
            animal = GetRandomAnimal();
            count++;
            if (count > 99)
                break;
        } while (existing.Contains(animal));                            // While the list already contains the animal, redo
        return animal;
    }

    public GameObject[] GetRandomAnimalList(int length)
    {
        GameObject[] randomList = new GameObject[length];
        if (length % animals.Length == 0)                               // No need to get random, just return by quotient then shuffle
        {
            for (int i = 0; i < length; i++)
            {
                randomList[i] = animals[i % animals.Length];
            }
        }
        else if (length > animals.Length)                               // Get random for remainder
        {
            for (int i = 0; i < length % animals.Length; i++)
            {
                randomList[i] = GetRandomAnimalUnique(randomList);
            }
            for (int i = length % animals.Length; i < length; i++)
            {
                randomList[i] = animals[i % animals.Length];
            }
        }
        else
        {
            for (int i = 0; i < length; i++)
            {
                randomList[i] = GetRandomAnimalUnique(randomList);
            }
        }

        return randomList;
    }

    // ------------------------------------------------------------------

    public void Awake()
    {
        // If this phylum name is empty
        // Set it to the GameOjbect's name
        name = string.IsNullOrEmpty(name) ?  name : this.gameObject.name;
    }

    public GameObject[] GetRandomUniqueAnimalList(int length)
    {
        // Get a list of unique random animals depending
        // on the given length.

        GameObject[] randomList = new GameObject[length];               // Array to hold the animals

        // NOTE: '%' is used to get the modulo
        // For example: 25 % 5 = 0 and 17 % 5 = 2

        // If the required animals is divisible by the
        // available animals, no need to get random
        // Just return all available animals and shuffle
        if (length % animals.Length == 0)                              
        {
            for (int i = 0; i < length; i++)
            {  
                // Keep looping through all available animals using '%'
                randomList[i] = animals[i % animals.Length];            
            }
        }
        // If the required animals is not divisible by the
        // available animals and is greater in length, get
        // random only for the remainder. Then just repeat
        // animals for the remaning slots, like the above IF
        else if (length > animals.Length)
        {
            // Loop for each remainder
            for (int i = 0; i < length % animals.Length; i++)
            {
                randomList[i] = GetRandomUniqueAnimal(randomList);
            }

            // All remainder accounted for, proceed like first IF
            for (int i = length % animals.Length; i < length; i++)
            {
                randomList[i] = animals[i % animals.Length];
            }
        }
        // If the required animals is lesser than the available
        // animals. Simply get a random unique animal for each
        // array iteration.
        else
        {
            for (int i = 0; i < length; i++)
            {
                // Get a random animal that isn't in the list yet
                randomList[i] = GetRandomUniqueAnimal(randomList);
            }
        }
        
        return randomList;
    }

    private GameObject GetRandomUniqueAnimal(GameObject[] existing)
    {
        // Gets random animal that is not in the list given
        GameObject animal;
        int count = 0;                                                  // Breaker for inf loops
        do
        {
            animal = GetRandomAnimal();

            count++;
            if (count >= animals.Length)                                // Fail safe breaker if already maxed available animals
                break;
        } while (existing.Contains(animal));                            // While the list already contains the animal, redo

        
        return animal;
    }

    private GameObject GetRandomAnimal()
    {
        // Gets random animal indiscriminately
        return animals[Random.Range(0, animals.Length)];
    }
}
