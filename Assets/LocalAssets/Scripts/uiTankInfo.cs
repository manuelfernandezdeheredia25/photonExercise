using System;
using UnityEngine;
using UnityEngine.UI;

public class uiTankInfo : MonoBehaviour
{

    public GameObject image, attackSlider, lifeSlider, speedSlider, rateSlider;
    public void ChangeTankInfo(TankInfo tankSO)
    {
        attackSlider.GetComponent<Slider>().value = tankSO.damage;
        lifeSlider.GetComponent<Slider>().value = tankSO.life;
        speedSlider.GetComponent<Slider>().value = tankSO.speed;
        rateSlider.GetComponent<Slider>().value = tankSO.fireRate;
    }


}
