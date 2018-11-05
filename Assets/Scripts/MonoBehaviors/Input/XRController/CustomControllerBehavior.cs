﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEditor;

public class CustomControllerBehavior : MonoBehaviour {

    [SerializeField]
    private float _maxInteractionDistance = 20.0f;

    [SerializeField]
    public GameObject cursor;

    // TODO Make this private setter.
    [SerializeField]
    public GameObject cameraRig;

    [SerializeField]
    private XRMenu _menu;

    [SerializeField]
    private float _speedMultiplier = 0.1f;

    public SteamVR_TrackedController controller { get; private set; }

    private bool _padClicked = false;

    private void OnEnable() {
        controller = GetComponent<SteamVR_TrackedController>();
        controller.TriggerClicked += TriggerClickedHandler;
        controller.TriggerUnclicked += TriggerUnclickedHandler;
        controller.PadClicked += PadClickedHandler;
        controller.PadUnclicked += PadUnclickedHandler;
        controller.MenuButtonClicked += MenuButtonClickedHandler;
        controller.Gripped += GrippedHandler;
        controller.Ungripped += UngrippedHandler;
    }

    private void OnDisable() {
        controller.TriggerClicked -= TriggerClickedHandler;
        controller.TriggerUnclicked -= TriggerUnclickedHandler;
        controller.PadClicked -= PadClickedHandler;
        controller.PadUnclicked -= PadUnclickedHandler;
        controller.MenuButtonClicked -= MenuButtonClickedHandler;
        controller.Gripped -= GrippedHandler;
        controller.Ungripped -= UngrippedHandler;
    }

    #region Controller Event Handlers

    private void TriggerClickedHandler(object sender, ClickedEventArgs e) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxInteractionDistance)) {
            XRInteractableObject obj = hit.transform.GetComponent<XRInteractableObject>();
            if (obj != null && obj.triggerDown) {
                // TODO Verify sender class.
                obj.OnTriggerDown(this, hit.point, hit.normal, e);
                //obj.OnTriggerDoubleClick(this, hit.point, e);
            }
        }
    }

    private void TriggerUnclickedHandler(object sender, ClickedEventArgs e) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxInteractionDistance)) {
            XRInteractableObject obj = hit.transform.GetComponent<XRInteractableObject>();
            if (obj != null && obj.triggerUp) {
                // TODO Verify sender class.
                obj.OnTriggerUp(this, hit.point, hit.normal, e);
            }
        }
    }

    private void PadClickedHandler(object sender, ClickedEventArgs e) {
        Debug.Log("Pad clicked at (" + e.padX + ", " + e.padY + ")");
        _padClicked = true;
    }

    private void PadUnclickedHandler(object sender, ClickedEventArgs e) {
        Debug.Log("Pad unclicked at (" + e.padX + ", " + e.padY + ")");
        _padClicked = false;
    }

    private void MenuButtonClickedHandler(object sender, ClickedEventArgs e) {
        if (_menu.gameObject.activeSelf) {
            _menu.gameObject.SetActive(false);
        }
        else {
            Camera eye = cameraRig.GetComponentInChildren<Camera>();
            Ray forward = new Ray(eye.transform.position, Vector3.Scale(eye.transform.forward, new Vector3(1, 0 ,1)));
            Vector3 menuPosition = forward.GetPoint(_menu.distance);
            menuPosition.y = _menu.height;
            _menu.transform.position = menuPosition;
            _menu.transform.forward = menuPosition - eye.transform.position;
            _menu.gameObject.SetActive(true);
        }
    }

    private void GrippedHandler(object sender, ClickedEventArgs e) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxInteractionDistance)) {
            XRInteractableObject obj = hit.transform.GetComponent<XRInteractableObject>();
            if (obj != null && obj.gripDown) {
                // TODO Verify sender class.
                obj.OnGripDown(this, hit.point, hit.normal, e);
            }
        }
    }

    private void UngrippedHandler(object sender, ClickedEventArgs e) {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxInteractionDistance)) {
            XRInteractableObject obj = hit.transform.GetComponent<XRInteractableObject>();
            if (obj != null && obj.gripUp) {
                // TODO Verify sender class.
                obj.OnGripUp(this, hit.point, hit.normal, e);
            }
        }
    }

    #endregion


    private void Update() {

        // Update player movement
        if (_padClicked) {

            // Get the pad position from the controller device.
            SteamVR_Controller.Device device = SteamVR_Controller.Input((int)controller.controllerIndex);
            Vector2 axis = device.GetAxis();

            // Move the player based on controller direction and pad position.
            // Movement is limited along the xz-plane.
            Vector3 direction = Vector3.Scale(transform.forward, new Vector3(1, 0, 1));
            cameraRig.transform.position += (axis.y > 0 ? 1 : -1) * _speedMultiplier * direction;

        }

        RaycastHit hit;
        // TODO Save raycast result as global variable so that we dont need another raycast when buttons are pressed.
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxInteractionDistance)) {
            XRInteractableObject obj = hit.transform.GetComponent<XRInteractableObject>();
            if (obj != null) {
                obj.OnCursorOver(this, hit.point, hit.normal);
                cursor.transform.position = hit.point;
                cursor.SetActive(true);
            }
            else {
                cursor.SetActive(false);
            }
        }
        else {
            cursor.SetActive(false);
        }

    }

}