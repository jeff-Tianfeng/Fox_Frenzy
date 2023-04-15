using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles the behaviour of the instructions menu
/// </summary>
public class InstructionsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject keyboard;

    [SerializeField]
    private GameObject gamepad;

    [SerializeField]
    private GameObject description;

    [SerializeField]
    private TMP_Dropdown controlTypeSelection;

    [SerializeField]
    private TextMeshProUGUI altButtonText;

    private GameObject currentControlType;

    private bool shouldShowControls = false;

    private void Awake()
    {
        currentControlType = controlTypeSelection.value == 0 ? keyboard : gamepad;
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Changes which control scheme is shown on the screen
    /// </summary>
    /// <param name="index"></param>
    public void ChangeControlType(System.Int32 index)
    {
        currentControlType.SetActive(false);

        switch (index)
        {
            case 0:
                currentControlType = keyboard;
                break;
            case 1:
                currentControlType = gamepad;
                break;
            default:
                break;
        }

        if (shouldShowControls)
        {
            currentControlType.SetActive(true);
        }
    }

    public void ToggleControlsVisiblity()
    {
        shouldShowControls = !shouldShowControls;

        description.SetActive(!shouldShowControls);
        currentControlType.SetActive(shouldShowControls);
        controlTypeSelection.gameObject.SetActive(shouldShowControls);

        altButtonText.text = shouldShowControls ? "Instructions" : "Controls";
    }
}
