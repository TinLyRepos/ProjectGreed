using UnityEngine;
using TMPro;

public class DialogManager : SingletonMonobehaviour<DialogManager>
{
    [SerializeField] private Transform dialogPanel;
    [SerializeField] private TextMeshProUGUI dialogText;

    private string[] dialogLines;
    private int lineIndex;
    public bool activated = false;

    //===========================================================================
    private void Update()
    {
        if (!activated)
        {
            return;
        }
        if (dialogPanel.gameObject.activeSelf == false)
            return;

        if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Space))
        {
            lineIndex++;
            if(dialogLines == null)
            {
                lineIndex = 0;

                SetDialogPanelActiveState(false);

                return;
            }
            if (lineIndex >= dialogLines.Length)
            {
                lineIndex = 0;

                SetDialogPanelActiveState(false);
                dialogLines = new string[0];
                return;
            }

            dialogText.SetText(dialogLines[lineIndex]);
        }
    }

    //===========================================================================
    public void SetDialogPanelActiveState(bool newBool)
    {
        if (newBool)
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Pause;
        else
            SceneControlManager.Instance.CurrentGameplayState = GameplayState.Ongoing;

        dialogPanel.gameObject.SetActive(newBool);
    }

    public void SetDialogLines(string[] newDialogLines)
    {
        dialogLines = newDialogLines;

        dialogText.SetText(dialogLines[lineIndex]);
    }
    public void SetDialogLine(string newDialogLines)
    {
        dialogText.SetText(newDialogLines);
    }
}
