using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderToText : MonoBehaviour
{

    public Slider senSlider;
    public Slider volSlider;

    public TMP_InputField senInputField;
    public TMP_InputField volInputField;

    static public float senValue;
    static public int volValue;



    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Sensitivity")){
            senSlider.value = PlayerPrefs.GetFloat("Sensitivity");
        }
        if(PlayerPrefs.HasKey("Volume")){
            volSlider.value = PlayerPrefs.GetInt("Volume");
        }
    }

    // Update is called once per frame
    void Update()
    {
        senValue = ((float)System.Math.Round(senSlider.value, 2));
        volValue = Mathf.RoundToInt(volSlider.value);

        senInputField.text = senValue.ToString();
        volInputField.text = volValue.ToString();
    }
}
