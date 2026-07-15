using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Left (Red)
    public bool RedPressed =>
        Input.GetMouseButton(0);

    public bool RedDown =>
        Input.GetMouseButtonDown(0);

    public bool RedUp =>
        Input.GetMouseButtonUp(0);

    // Right (Blue)
    public bool BluePressed =>
        Input.GetMouseButton(1);

    public bool BlueDown =>
        Input.GetMouseButtonDown(1);

    public bool BlueUp =>
        Input.GetMouseButtonUp(1);
}