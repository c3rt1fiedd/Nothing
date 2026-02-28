using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI mainText;

    private float timer;
    private GameState currentState;
    private Vector3 lastMousePosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        SetState(GameState.WaitingToStart);
    }

 void Update()
{
    switch (currentState)
    {
        case GameState.WaitingToStart:
            if (Input.anyKeyDown)
                SetState(GameState.DoingNothing);
            break;

        case GameState.DoingNothing:
            timer += Time.deltaTime;
            mainText.text = $"You've been doing Nothing for {timer:F2} seconds."; // always overwrite

            if (DetectedSomething() || !Application.isFocused)
                SetState(GameState.Lost);

            lastMousePosition = Input.mousePosition;
            break;

        case GameState.Lost:
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            else if (Input.anyKeyDown)
                SetState(GameState.DoingNothing); // this now fully clears old text
            break;
    }
}

    void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.WaitingToStart:
                mainText.text = "Press any key to start doing Nothing.";
                break;

            case GameState.DoingNothing:
                timer = 0f;
                break;

            case GameState.Lost:
                mainText.text =
                $"You did Something.\n" +
                $"You did Nothing for {timer:F2} seconds.\n\n" +
                "Press any key to start doing Nothing again.\n" +
                "Press ESC to exit.";
            break;
        }
    }

    bool DetectedSomething()
    {
        if (Input.anyKeyDown)
            return true;
        if (Input.mouseScrollDelta.y != 0)
            return true;
        if (Input.mousePosition != lastMousePosition)
            return true;

        return false;
    }
}

public enum GameState
{
    WaitingToStart,
    DoingNothing,
    Lost
}