﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    private Grid grid;
    private Unit selectedUnit;

    private AudioSource audioSource;
    public AudioClip audioMoveCursor;
    public AudioClip audioSelect;
    public AudioClip audioFailMove;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        // if key was pressed (similar to onpress, but remember a vector is returned
        // so 2 keys at the same time (up and left) is a combo, not 2 separate calls.
        if (context.phase.Equals(InputActionPhase.Started))
        {
            var controlDirection = context.ReadValue<Vector2>();
            Debug.Log("movementdirection is: " + controlDirection);

            Vector3 movementDirection = new Vector3();
            if (controlDirection.x >= .5)
            {
                Debug.Log("right was pressed");
                movementDirection = new Vector3(1, 0, 0);
            }
            if (controlDirection.x <= -.5)
            {
                Debug.Log("left was pressed");
                movementDirection = new Vector3(-1, 0, 0);
            }
            if (controlDirection.y >= .5)
            {
                Debug.Log("up was pressed");
                movementDirection = new Vector3(0, 1, 0);
            }
            if (controlDirection.y <= -.5)
            {
                Debug.Log("down was pressed");
                movementDirection = new Vector3(0, -1, 0);
            }
            this.transform.Translate(movementDirection);

            bool isValidLocation = !grid.isOutOfBounds((int)this.transform.position.x, (int)this.transform.position.y);

            // if the move is invalid, reverse the transform translation
            if (!isValidLocation)
            {
                audioSource.PlayOneShot(audioFailMove);
                this.transform.Translate(movementDirection * -1);
            }
            else
            {
                audioSource.PlayOneShot(audioMoveCursor);
            }
        }
    }

    public void Select(InputAction.CallbackContext context)
    {
        Unit unitAtCursor = grid.Get((int)this.transform.position.x, (int)this.transform.position.y);
        if (context.phase.Equals(InputActionPhase.Started))
        {
            // if unit is already selected, pop up menu with options.
            if (selectedUnit != null &&
                unitAtCursor == null)
            {
                // TODO: menu should appear

                // wait option causes the unit to be moved and unselected.
                selectedUnit.Move((int)this.transform.position.x, (int)this.transform.position.y);
                selectedUnit = null;
                audioSource.PlayOneShot(audioSelect);
                Debug.Log("unselected unit.");
            }
            // if unit already selected, but there is already a unit in the cursor's location,
            // do not allow the unit to be moved here.
            else if (selectedUnit != null &&
                unitAtCursor != null)
            {
                audioSource.PlayOneShot(audioFailMove);
                Debug.Log($"Failed to move unit {selectedUnit} since there is already a unit at this location.");
            }
            // no unit is selected, attempt to select a unit at the cursor's location
            else //if (selectedUnit == null)
            {
                // get the unit under the selector.
                selectedUnit = grid.Get((int)this.transform.position.x, (int)this.transform.position.y);
                audioSource.PlayOneShot(audioSelect);
                Debug.Log($"selected unit: {selectedUnit}");
            }
        }
    }
}
