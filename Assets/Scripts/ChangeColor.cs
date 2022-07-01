using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField]
    private Material colorMaterial;     //The orginal Material
    private Color32 alteredColor;       //Color were going to change the object to 

    void Start()
    {
        AlteredColor();
    }

    private void OnTriggerEnter(Collider other) {
        //Checks if object has the "Ticket" tag
        if(other.tag == "Ticket")
        {
            other.gameObject.GetComponent<Renderer>().material.color = alteredColor;
        }
    }

    //Checks which material is appled to this script and alters the color
    private void AlteredColor(){
        if(colorMaterial.name.Equals("Blue"))
        {
            alteredColor = new Color32(0, 70, 255, 1);
        } 
        else if(colorMaterial.name.Equals("Green"))
        {
            alteredColor = new Color32(100, 200, 80, 1);
        }
        else if(colorMaterial.name.Equals("Yellow"))
        {
            alteredColor = new Color32(210, 210, 50, 1);
        }
        else if(colorMaterial.name.Equals("Red"))
        {
            alteredColor = new Color32(200, 50, 50, 1);
        }
    }


}
