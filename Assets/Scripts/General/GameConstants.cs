using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    // === interaction === //
    public const float VRPickupRadius = 0.3f;

    public static Vector2 VRVelocityScaleBounds = new Vector2( 2.0f, 8.0f );
    public static Vector2 VRVelocityScaleValues = new Vector2( 1.25f, 3.25f );

    public const float VRPickupVelocityTransfer = 3.25f;

    public const float VRLaserPickupMaxAngle = 3.8f;
    public const float VRLaserPickupMaxDistance = 50.0f;

    public const float ExplosionTransferSpeed = 0.3f; // meters per second
    public const float GrenadeExplosionVelocity = 2.0f;

    // === input === //
    public const KeyCode OTButtonOnePress = KeyCode.JoystickButton0;
    public const KeyCode OTButtonTwoPress = KeyCode.JoystickButton1;
    public const KeyCode OTButtonThreePress = KeyCode.JoystickButton2;
    public const KeyCode OTButtonFourPress = KeyCode.JoystickButton3;

    public const KeyCode OTButtonStartPress = KeyCode.JoystickButton7;

    public const string OTTriggerLeftHand = "OTTriggerLeftHand";
    public const string OTTriggerLeftIndex = "OTTriggerLeftIndex";
    public const string OTTriggerLeftIndexTouch = "OTTriggerLeftIndexTouch";
    public const string OTTriggerLeftIndexNearTouch = "OTTriggerLeftIndexNearTouch";

    public const string OTTriggerRightHand = "OTTriggerRightHand";
    public const string OTTriggerRightIndex = "OTTriggerRightIndex";
    public const string OTTriggerRightIndexTouch = "OTTriggerRightIndexTouch";
    public const string OTTriggerRightIndexNearTouch = "OTTriggerRightIndexNearTouch";

    public const string OTAxisLeftVertical = "OTAxisLeftVertical";
    public const string OTAxisLeftHorizontal = "OTAxisLeftHorizontal";
    public const string OTAxisRightVertical = "OTAxisRightVertical";
    public const string OTAxisRightHorizontal = "OTAxisRightHorizontal";


    // === tags === //
    public const string PickupTag = "Pickupable";

    // === layers === //
    public const int PickupLayer = 10;
}
