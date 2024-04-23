using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.IO;

public class BookContent : MonoBehaviour
{
    [Space]
    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [Space]
    [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;

    public InputActionProperty nextPageAction;
    public InputActionProperty previousPageAction;

    private string content;

    private void OnEnable()
    {
        // Load content from file
        string contentFilePath = Path.Combine(Application.dataPath, "Resources/content.txt");
        content = File.ReadAllText(contentFilePath);

        SetupContent();
        UpdatePagination();

        // Enable input actions
        nextPageAction.action.Enable();
        previousPageAction.action.Enable();
    }

    private void OnDisable()
    {
        // Disable input actions
        nextPageAction.action.Disable();
        previousPageAction.action.Disable();
    }

    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }

    private void UpdatePagination()
    {
        leftPagination.text = leftSide.pageToDisplay.ToString();
        rightPagination.text = rightSide.pageToDisplay.ToString();
    }

    private void Update()
    {
        // Check for input action triggers
        if (nextPageAction.action.triggered)
        {
            NextPage();
        }
        else if (previousPageAction.action.triggered)
        {
            PreviousPage();
        }
    }

    public void PreviousPage()
    {
        if (leftSide.pageToDisplay < 1)
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1)
            leftSide.pageToDisplay -= 2;
        else
            leftSide.pageToDisplay = 1;

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;

        UpdatePagination();
    }

    public void NextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;

        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1)
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }

        UpdatePagination();
    }
}