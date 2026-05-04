using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DialogView : MonoBehaviour
{
	[SerializeField] private Text description = null;

	public Text Description
	{
		get { return description; }
	}
	
	[SerializeField] private Button okButton = null;

	public Button OKButton
	{
		get { return okButton; }
	}

	[SerializeField] private Image _background = null;	
	public Image Background
	{
		get { return _background; }
	}
	
	
	[SerializeField] private Button yesButton = null;

	public Button YesButton
	{
		get { return yesButton; }
	}

	[SerializeField] private Button noButton = null;
	public Button NoButton
	{
		get { return noButton; }
	}

}
