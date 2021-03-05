﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ControllerSelection;
using System.ComponentModel;

namespace QuestAppLauncher
{
    public class ScrollRectOverride : ScrollRect, IMoveHandler, IPointerClickHandler, IScrollHandler, IDragHandler
    {
        // Scrolling speed multiplier
        private const float SpeedMultiplier = 15f;

        // Maximum speed multiplier
        private const float MaxSpeedMultiplier = 150f;

        // Speed multiplier increase percentage per frame
        // How the value is calculated: We want to increase from 15 -> 150 in ~2 sec. At 90Hz, that's ~180 frames. So 150=15*(x)^180.
        private const float SpeedMultiplierIncrease = 1.0128f;

        // Last frame's speed multiplier
        private float lastSpeedMultiplier = 0f;

        // Height of a single cell in the grid
        private float cellHeight = 0f;

        // Tracking space used for ray cast
        public Transform trackingSpace = null;

        // Box collider used for ray cast
        private BoxCollider boxCollider = null;

        // Whether the pointer is within bounds of the scroll rect
        private bool isInBounds = false;

        // Parent OVRRawRayCaster
        private OVRRawRaycaster parentRawRaycaster = null;

        void Start()
        {
            this.cellHeight = this.transform.GetComponentInChildren<GridLayoutGroup>().cellSize.y;
            this.boxCollider = GetComponent<BoxCollider>();

            // Find parent OVRRawRayCaster
            Transform t = this.gameObject.transform;
            while (t.parent != null)
            {
                parentRawRaycaster = t.parent.GetComponent<OVRRawRaycaster>();
                if (parentRawRaycaster != null)
                {
                    break;
                }
                t = t.parent.transform;
            }
        }

        void Update()
        {
            // The scroll view has a viewport that masks the UI that is outside the scroll view.
            // However, it does not filter any ray casting that is outside the mask!
            // This means that the box colliders of the individual cells still get hit outside the scroll view itself,
            // which can interfer with the tabs above the scroll view.
            //
            // To fix this issue, we cast a ray from current pointer to the scroll view's box collider.
            // If we get a hit, it means we're inside the scroll view - so we enable all the children box
            // colliders, which will behave as expected.
            // If we do not get a hit, it means that we're outside the scroll view - so we disable all the children
            // box colliders, which addresses the issue above.
            var activeControllerLeft = OVRInputHelpers.GetConnectedControllers(OVRInputHelpers.HandFilter.Left);
            Ray pointerLeft;
            bool gotRayLeft = OVRInputHelpers.GetSelectionRay(activeControllerLeft, this.trackingSpace, out pointerLeft);

            var activeControllerRight = OVRInputHelpers.GetConnectedControllers(OVRInputHelpers.HandFilter.Right);
            Ray pointerRight;
            bool gotRayRight = OVRInputHelpers.GetSelectionRay(activeControllerRight, this.trackingSpace, out pointerRight);

            RaycastHit hitLeft;
            RaycastHit hitRight;
            if (gotRayLeft && this.boxCollider.Raycast(pointerLeft, out hitLeft, 500) || gotRayRight && this.boxCollider.Raycast(pointerRight, out hitRight, 500))
            {
                // We got a hit in the scroll view. Check if we're already within the bounds - if so, do nothing.
                if (!isInBounds)
                {
                    // We entered the scroll view, so enable box colliders on children.
                    foreach (var boxCollider in this.content.gameObject.GetComponentsInChildren<BoxCollider>())
                    {
                        boxCollider.enabled = true;
                    }

                    isInBounds = true;
                }
            }
            else if (isInBounds)
            {
                // We are outside the scroll view and were previously inside, so disable box colliders on children.
                foreach (var boxCollider in this.content.gameObject.GetComponentsInChildren<BoxCollider>())
                {
                    boxCollider.enabled = false;
                }

                isInBounds = false;
            }

            // Get vector from either left or right thumbstick
            var moveVector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            if (moveVector.x == 0 && moveVector.y == 0)
            {
                moveVector = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            }
            if (moveVector.x == 0 && moveVector.y == 0)
            {
                moveVector = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            }
            if (moveVector.x == 0 && moveVector.y == 0)
            {
                moveVector = OVRInput.Get(OVRInput.Axis2D.SecondaryTouchpad);
            }

            if (moveVector.y == 0)
            {
                // No y-movement, so return. Also reset the speed multiplier
                this.lastSpeedMultiplier = 0;
                return;
            }

            // Scroll by a fixed amount proportional to thumbstick position on each frame
            // and map this to a fraction of the total viewport size:
            //   moveVector.y: The thumbstick vertical position normalized to [-1,1].
            //   Time.deltaTime: The time delta since last frame
            //   speedMultiplier: Just a multiplier to get a good scrolling speed. Increase over time to speed up scrolling.
            // So, moveVector.y * Time.deltaTime * speedMultiplier = the amount to scroll in "units"
            //   proportional to thumbstick position since last frame.
            // this.cellHeight / this.content.sizeDelta.y = cell height / total content height.
            float speedMultiplier = Mathf.Clamp(this.lastSpeedMultiplier * SpeedMultiplierIncrease, SpeedMultiplier, MaxSpeedMultiplier);
            float verticalIncrement = moveVector.y * Time.deltaTime * speedMultiplier * this.cellHeight / this.content.sizeDelta.y;
            this.lastSpeedMultiplier = speedMultiplier;
            this.verticalNormalizedPosition = Mathf.Clamp01(this.verticalNormalizedPosition + verticalIncrement);
        }

        void OnEnable()
        {
            // When this scroll view is enabled, make sure we resize the box collider appropriately.
            ResizeBoxCollider();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            // When the scroll view rect size changes, make sure we resize the box collider appropriately.
            ResizeBoxCollider();
        }

        private void ResizeBoxCollider()
        {
            // Resize the scroll view's box collider to match the scroll view rect size.
            var rect = transform.GetComponent<RectTransform>();
            var boxCollider = transform.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(rect.rect.width, rect.rect.height, 0);

            Debug.LogFormat("Resizing box collider: {0} x {1}", boxCollider.size.x, boxCollider.size.y);
        }

        public void OnPointerClick(PointerEventData e)
        {
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            // Only handle dragging if hands are active. Use thumbstick on controllers.
            if (!OVRPlugin.GetHandTrackingEnabled())
            {
                return;
            }

            if (null != parentRawRaycaster)
            {
                parentRawRaycaster.OnBeginDrag(eventData);
            }
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            // Only handle dragging if hands are active. Use thumbstick on controllers.
            if (!OVRPlugin.GetHandTrackingEnabled())
            {
                return;
            }

            if (null != parentRawRaycaster)
            {
                parentRawRaycaster.OnEndDrag(eventData);
            }
            base.OnEndDrag(eventData);
        }

        void IMoveHandler.OnMove(AxisEventData e)
        {
        }

        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
        }

        void OnMouseDrag()
        {
        }

        void OnMouseUp()
        {
        }

        void OnMouseDown()
        {
        }
    }
}