using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class BeatingHealthBar : IconProgressBar
{
    public float minHeartSize, maxHeartSize;
    public float minBeatingTime, maxBeatingTime;

    public float yPivot = 0.5f;

    int currentIndex, previousIndex = -1;//to help determine which icon to animate

    public TransitionalObject beatingTransition;

    void Start()
    {
        base.Start();

        if(beatingTransition == null)
        {
            beatingTransition = GetComponent<TransitionalObject>();//find the transition!

            //if(beatingTransition == null)//no transition, so make one
            //{
            //    beatingTransition = gameObject.AddComponent<TransitionalObject>();
            //    beatingTransition.transitions = new TransitionalObjects.BaseTransition[1];
            //    beatingTransition.transitions[0] = gameObject.AddComponent<TransitionalObjects.ScalingTransition>();

            //    beatingTransition.FirstTransition.EditorInitialise(beatingTransition);
            //    beatingTransition.ScalingTransition.messagingEnabled = true;
            //    beatingTransition.FirstTransition.whenToSends = new TransitionalObjects.BaseTransition.TransitionState[] { TransitionalObjects.BaseTransition.TransitionState.Waiting, TransitionalObjects.BaseTransition.TransitionState.LoopFinished };

            //    UnityEvent biggestEvent = new UnityEvent();//sadly this doesn't work and not sure why!
            //    biggestEvent.AddListener(AtBiggestSize);

            //    UnityEvent smallestEvent = new UnityEvent();
            //    smallestEvent.AddListener(AtSmallestSize);

            //    beatingTransition.FirstTransition.events = new UnityEvent[] { biggestEvent, smallestEvent };

            //    beatingTransition.FirstTransition.stayForever = false;
            //    beatingTransition.FirstTransition.triggerInstantly = true;
            //    beatingTransition.FirstTransition.looping = true;
            //}
        }
        UpdateIndex(CheckIfIndexChanged());
    }

    protected override void UpdateAnimation()
    {
        for(int i = 0; i < maxIcons; i++)
        {
            ((RectTransform)backings[i].transform).anchoredPosition = new Vector2(backingImage.rect.width * (i + 0.5f) - ((RectTransform)transform).sizeDelta.x * 0.5f, 0);
            backings[i].transform.position = new Vector3(backings[i].transform.position.x, backings[i].transform.position.y, transform.position.z);
            mainImages[i].transform.position = backings[i].transform.position;

            ((RectTransform)mainImages[i].transform).anchoredPosition -= new Vector2(0, ((RectTransform)mainImages[i].transform).sizeDelta.y * (0.5f - yPivot));//now take into consdieration the changed pivot!
        }
    }

    /// <summary>
    /// Checks if health has been gained or lost and if a new icon needs to animate
    /// </summary>
    void UpdateIndex(int indexChange)
    {
        if(indexChange < 0 && currentIndex > 0)
            currentIndex--;
        else if(indexChange > 0 && currentIndex < mainImages.Count - 1)
            currentIndex++;

        previousIndex = currentIndex;//show there is no change and avoid stack overflows

        if(mainImages[currentIndex] != null && beatingTransition != null)
            beatingTransition.ScalingTransition.transform = mainImages[currentIndex].transform;//make the transition affect the correct images transform
    }

    /// <summary>
    /// Returns the difference between the current and previous index
    /// </summary>
    int CheckIfIndexChanged()
    {
        float currentPercentage = (currentValue / maxValue);
        int tempIndex = (int)(currentPercentage * MaxIcons);//determine the current index

        if(tempIndex >= MaxIcons)
            tempIndex = MaxIcons - 1;//avoid index out of range errors

        if(tempIndex < 0)
            tempIndex = 0;

        if(previousIndex == -1)//if nothing was set
        {
            previousIndex = tempIndex;//set the initial values
            currentIndex = previousIndex;
        }

        return tempIndex - previousIndex;
    }

    /// <summary>
    /// This is called when the heart has shrunk to the smallest size, it is used to determine how big to grow
    /// </summary>
    public void AtSmallestSize()
    {
        int indexChange = CheckIfIndexChanged();//has the icon we need to animate changed

        if(indexChange < 0)//if health needs to decrease
        {
            if(beatingTransition.ScalingTransition.transform.localScale == Vector3.zero)//if called again and shrunk to nothing
            {
                UpdateIndex(indexChange);//update the index to animate the next icon
                indexChange = CheckIfIndexChanged();//check for the new change

                if(indexChange == 0)//if we are on the right icon
                    UpdateStartPoint();//call again so the correct new min size is set
                else
                    beatingTransition.ScalingTransition.startPoint = Vector3.zero;//otherwise fully shrink the next heart as well

                beatingTransition.ScalingTransition.endPoint = Vector3.one;//make sure to start a full sized heart
                beatingTransition.TriggerFadeOut();
            }
            else//still mid beat! So shrink to nothing
            {
                beatingTransition.ScalingTransition.startPoint = Vector3.zero;//shrink to nothing
                beatingTransition.ScalingTransition.endPoint = beatingTransition.ScalingTransition.transform.localScale;//start at currentSize
                beatingTransition.TriggerFadeOut();
            }
        }
        #region Beat Normally
        else if(indexChange == 0)
            UpdateEndPoint();
        #endregion
    }

    /// <summary>
    /// This is called when the heart has grown to its largest size, it is used to determine how small to shrink
    /// </summary>
    public void AtBiggestSize()
    {
        int indexChange = CheckIfIndexChanged();//has the icon we need to animate changed

        if(indexChange > 0)//if health needs to increase
        {
            if(beatingTransition.ScalingTransition.transform.localScale == Vector3.one)//if called again and full size
            {
                UpdateIndex(indexChange);//update the index to animate the next icon
                indexChange = CheckIfIndexChanged();//check for the new change

                if(indexChange == 0)//if we are on the right icon
                    UpdateEndPoint();//call again so the correct new min size is set
                else
                    beatingTransition.ScalingTransition.endPoint = Vector3.one;

                beatingTransition.ScalingTransition.startPoint = Vector3.zero;//make sure to start an empty heart
                beatingTransition.TriggerTransition();
            }
            else//still mid beat! So expand fully
            {
                beatingTransition.ScalingTransition.endPoint = Vector3.one;//expand fully
                beatingTransition.ScalingTransition.startPoint = beatingTransition.ScalingTransition.transform.localScale;//start at currentSize
                beatingTransition.TriggerTransition();
            }
        }
        #region Beat Normally
        else if(indexChange == 0)
            UpdateStartPoint();
        #endregion
    }

    /// <summary>
    /// Called when expanding
    /// </summary>
    void UpdateStartPoint()
    {
        float minSize;

        if(currentValue == maxValue)
            minSize = 1;
        else if(currentValue == 0)
            minSize = 0;
        else
            minSize = currentValue % (maxValue / MaxIcons);//convert to find the percentage of the current heart 

        beatingTransition.FirstTransition.transitionInTime = Lerp(maxBeatingTime, minBeatingTime, minSize);

        if(currentValue > 0)
            minSize = Mathf.Max(minSize, minHeartSize);//don't go smaller than the min! 

        beatingTransition.ScalingTransition.startPoint = new Vector3(minSize, minSize, minSize);
    }

    /// <summary>
    /// Called when shrinking
    /// </summary>
    void UpdateEndPoint()
    {
        float minSize;

        if(currentValue == maxValue)
            minSize = 1;
        else
            minSize = currentValue % (maxValue / MaxIcons);//convert to find the percentage of the current heart 

        float maxSize = Lerp(maxHeartSize, 1, minSize);//use min heart size whilst its still a relative percentage to determine the max size

        if(currentValue == 0)
            maxSize = 0;

        beatingTransition.FirstTransition.transitionInTime = Lerp(maxBeatingTime, minBeatingTime, minSize);

        beatingTransition.ScalingTransition.endPoint = new Vector3(maxSize, maxSize, maxSize);
    }

    public void UpdatePivot()
    {
        for(int i = 0; i < maxIcons; i++)
            ((RectTransform)mainImages[i].transform).pivot = new Vector2(0.5f, yPivot);
    }

    protected override GameObject InstantiateMainImage(int index)
    {
        GameObject current = base.InstantiateMainImage(index);
        ((RectTransform)current.transform).pivot = new Vector2(0.5f, yPivot);

        if(index > currentIndex)//if after the current position
            current.transform.localScale = Vector3.zero;//hide until told otherwise!

        return current;
    }

    /// <summary>
    /// Returns the value that is the defined percentage between both values
    /// </summary>
    public static float Lerp(float min, float max, float percentageBetween)
    {
        float differenceSign = (min - max > 0 ? -1 : 1);//if the difference is negative then subtract the value at the end!

        return min + Mathf.Abs(min - max) * percentageBetween * differenceSign;
    }
}
