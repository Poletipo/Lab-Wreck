using UnityEngine;

public class FacingController : MonoBehaviour {
    public enum FacingDirections {
        Invalid = -1,
        Right,
        Left
    }

    public delegate void FacingControllerEvent(FacingController facingController);

    public FacingControllerEvent OnChangingDirection;

    private FacingDirections facingSide = FacingDirections.Invalid;

    public FacingDirections FacingSide {
        get { return facingSide; }
        set {
            if (value != facingSide) {
                Vector3 scale = transform.localScale;
                if (value == FacingDirections.Right) {
                    scale.x = Mathf.Abs(scale.x);
                }
                else {
                    scale.x = -Mathf.Abs(scale.x);
                }

                transform.localScale = scale;

                OnChangingDirection?.Invoke(this);
                facingSide = value;
            }
        }
    }

    public int GetIntDirection()
    {
        int dir = 1;

        if (FacingSide == FacingDirections.Left) {
            dir = -1;
        }
        return dir;
    }


    private void Start()
    {
        FacingSide = FacingDirections.Right;
    }

}
