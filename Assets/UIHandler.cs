﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
    public Text ForwardText;
    public Text HorizontalText;
    public Text VerticalText;

    public Text PitchText;
    public Text RollText;
    public Text YawText;

    public Text RotationalStabilisers;
    public Text MovementStabilisers;


    public void UpdateUI(Rigidbody playerShip)
    {
        ForwardText.text = "Forward: " + playerShip.velocity.z.ToString("F3");
        HorizontalText.text = "Horizontal: " + playerShip.velocity.x.ToString("F3");
        VerticalText.text = "Vertical: " + playerShip.velocity.y.ToString("F3");

        PitchText.text = "Pitch: " + playerShip.angularVelocity.x.ToString("F3");
        RollText.text = "Roll: " + playerShip.angularVelocity.z.ToString("F3");
        YawText.text = "Yaw: " + playerShip.angularVelocity.y.ToString("F3");

    }

    public void ToggleRotationalStabiliers(bool active)
    {
        if (active)
        {
            RotationalStabilisers.text = "Rotational Stabilisers: Active";
        }
        else
        {
            RotationalStabilisers.text = "Rotational Stabilisers: Inactive";
        }
    }

    public void ToggleMovementStabiliers(bool active)
    {
        if (active)
        {
            MovementStabilisers.text = "Movement Stabilisers: Active";
        }
        else
        {
            MovementStabilisers.text = "Movement Stabilisers: Inactive";
        }
    }
}