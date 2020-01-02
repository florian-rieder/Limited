using UnityEngine;
using TMPro; // Text Mesh Pro namespace
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildButton : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_text; // the text component of our button
	[SerializeField]
	private ButtonListControl btnControl;
	public Image m_icon;
	public BuildTooltip tooltip;
	public Color disabledColor;
	private string m_textString;
	private FacilitiesTileType m_type;

	private bool isEnabled = false;
	public string warningMessage;

	public void SetType(FacilitiesTileType type)
	{
		m_type = type;
	}
	public void SetImage(Sprite sprite)
	{
		m_icon.sprite = sprite;
	}

	public FacilitiesTileType GetTileType()
	{
		return m_type;
	}

	public void OnClick()
	{
		if (isEnabled)
		{
			btnControl.ButtonClicked(m_type);
		}
	}

	public void Enable(IsBuildableReport value)
	{
		if (value.isBuildable)
		{
			gameObject.GetComponent<Image>().color = Color.white;
		}
		else
		{
			gameObject.GetComponent<Image>().color = disabledColor;
		}

		isEnabled = value.isBuildable;
		warningMessage = value.warningMessage;
	}

	public void OnPointerEnter()
	{
		tooltip.MoveTo(gameObject.transform.position);
		tooltip.Enable(true);
		tooltip.SetTitle(m_type.Name);
		tooltip.SetWarning(warningMessage);
		tooltip.SetDescription(m_type.Description);
		tooltip.SetResourceDisplay(m_type.GenerateResourcesDictionary());
	}

	public void OnPointerExit()
	{
		tooltip.Enable(false);
	}
}
