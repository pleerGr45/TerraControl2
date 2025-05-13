using Unity.VisualScripting;
using UnityEngine;

public class CursorChangeSelectScript : MonoBehaviour
{
    
    [SerializeField] private Texture2D cursor_default;
    
    [SerializeField] private Texture2D cursor_entered;
    [SerializeField] private Texture2D cursor_pressed;
    [DoNotSerialize] private bool isPressed;
    [DoNotSerialize] private bool isEntered;

    void Awake()
    {
        isPressed = false;
        isEntered = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable() 
    {
        isPressed = false;
        isEntered = false;
        Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.ForceSoftware);
    }



    public void OnMouseEnter() {
        isEntered = true;
        if (!isPressed)
            Cursor.SetCursor(cursor_entered, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseExit() {
        isEntered = false;
        if(!isPressed) 
            Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseDown() {
        isPressed = true;
        Cursor.SetCursor(cursor_pressed, Vector2.zero, CursorMode.ForceSoftware);
    }

    public void OnMouseUp() {
        isPressed = false;
        if (isEntered)
            Cursor.SetCursor(cursor_entered, Vector2.zero, CursorMode.ForceSoftware);
        else
            Cursor.SetCursor(cursor_default, Vector2.zero, CursorMode.ForceSoftware);
    }
}
