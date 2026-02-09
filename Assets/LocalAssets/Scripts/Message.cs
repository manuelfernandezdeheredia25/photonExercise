using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{

    public TMP_Text miMensaje;

    public void Start()
    {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }

}