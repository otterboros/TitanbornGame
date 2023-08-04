using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DoorInteractionKit
{
    public class DoorInteractable : MonoBehaviour
    {
        //Interactable Type
        public enum InteractableType { Door, Drawer }
        [SerializeField] private InteractableType interactableType = InteractableType.Door;

        //Door / Door Pivot Transform
        [SerializeField] private Transform doorTransform = null;

        //Door Settings
        [SerializeField] private bool doorOpenRight = true; // False to open left
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float openSpeed = 3f;
        [SerializeField] private float closeSpeed = 3f;

        //Drawer Settings
        public enum DrawerDirection { Forward, Backward, Left, Right, Up, Down }
        [SerializeField] private DrawerDirection drawerOpenDirection = DrawerDirection.Forward;
        [SerializeField] private float drawerOpenSpeed = 3f;
        [SerializeField] private float drawerCloseSpeed = 3f;
        [SerializeField] private float slideDistance = 0.5f;

        // Plank settings
        [SerializeField] private int plankCount = 0; // Number of planks blocking the door
        [SerializeField] private string doorPlankedText = "Blocked by Planks";

        //Lock Settings
        [SerializeField] private bool isLocked = false;
        [SerializeField] private string lockedDoorText = "Locked";
        [SerializeField] private Sound doorLockedSound = null;
        [SerializeField] private Key keyScriptable = null;
        [SerializeField] private bool removeKeyAfterUse = false;
        [SerializeField] private Sound doorUnlockSound = null;

        //Open Door Audio
        [SerializeField] private Sound doorOpenSound = null;
        [SerializeField] private float openDelay = 0;
        
        //Close Door Audio
        [SerializeField] private Sound doorCloseSound = null;
        [SerializeField] private float closeDelay = 0.8f;

        //Spawnable Items
        [SerializeField] private UnityEvent onDrawerOpen;

        private bool hasSpawnedItems = false;
        private Vector3 closedPosition;
        private Vector3 openPosition;
        private bool isUnlocking = false;
        private bool isOpening = false;
        private bool isClosing = false;
        private Quaternion closedRotation;
        private Quaternion openRotation;

        private bool doorOpen = false;

        private void Start()
        {
            if (doorTransform == null)
            {
                Debug.LogWarning("Door Transform not set in the DoorController");
                return;
            }

            switch (interactableType)
            {
                case InteractableType.Door:
                    closedRotation = doorTransform.rotation;
                    openRotation = Quaternion.Euler(doorTransform.eulerAngles + new Vector3(0, doorOpenRight ? -openAngle : openAngle, 0));
                    break;

                case InteractableType.Drawer:
                    closedPosition = doorTransform.position;
                    openPosition = closedPosition + GetDirection(drawerOpenDirection) * slideDistance;
                    break;
            }
        }

        private Vector3 GetDirection(DrawerDirection dir)
        {
            switch (dir)
            {
                case DrawerDirection.Forward: return doorTransform.forward;
                case DrawerDirection.Backward: return -doorTransform.forward;
                case DrawerDirection.Left: return -doorTransform.right;
                case DrawerDirection.Right: return doorTransform.right;
                case DrawerDirection.Up: return doorTransform.up;
                case DrawerDirection.Down: return -doorTransform.up;
                default: return Vector3.forward;
            }
        }

        private void Update()
        {
            if (doorTransform == null)
            {
                return;
            }

            switch (interactableType)
            {
                case InteractableType.Door:
                    if (isOpening)
                    {
                        doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, openRotation, Time.deltaTime * openSpeed);
                        if (Quaternion.Angle(doorTransform.rotation, openRotation) < 0.1f)
                        {
                            doorTransform.rotation = openRotation;
                            isOpening = false;
                        }
                    }
                    else if (isClosing)
                    {
                        doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, closedRotation, Time.deltaTime * closeSpeed);
                        if (Quaternion.Angle(doorTransform.rotation, closedRotation) < 0.1f)
                        {
                            doorTransform.rotation = closedRotation;
                            isClosing = false;
                        }
                    }
                    break;

                case InteractableType.Drawer:
                    if (isOpening && interactableType == InteractableType.Drawer)
                    {
                        doorTransform.position = Vector3.MoveTowards(doorTransform.position, openPosition, Time.deltaTime * drawerOpenSpeed);
                        if (Vector3.Distance(doorTransform.position, openPosition) < 0.01f)
                        {
                            doorTransform.position = openPosition;
                            isOpening = false;
                        }
                    }
                    else if (isClosing && interactableType == InteractableType.Drawer)
                    {
                        doorTransform.position = Vector3.MoveTowards(doorTransform.position, closedPosition, Time.deltaTime * drawerCloseSpeed);
                        if (Vector3.Distance(doorTransform.position, closedPosition) < 0.01f)
                        {
                            doorTransform.position = closedPosition;
                            isClosing = false;
                        }
                    }
                    break;
            }
        }

        public void CheckDoor()
        {
            if (plankCount > 0)
            {
                // Door is blocked by planks
                DoorAudioManager.instance.Play(doorLockedSound);
                ShowLockedPrompt(doorPlankedText);
                return;
            }

            if (isLocked)
            {
                // Check if the key exists in the inventory
                if (DoorInventory.instance._keyList.Contains(keyScriptable))
                {
                    // Remove the key from the inventory if specified
                    if (removeKeyAfterUse)
                    {
                        DoorInventory.instance.RemoveKey(keyScriptable);
                        ShowLockedPrompt(keyScriptable._KeyName + " Removed");
                    }

                    // Play unlock sound
                    DoorAudioManager.instance.Play(doorUnlockSound);
                    StartCoroutine(UnlockDoor());
                    return;
                }
                else
                {
                    // Play locked sound and show UI text
                    DoorAudioManager.instance.Play(doorLockedSound);
                    ShowLockedPrompt(lockedDoorText);
                    return;
                }
            }

            // Ignore interaction if door is currently moving
            if (isOpening || isClosing || isUnlocking)
                return;

            if (doorOpen)
            {
                isClosing = true;
                DoorAudioManager.instance.Play(doorCloseSound, closeDelay);
            }
            else
            {
                isOpening = true;
                DoorAudioManager.instance.Play(doorOpenSound, openDelay);

                if (!hasSpawnedItems)
                {
                    onDrawerOpen?.Invoke();
                    hasSpawnedItems = true;
                }
            }

            doorOpen = !doorOpen;
        }

        public void RemovePlank()
        {
            if (plankCount > 0)
            {
                plankCount--;
                if (plankCount == 0)
                {
                    // You might play a sound effect or animation here
                }
            }
        }

        private IEnumerator UnlockDoor()
        {
            isUnlocking = true;

            // Wait for the duration of the unlock sound
            yield return new WaitForSeconds(doorUnlockSound.clip.length);

            isLocked = false;
            isUnlocking = false;
        }

        public void ShowLockedPrompt(string promptText)
        {
            DoorUIManager.instance.ShowDoorText(promptText);
        }
    }
}