using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class OpenBook : MonoBehaviour
{
    public InputActionProperty leftHandGrab;
    public InputActionProperty rightHandGrab;
    public GameObject pages;
    public GameObject images;
    public XRGrabInteractable book;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float leftValue = leftHandGrab.action.ReadValue<float>();
        float rightValue = rightHandGrab.action.ReadValue<float>();
        if (book.isSelected)
        {
            if (leftValue > 0 && rightValue >0) { pages.SetActive(true); images.SetActive(true); }
            else { pages.SetActive(false); images.SetActive(false); }
        }
    }

}
