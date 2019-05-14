using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    // === interaction === //
    public static float VRPickupRadius = 0.3f;
    public static float VRPickupVelocityTransfer = 1.25f;
    public static float ExplosionTransferSpeed = 0.3f; // meters per second
    public static float GrenadeExplosionVelocity = 5.0f;

    // === input === //
    public static KeyCode OTButtonOnePress = KeyCode.JoystickButton0;
    public static KeyCode OTButtonTwoPress = KeyCode.JoystickButton1;
    public static KeyCode OTButtonThreePress = KeyCode.JoystickButton2;
    public static KeyCode OTButtonFourPress = KeyCode.JoystickButton3;

    public static KeyCode OTButtonStartPress = KeyCode.JoystickButton7;

    public static string OTTriggerLeftHand = "OTTriggerLeftHand";
    public static string OTTriggerLeftIndex = "OTTriggerLeftIndex";
    public static string OTTriggerRightHand = "OTTriggerRightHand";
    public static string OTTriggerRightIndex = "OTTriggerRightIndex";

    public static string OTAxisLeftVertical = "OTAxisLeftVertical";
    public static string OTAxisLeftHorizontal = "OTAxisLeftHorizontal";
    public static string OTAxisRightVertical = "OTAxisRightVertical";
    public static string OTAxisRightHorizontal = "OTAxisRightHorizontal";

    // === tags === //
    public static string PickupTag = "Pickupable";

    // === layers === //
    public static int PickupLayer = 10;
}
