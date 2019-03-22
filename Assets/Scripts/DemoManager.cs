using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DemoManager : MonoBehaviour {
	public GameObject _mainView;

	public Transform _textContainer;
	public GameObject _infoWindow;

	public Animator _menuShowHide;

	public ToggleGroup _toggleGroup;
	public Toggle[] _toggles;
	public Toggle[] _pieceToggles;


	public TextManager _textManager;
	public InputField _input;
	public Text _forceText;
	public Slider _forceSlider;
	public Text _radiusText;
	public Slider _radiusSlider;
	public Slider _rotationSliderX;
	public Slider _rotationSliderY;
	public Slider _sizeSlider;
	public Slider _distanceSlider;

	public Toggle _autoIdealSize;

	public Toggle _gavityOn;
	public Toggle _explode;

	public GameObject[] _textPieces;

	private bool _isDragging;
	private Vector3 _clickOrigin;

	void Start () {
		_textManager.SetUp();
		_textManager.CreateText("Cube Text",ENTRY.TOP);
		//UpdateRotation();
		OpenInfo(false);
		//SetEntry(0);
	}
	void Update(){
		//Dragging and mouse up will fire regaurdless if moue is over UI
		//Mouse press will not start dragging if over UI

		if(Input.GetMouseButtonUp(0)){
			_isDragging = false;
			Debug.Log("draggin = " + _isDragging);

		}

		if(_isDragging){
			 //Camera.main.transform.Rotate(0, Input.GetAxis("Mouse X")*5.0f, 0), Space.World);

			 transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -4.0f, -Input.GetAxis("Mouse X") * -4.0f, 0));
             float X = transform.rotation.eulerAngles.x;
             float Y = transform.rotation.eulerAngles.y;
             Camera.main.transform.rotation = Quaternion.Euler(X, Y, 0);
    }

		if(EventSystem.current.IsPointerOverGameObject()){
			return;
		}
		if(Input.GetMouseButtonDown(0)){
			_isDragging = true;
			Debug.Log("draggin = " + _isDragging);
			Vector3 pos = Input.mousePosition;
      pos.z = 20;
			_clickOrigin = Camera.main.ScreenToWorldPoint(pos);


		}



			/*
			Vector3 pos = Input.mousePosition;
      pos.z = 20;
			pos = Camera.main.ScreenToWorldPoint(pos);
			Vector3 deltaMouse = pos - _clickOrigin;
			Debug.Log("deltaMos = " + deltaMouse);
			Camera.main.transform.LookAt(pos);
			return;
			Camera.main.transform.Rotate(new Vector3(0,deltaMouse.x,0));
			*/

	}

	public ENTRY FindEntry(){
		int i = 0;

		foreach(Toggle toggle in _toggles){
			if(toggle.isOn){

				return (ENTRY)i;
			}
			i++;
		}
		//default to zero

		return (ENTRY)0;
	}

	public void UpdateForce(){
		_forceText.text = "" + _forceSlider.value;
	}
	public void UpdateRadius(){
		_radiusText.text = "" + _radiusSlider.value;
	}
	public void UpdateRotation(){
		_mainView.transform.rotation = Quaternion.Euler(_rotationSliderX.value-180,_rotationSliderY.value, 0 );
	}

	public void CreateText(){
		int i = 0;
		foreach(Toggle toggle in _pieceToggles){
			if(toggle.isOn){
				_textManager.CreateText(_input.text,FindEntry(),_textPieces[i]);
				return;
			}
			i++;
		}
		//default to cubes if toggles not working for some reason
		Debug.LogWarning("no toggle is on.  Defaulting to cubes");
		_textManager.CreateText(_input.text,FindEntry(),_textPieces[0]);
	}
	public void EndText(){
		_textManager.Explode(_gavityOn.isOn,_explode.isOn,_forceSlider.value,_radiusSlider.value);
	}
	public void ClearAll(){
		_textManager.ClearAll();
	}
	public void UpdateSize(){
		_textManager.AdjustSize(_sizeSlider.value,_autoIdealSize.isOn);
	}
	public void FindIdealDistance(){
		_textManager.FindIdealDistance();
	}
	public void AdjustDistance(){
		_textManager.AdjustDistance(_distanceSlider.value);
	}
	public void CenterText(){
		_textManager.CenterText();
	}
	public void UpdateLine(){

	}
	public void OpenInfo(bool b){
		_infoWindow.SetActive(b);
	}
	public void ToggleMenu(){
		ShowHideMenu(!_menuShowHide.GetBool("show"));
	}
	public void ShowHideMenu(bool b){
		_menuShowHide.SetBool("show",b);
	}


}
