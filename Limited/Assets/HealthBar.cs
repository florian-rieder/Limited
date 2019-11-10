using UnityEngine;

public class HealthBar : MonoBehaviour
{
	public GameObject bar;
    private float currentValue;

	public void SetValue(float value)
	{
		/* Value must be beween 0 and 1 */
		if (value >= 0 && value <= 1 && value != currentValue)
		{
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
