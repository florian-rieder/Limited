using UnityEngine;

public class HealthBar : MonoBehaviour
{
	public GameObject bar;
    private float currentValue = 1f;

	public void SetValue(float value)
	{
		/* Value must be beween 0 and 1 */
		if (value >= 0 && value <= 1 && value != currentValue)
		{
            if (value == 1){
                //dont show the bar when it is full
                gameObject.SetActive(false);
            } else if (currentValue == 1 && value != 1){
                gameObject.SetActive(true);
            }
			Vector3 scale = bar.transform.localScale;
			Vector3 newScale = new Vector3(value, scale.y, scale.z);

			bar.transform.localScale = newScale;
            currentValue = value;
		}
	}

    public void MoveTo(Vector3Int destination){
        Vector3 adjustedPosition = new Vector3(destination.x + 0.5f, destination.y + 0.1f, destination.z);
        transform.position = adjustedPosition;
    }

    public void Enable(bool value){
        gameObject.SetActive(value);
    }
}
