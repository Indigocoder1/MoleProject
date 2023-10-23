using UnityEngine;
using TMPro;

public class PickupPopup : MonoBehaviour
{
    [SerializeField] private float timeBeforeDisappearing;
    [SerializeField] private AnimationCurve alphaOverTime;
    [SerializeField] private float floatUpwardsSpeed;
    [SerializeField] private TMP_Text textChild;
    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        textChild.alpha = alphaOverTime.Evaluate(timeElapsed / timeBeforeDisappearing);
        transform.position += new Vector3(0, floatUpwardsSpeed, 0);
        if(timeElapsed >= timeBeforeDisappearing)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        textChild.text = text;
    }
}
