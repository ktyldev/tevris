using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MoveDirection
{
    Left,
    Right
}

public enum RotateDirection
{
    Clockwise,
    Anticlockwise
}

public enum DropType
{
    Soft,
    Hard
}

public class MoveInput : UnityEvent<MoveDirection> { }
public class RotateInput : UnityEvent<RotateDirection> { }

public class TetrisInput : MonoBehaviour
{
    public InputLayout[] inputLayouts;
    public int selectedInputLayout = 0;

    public UnityEvent<MoveDirection> OnMove { get; private set; }
    public UnityEvent<RotateDirection> OnRotate { get; private set; }
    public UnityEvent OnDrop { get; private set; }
    public bool SoftDrop => Input.GetKey(inputLayout_.softDrop);
    public bool SoftDropStart => Input.GetKeyDown(inputLayout_.softDrop);

    private InputLayout inputLayout_ => inputLayouts[selectedInputLayout];

    private void Awake()
    {
        OnMove = new MoveInput();
        OnRotate = new RotateInput();
        OnDrop = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        var layout = inputLayout_;

        // drop
        // TODO: soft drop
        if (Input.GetKeyDown(layout.hardDrop))
        {
            OnDrop.Invoke();
            return;
        }

        // movement
        if (Input.GetKeyDown(layout.moveLeft))
        {
            OnMove.Invoke(MoveDirection.Left);
            return;
        }
        if (Input.GetKeyDown(layout.moveRight))
        {
            OnMove.Invoke(MoveDirection.Right);
            return;
        }

        // rotation
        if (Input.GetKeyDown(layout.rotate) || Input.GetKeyDown(layout.rotateAnticlockwise))
        {
            OnRotate.Invoke(RotateDirection.Anticlockwise);
            return;
        }
        if (Input.GetKeyDown(layout.rotateClockwise))
        {
            OnRotate.Invoke(RotateDirection.Clockwise);
            return;
        }
    }
}

[Serializable]
public struct InputLayout
{
    // move
    public KeyCode moveLeft;
    public KeyCode moveRight;
    // rotate
    public KeyCode rotate;              // defaults to rotate anti-clockwise
    public KeyCode rotateClockwise;     // anti-clockwise
    public KeyCode rotateAnticlockwise; // clockwise
    // drop
    public KeyCode hardDrop;
    public KeyCode softDrop;

}
