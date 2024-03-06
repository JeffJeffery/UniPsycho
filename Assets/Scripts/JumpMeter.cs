using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{

    public static JumpMeter singleton;
    private Slider mySlider;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;
        mySlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateChargeMeter(float percentCharged)
    {
        mySlider.value = percentCharged;
    }

}
