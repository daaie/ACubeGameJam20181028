using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CBUIHP : MonoBehaviour {

    public int maxHp;
    int currentHp;  

    public int CurrentHP {
        get
        {
            return currentHp;
        }

        set
        {
            currentHp = value;
            hpSlider.value = CurrentHP;
        }
    }

    public Slider hpSlider;

    // Use this for initialization
    void Start () {
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
    }

    private void OnGUI()
    {
        //hpSlider.value = CurrentHP;
    }

    // Update is called once per frame
    void Update () {
        transform.LookAt(Camera.main.transform.position);
	}

    public void ResetHP()
    {

    }
}
