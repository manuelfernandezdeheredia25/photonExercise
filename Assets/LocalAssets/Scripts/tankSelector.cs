using UnityEngine;
using UnityEngine.UI;


public class TankSelector : MonoBehaviour
{
    public uiTankInfo tankInfoUI;
    public GameObject tankBaseButton;
    public GameObject tankHeavyButton;
    public GameObject tankFastButton;

    [Header("Scriptable Objects")]
    public TankInfo tankBaseSO;
    public TankInfo tankHeavySO;
    public TankInfo tankFastSO;

    [HideInInspector]
    public TankInfo selectedTank;


    public Color selectedColor = Color.white;
    public Color unselectedColor = Color.black;


    private void Start()
    {
        OnClickTankBase();
    }
    public void OnClickTankBase()
    {
        // cambiar los otros botones para que no esten seleccionados, 
        tankBaseButton.GetComponent<Image>().color = selectedColor;
        tankHeavyButton.GetComponent<Image>().color = unselectedColor;
        tankFastButton.GetComponent<Image>().color = unselectedColor;


        // cambiar tank info ui para mostrar los datos de este tanque
        tankInfoUI.ChangeTankInfo(tankBaseSO);
        //guardar la opcion seleccionada en una variable para pasarsela al connect.
        selectedTank = tankBaseSO;

    }

    public void OnClickTankHeavy()
    {
        // cambiar los otros botones para que no esten seleccionados, 
        tankBaseButton.GetComponent<Image>().color = unselectedColor;
        tankHeavyButton.GetComponent<Image>().color = selectedColor;
        tankFastButton.GetComponent<Image>().color = unselectedColor;


        // cambiar tank info ui para mostrar los datos de este tanque
        tankInfoUI.ChangeTankInfo(tankHeavySO);
        //guardar la opcion seleccionada en una variable para pasarsela al connect.
        selectedTank = tankHeavySO;
    }

    public void OnClickTankFast()
    {
        // cambiar los otros botones para que no esten seleccionados, 
        tankBaseButton.GetComponent<Image>().color = unselectedColor;
        tankHeavyButton.GetComponent<Image>().color = unselectedColor;
        tankFastButton.GetComponent<Image>().color = selectedColor;


        // cambiar tank info ui para mostrar los datos de este tanque
        tankInfoUI.ChangeTankInfo(tankFastSO);
        //guardar la opcion seleccionada en una variable para pasarsela al connect.
        selectedTank = tankFastSO;
    }
}
