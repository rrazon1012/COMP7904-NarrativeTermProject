using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intr_Book : InteractableObject
{
    //put the book manager here
    [SerializeField]
    protected puzzle_Shelf puzzle_shelf;
    [SerializeField]
    bool isRayHit = false;
    //put boolean for if its already pulled out
    [SerializeField]
    public bool isPulled = false;
    public bool isReward = false;
    public bool initialDisable = false;

    private void Start()
    {
        promptCanvas.enabled = false;
    }

    void FixedUpdate()
    {
        UpdateInteraction();
    }

    public override void OnInteraction(InteractionManager interactor)
    {

        if (active)
        {
            if (!puzzle_shelf.IsSolved)
            {
                base.OnInteraction(interactor);
                if (!isReward)
                {
                    if (!isPulled)
                    {
                        pullBook();
                        //function that moves the object forward
                    }
                    else
                    {
                        interactor.currentInteraction = this;
                    }
                }
            }
            else {
                if (isReward) {
                    interactor.currentInteraction = this;
                }
            }
        }

    }

    //show canvas based on if raycast is hitting
    public override void UpdateInteraction(){
        List<Transform> nearbyTargets = fov.FindNearbyTargets(INTERACTION_RANGE, fov.TargetMask);
        // fov.FindVisibleTargets();

        if ((nearbyTargets.Count > 0))
        {
            // active = true;

            int targets = 0;
            foreach (Transform target in nearbyTargets)
            {
                if (directional && fov.SourceWithinAngle(target.transform.position, this.transform.position, target.transform.forward, INTERACTION_ANGLE))
                {
                    targets++;
                }
                else if (!directional)
                {
                    targets++;
                }
            }

            if (targets > 0 && isRayHit)
            {
                if (!isReward && !puzzle_shelf.IsSolved || isReward && puzzle_shelf.IsSolved) {
                    active = true;
                    promptCanvas.gameObject.SetActive(true);
                }
            }
            else
            {
                active = false;
                promptCanvas.gameObject.SetActive(false);
            }

        }
        else if (!(nearbyTargets.Count > 0))
        {

            promptCanvas.gameObject.SetActive(false);
            active = false;
        }
    }

    public void pullBook() {
        if (!puzzle_shelf.IsSolved)
        {
            puzzle_shelf.pullOrder.Add(this);
        }
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * 15.0f, Time.deltaTime * 2.0f);
        isPulled = true;
    }
    public void pushBook() {
        transform.position = Vector3.Lerp(transform.position, transform.position + transform.forward * -15.0f, Time.deltaTime * 2.0f);
        isPulled = false;
    }

    public void enableCanvas() {
        if (!isReward || isReward && puzzle_shelf.IsSolved)
        {
            isRayHit = true;
            promptCanvas.enabled = true;
        }
    }
    public void disableCanvas() {
        isRayHit = false;
        promptCanvas.enabled = false;
    }
}
