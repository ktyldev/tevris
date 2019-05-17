using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TetrisBoard))]
public class TetrisGridCollision : MonoBehaviour
{
    public GameObject camera;

    private BoxCollider collider_;
    private Camera camera_;
    private TetrisBoard board_;

    private void Awake()
    {
        collider_ = gameObject.AddComponent<BoxCollider>();
    }

    // Use this for initialization
    void Start()
    {
        board_ = GetComponent<TetrisBoard>();
        collider_.center = new Vector3
        {
            x = board_.columns / 2 - 0.5f,
            y = board_.rows / 2 - 0.5f,
            z = 0
        };
        collider_.size = new Vector3(board_.columns, board_.rows, 1);
        collider_.isTrigger = true;
        camera_ = camera.GetComponent<Camera>();
    }

    public Collider GetCollider()
    {
        return collider_;
    }

    // Update is called once per frame
    void Update()
    {
        // test code
        if (Input.GetMouseButtonDown(0))
        {
            MouseRaycast(false);     
        }

        if (Input.GetMouseButtonDown(1))
        {
            MouseRaycast(true);
        }
    }

    private void MouseRaycast(bool addPiece)
    {
        var origin = camera_.transform.position;
        var mousePos = Input.mousePosition;
        var to = camera_.ScreenToWorldPoint(new Vector3
        {
            x = mousePos.x,
            y = mousePos.y,
            z = camera_.nearClipPlane
        });

        var ray = new Ray(origin, (to - origin).normalized);

        RaycastHit? hit = null;
        var hits = Physics.RaycastAll(ray, 50, -1, QueryTriggerInteraction.Collide);
        foreach (var h in hits)
        {
            if (h.collider == collider_)
            {
                hit = h;
                break;
            }
        }

        if (!hit.HasValue)
            return;

        SpaceCast(hit.Value);
    }

    private int deadRows = 6;
    public void SpaceCast(RaycastHit hit)
    {
        var result = GetGridPosition(hit.point);
        if (result.y > board_.rows - deadRows)
        {
            return;
        }

        board_.SpawnTetromino(result.x, result.y, settled:true);
        board_.ClearLines();
    }

    private Vector2Int GetGridPosition(Vector3 hitPosition)
    {
        hitPosition /= transform.parent.localScale.x;

        hitPosition += Vector3.right * board_.columns / 2;
        hitPosition += Vector3.down * 1.5f;

        var result = new Vector2Int
        {
            x = (int)hitPosition.x,
            y = (int)hitPosition.y
        };

        return result;    
    }
}
