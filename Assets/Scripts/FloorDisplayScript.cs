using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDisplayScript : MonoBehaviour
{
    public SpriteRenderer topLeftTrapLight1, botLeftTrapLight1, topRightTrapLight1, botRightTrapLight1, leftButtonLight1, rightButtonLight1;
    public SpriteRenderer topLeftTrapLight2, botLeftTrapLight2, topRightTrapLight2, botRightTrapLight2, leftButtonLight2, rightButtonLight2;
    public Color defaultColor, trapActiveColor, buttonActiveColor;


    public void ActivateLight(string _buttonOrTrapName)
    {
        switch (_buttonOrTrapName)
        {
            case "botRightButton":
                rightButtonLight1.color = buttonActiveColor;
                rightButtonLight2.color = buttonActiveColor;
                break;

            case "leftButton":
                leftButtonLight1.color = buttonActiveColor;
                leftButtonLight2.color = buttonActiveColor;
                break;

            case "ElecFloor1":
                topLeftTrapLight1.color = trapActiveColor;
                topLeftTrapLight2.color = trapActiveColor;
                break;

            case "ElecFloor2":
                topRightTrapLight1.color = trapActiveColor;
                topRightTrapLight2.color = trapActiveColor;
                break;

            case "ElecFloor3":
                botLeftTrapLight1.color = trapActiveColor;
                botLeftTrapLight2.color = trapActiveColor;
                break;

            case "ElecFloor4":
                botRightTrapLight1.color = trapActiveColor;
                botRightTrapLight2.color = trapActiveColor;
                break;

            default:
                break;
        }
    }

    public void DeActivateLight(string _buttonOrTrapName)
    {
        switch (_buttonOrTrapName)
        {
            case "botRightButton":
                rightButtonLight1.color = defaultColor;
                rightButtonLight2.color = defaultColor;
                break;

            case "leftButton":
                leftButtonLight1.color = defaultColor;
                leftButtonLight2.color = defaultColor;
                break;

            case "ElecFloor1":
                topLeftTrapLight1.color = defaultColor;
                topLeftTrapLight2.color = defaultColor;
                break;

            case "ElecFloor2":
                topRightTrapLight1.color = defaultColor;
                topRightTrapLight2.color = defaultColor;
                break;

            case "ElecFloor3":
                botLeftTrapLight1.color = defaultColor;
                botLeftTrapLight2.color = defaultColor;
                break;

            case "ElecFloor4":
                botRightTrapLight1.color = defaultColor;
                botRightTrapLight2.color = defaultColor;
                break;

            default:
                break;
        }
    }

}
