using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogPresenter : SingletonMonoBehaviour<DialogPresenter>
{
	public DialogView _dialogView;
	public UnityAction onDestroyed;
	public UnityAction onYes;
	public UnityAction onNo;

	private static readonly string PREFAB_NAME = "Prefabs/UI/Dialog/DialogCanvas";
	private static GameObject prefab;

	private DialogPresenter _dialogPresenter;
	
	public enum DialogType
	{
		OK,
		YesNo,
	}
	void Start()
	{
		this._dialogPresenter = null;
	}

	public DialogPresenter ShowDialog(
		string description, DialogType dialogType
	)
	{
		if (prefab == null)
		{
			prefab = Resources.Load(PREFAB_NAME) as GameObject;
		}

		var instance = Instantiate(prefab);
		var dialogPresenter = instance.GetComponent<DialogPresenter>();
		this._dialogPresenter = dialogPresenter;
		_dialogView = instance.GetComponent<DialogView>();
		// handler._title.text = title;
		dialogPresenter._dialogView.Description.text = description;

		switch (dialogType)
		{
			case DialogType.OK:
				_dialogView.OKButton.gameObject.SetActive(true);
				_dialogView.YesButton.gameObject.SetActive(false);
				_dialogView.NoButton.gameObject.SetActive(false);
//				if (string.IsNullOrEmpty(ok))
//				{
					// Destroy(dialogPresenter._dialogView.OKButton.gameObject);
//					dialogPresenter._dialogView.OKButton.GetComponentInChildren<Text>().text = ok;
					dialogPresenter._dialogView.OKButton.onClick.AddListener
					(
						() =>
						{
							Destroy(dialogPresenter.gameObject);
							_dialogPresenter = null;
						}
					);
//				}
//				else
				{
//					dialogPresenter._dialogView.OKButton.GetComponentInChildren<Text>().text = ok;

				// kari					dialogPresenter._dialogView.OKButton.onClick.AddListener(() => Destroy(dialogPresenter.gameObject));
				}

				break;
			case DialogType.YesNo:
				_dialogView.OKButton.gameObject.SetActive(false);
                _dialogView.YesButton.gameObject.SetActive(true);
				_dialogView.NoButton.gameObject.SetActive(true);
				
				dialogPresenter._dialogView.YesButton.onClick.AddListener
				(
					() =>
					{
						if (dialogPresenter.onYes != null)
						{
							dialogPresenter.onYes.Invoke();
						}

						Destroy(dialogPresenter.gameObject);
						_dialogPresenter = null;
					}
				);
				dialogPresenter._dialogView.NoButton.onClick.AddListener
				(
					() =>
					{
						if (dialogPresenter.onNo != null)
						{
							dialogPresenter.onNo.Invoke();;
						}

						Destroy(dialogPresenter.gameObject);
						_dialogPresenter = null;
					}
				);
				break;
		}
		dialogPresenter.gameObject.SetActive(true);

		return dialogPresenter;
	}


	private void OnDestroy()
	{
		// onDestroyed?.Invoke();
		if (onDestroyed != null)
		{
			onDestroyed.Invoke();
		}
	}

	public void Close()
	{
		if (_dialogPresenter)
		{
			_dialogPresenter.gameObject.SetActive(false);
		}
	}

   
}
