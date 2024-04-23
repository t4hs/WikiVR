using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class showKeyboard : MonoBehaviour
{
    private TMP_InputField searchBar;
    
    public float distance = 0.5f;
    public float yOffset = -0.5f;

    public Transform positionSource;

    // Start is called before the first frame update
    void Start()
    {
        searchBar = GetComponent<TMP_InputField>();
        searchBar.onSelect.AddListener(x => openKeyboard());

    }

    // Update is called once per frame
    public void openKeyboard()
    {
        NonNativeKeyboard.Instance.InputField = searchBar;
        NonNativeKeyboard.Instance.PresentKeyboard(searchBar.text);

        Vector3 direction = positionSource.forward;
        direction.y = 0;
        direction.Normalize();

        Vector3 targetPostion = positionSource.position + direction * distance  + Vector3.up * yOffset;

        NonNativeKeyboard.Instance.RepositionKeyboard(targetPostion);
        setCursorAlpha(1);
        NonNativeKeyboard.Instance.OnClosed += Instance_OnClosed;
    }

    private void Instance_OnClosed(object sender, System.EventArgs e)
    {
        setCursorAlpha(0);
        NonNativeKeyboard.Instance.OnClosed -= Instance_OnClosed;
    }

    public void setCursorAlpha(float alpha)
    {
        searchBar.customCaretColor = true;
        Color cursorColor = searchBar.caretColor;
        cursorColor.a = alpha;
        searchBar.caretColor = cursorColor;
    }
}
