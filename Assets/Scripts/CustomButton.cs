using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	[SerializeField]
	private TMP_Text text;
	[SerializeField]
	private Color hoverColor;
	[SerializeField]
	private AudioClip hoverSound;
	[SerializeField]
	private AudioClip clickSound;

	private AudioSource audioSource;
	private Color defaultColor;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		defaultColor = text.color;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		var newScale = 1.1f;
		text.color = hoverColor;
		audioSource.PlayOneShot(hoverSound);
		transform.LeanScale(new Vector3(newScale, newScale, newScale), 0.1f);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		text.color = defaultColor;
		transform.LeanScale(Vector3.one, 0.2f);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		var newScale = 0.8f;
		transform.LeanScale(new Vector3(newScale, newScale, newScale), 0.1f).setLoopPingPong(1);
		audioSource.PlayOneShot(clickSound);
	}
}
