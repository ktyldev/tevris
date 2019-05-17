using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {
    // === interaction === //
    public const float VRPickupRadius = 0.3f;

    public static Vector2 VRVelocityScaleBounds = new Vector2( 3.5f, 8.0f );
    public static Vector2 VRVelocityScaleValues = new Vector2( 1.35f, 2.75f );

    public const float VRHandSnapThreshold = 0.15f;
    public const float VRHandFloatLerp = 0.15f;
    public const float VRHandHeldLerp = 0.4f;
    public const float VRWhooshThreshold = 3.0f;

    public const float VRLaserPickupMaxAngle = 3.8f;
    public const float VRLaserPickupMaxDistance = 1000.0f;

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

    public const string FireLeft = "FireLeft";
    public const string FireRight = "FireRight";


    // === tags === //
    public const string PickupTag = "Pickupable";
    public const string VRCameraTag = "VRCamera";
    public const string SoundEngineTag = "SoundEngine";

    // === layers === //
    public const int PickupLayer = 10;
    public const int TetrisGrid = 11;

    // === resources === //
    public const string ExplosionDebrisPrefab = "explosion_debris";

    // === sound === //
    public const string SFXClick = "click";
    public const string SFXExplosion = "explosion";
    public const string SFXArrowFire = "arrow_fire";
    public const string SFXArrowHit = "arrow_hit";

    public const string SFXTetrisMove = "tetris_move";
    public const string SFXTetrisRotate = "tetris_rotate";
    public const string SFXTetrisDrop = "tetris_hard_drop";
    public const string SFXTetrisSoftDrop = "tetris_hard_drop2";

    public static readonly string[] LevelTracks = {
        "8bit_2",
        "8bit_3",
        "8bit_4",
        "8bit_5",
        "8bit_8",
        //"essence_of_good_things",
        //"fancy_cakes",
        //"inside_dreams",
    };

    public static readonly string[] menuTracks =
    {
        "in_the_spinning_world"
    };
}
