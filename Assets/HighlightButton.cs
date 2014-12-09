using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HighlightButton : MonoBehaviour
{
    private void Update()
    {
        if (GetComponent<SpellButtonLogic>().CurrentCharges >= 1)
        {
            ColorBlock colorBlock = GetComponent<Button>().colors;
            
            colorBlock.normalColor = Time.timeSinceLevelLoad % 2.0f > 1.0f ? Color.white : Color.yellow;
            
            GetComponent<Button>().colors = colorBlock;
        }
        else
        {
            ColorBlock colorBlock = GetComponent<Button>().colors;

            colorBlock.normalColor = Color.white;

            GetComponent<Button>().colors = colorBlock;
        }
    }
}
