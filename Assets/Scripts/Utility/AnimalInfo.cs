using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
// This class handles all functions related to
// Animal info and relation to the data json
// use it to load animal info and other functions
public static class AnimalInfo{

    
	public static void LoadAnimalInfo(Phylum phylum,bool isStatic)
    {
        // TODO: Handle JToken returned as JObject, data file might be corrupted
        // Load the data file using JSONParser
        JArray jArray = (JSONParser.ReadJson("data")).ToObject<JArray>();

        // TODO: Better parsing algo
        // Get the animal info belonging the phylum
        var animals =
            from p in jArray
            where p["name"].ToString() == phylum.name
            select (JArray)p["animals"];

        // For each result in the returned JArray of animal set
        foreach (var result in animals)
        {
            // For each animal in the returned result
            foreach (var animal in result)
            {
                // Check where to assign the info, loop thru all animals in phylum
                foreach (var pAnimal in phylum.animals)
                {
                    // Check the name of the animal for match
                    if (pAnimal.GetComponent<Animal>().Name == animal["Scientific Name"].ToString() || pAnimal.GetComponent<Animal>().Name == animal["Common Name"].ToString())
                    {
                        JObject o = JObject.Parse(animal.ToString());               // Convert the given animal to a JObject

                        string info = "";
                        // For each info about the returned animal, build string info
                        foreach (var item in o)
                        {
                            info += item.Key + ": " + item.Value.ToString() + "\n";
                        }
                        
                        if(!isStatic)
                            pAnimal.GetComponent<Animal>().info = info;                 // Assign final info string to pAnimal
                    }
                }
            }
        }
    }

}