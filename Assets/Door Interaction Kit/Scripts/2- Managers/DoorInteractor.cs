using UnityEngine;

namespace DoorInteractionKit
{
    public class DoorInteractor : MonoBehaviour
    {
        //[Header("Raycast Features")]
        //[SerializeField] private float rayDistance = 5;

        [Header("Raycast Features")]
        [SerializeField] private KeyCode interactionKey;

        private DoorItem doorItem;
        private Camera _camera;

        private PlayerController playerController;

        void Start()
        {
            _camera = GetComponent<Camera>();
            playerController = FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        }

        void Update()
        {
            // TODO: Plug this into PlayerController/new input system and consolidate
            // Added grounded condition so player can only open doors when grounded
            // TODO: Add _input.jump so UI turns off if you're ever in the air (probs handle this when we consolidate
            if(playerController.Grounded)
            {
                // changed to coordinate with PlayerController.selectRange
                if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out RaycastHit hit, PlayerController.selectRange))
                {
                    var _doorItem = hit.collider.GetComponent<DoorItem>();
                    if (_doorItem != null)
                    {
                        doorItem = _doorItem;
                        doorItem.ShowObjectName(true);
                        HighlightCrosshair(true);
                    }
                    else
                    {
                        ClearItem();
                    }
                }
                else
                {
                    ClearItem();
                }

                if (doorItem != null)
                {
                    // TODO: Plug this into the new input system and consolidate
                    if (Input.GetKeyDown(interactionKey))
                    {
                        doorItem.ObjectInteraction();
                    }
                }
            }
        }

        private void ClearItem()
        {
            if (doorItem != null)
            {
                doorItem.ShowObjectName(false);
                doorItem = null;
                HighlightCrosshair(false);
            }
        }

        void HighlightCrosshair(bool on)
        {
            DoorUIManager.instance.HighlightCrosshair(on);
        }
    }
}
