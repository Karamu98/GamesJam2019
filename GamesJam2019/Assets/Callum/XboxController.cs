using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

public enum XboxControllerButton
{
    RT,
    RB,
    LT,
    LB,
    A,
    B,
    X,
    Y,
    Home,
    LS,
    RS
}

public static class XboxController
{
    //public static string GetButton(XboxControllerButton a_button)
    //{
    //    var fi = a_button.GetType().GetField(a_button.ToString());
    //    var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
    //    return (attributes.Length > 0) ? attributes[0].Description : a_button.ToString();
    //}

    public static string GetButtonName(XboxControllerButton a_button)
    {
        string toReturn = "";
        switch(a_button)
        {
            case XboxControllerButton.RT:
                {
                    toReturn = "RT";
                    break;
                }
            case XboxControllerButton.RB:
                {
                    toReturn = "RB";
                    break;
                }
            case XboxControllerButton.LT:
                {
                    toReturn = "LT";
                    break;
                }
            case XboxControllerButton.LB:
                {
                    toReturn = "LB";
                    break;
                }
            case XboxControllerButton.A:
                {
                    toReturn = "A";
                    break;
                }
            case XboxControllerButton.B:
                {
                    toReturn = "B";
                    break;
                }
            case XboxControllerButton.X:
                {
                    toReturn = "X";
                    break;
                }
            case XboxControllerButton.Y:
                {
                    toReturn = "Y";
                    break;
                }
            case XboxControllerButton.Home:
                {
                    toReturn = "Home";
                    break;
                }
            case XboxControllerButton.LS:
                {
                    toReturn = "LS";
                    break;
                }
            case XboxControllerButton.RS:
                {
                    toReturn = "RS";
                    break;
                }

        }

        return toReturn;
    }
}
