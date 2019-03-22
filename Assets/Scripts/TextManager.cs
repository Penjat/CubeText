using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour {
	private const string TAG = "TEXT MANAGER: ";

	public GameObject _cubePrefab;
	public Transform _cubeContainer;
	public Transform _toCleanUp;


	//private Letter[] characterRef;
	private Cube[,] _cubes;

	private Dictionary<char , Letter> characterRefs;

	int length;
	int height;

	public void SetUp(){

		characterRefs = new Dictionary<char, Letter>();

		//characterRefs = new Letter[26];
		characterRefs.Add(' ',new Letter(1,"0000000"));//space
		characterRefs.Add('A',new Letter(7,"0011100011011011000111100011111111111000111100011"));// = //A
		characterRefs.Add('B',new Letter(7,"0111111110001111000110111111110001111000110111111"));//B

		characterRefs.Add('C',new Letter(7,"0111100110011000000110000011000001111001100111100"));//C
		characterRefs.Add('D',new Letter(7,"0011111011001111000111100011110001101100110011111"));//D
		characterRefs.Add('E',new Letter(7,"1111111000001100000110111111000001100000111111111"));//E
		characterRefs.Add('F',new Letter(7,"1111111000001100000110111111000001100000110000011"));//F
		characterRefs.Add('G',new Letter(7,"1111100000011000000111110011110001111001101111100"));//G
		characterRefs.Add('H',new Letter(7,"1100011110001111000111111111110001111000111100011"));//H
		characterRefs.Add('I',new Letter(6,"111111001100001100001100001100001100111111"));//I
		characterRefs.Add('J',new Letter(7,"1100000110000011000001100000110001111000110111110"));//J
		characterRefs.Add('K',new Letter(7,"1100011011001100110110001111001101101100111100011"));//K
		characterRefs.Add('L',new Letter(7,"0000011000001100000110000011000001100000111111111"));//L
		characterRefs.Add('M',new Letter(7,"1100011111011111111111111111110101111000111100011"));//M
		characterRefs.Add('N',new Letter(7,"1100011110011111011111111111111101111100111100011"));//N
		characterRefs.Add('O',new Letter(7,"0111110110001111000111100011110001111000110111110"));//O
		characterRefs.Add('P',new Letter(7,"0111111110001111000111100011011111100000110000011"));//P
		characterRefs.Add('Q',new Letter(7,"0111110110001111000111100011111101111000111011110"));//Q
		characterRefs.Add('R',new Letter(7,"0111111110001111000111110011001111101110111110011"));//R
		characterRefs.Add('S',new Letter(7,"0011110011001100000110111110110000011000110111110"));//S
		characterRefs.Add('T',new Letter(6,"111111001100001100001100001100001100001100"));//T
		characterRefs.Add('U',new Letter(7,"1100011110001111000111100011110001111000110111110"));//U
		characterRefs.Add('V',new Letter(7,"1100011110001111000111110111011111000111000001000"));//V
		characterRefs.Add('W',new Letter(7,"1100011110001111000111101011111111111101111100011"));//W
		characterRefs.Add('X',new Letter(7,"1100011111011101111100011100011111011101111100011"));//X
		characterRefs.Add('Y',new Letter(6,"110011110011110011011110001100001100001100"));//Y
		characterRefs.Add('Z',new Letter(7,"1111111111000001110000011100000111000001111111111"));//Z

		characterRefs.Add('1',new Letter(6,"001100001110001100001100001100001100111111"));
		characterRefs.Add('2',new Letter(7,"0011110011001100110000011100000111000000111111111"));
		characterRefs.Add('3',new Letter(7,"1111110011000000110000111100110000011000110111110"));
		characterRefs.Add('4',new Letter(7,"0111000011110001101100110011111111101100000110000"));
		characterRefs.Add('5',new Letter(7,"0111111000001101111111100000110000011000110111110"));
		characterRefs.Add('6',new Letter(7,"0111100000011000000110111111110001111000110111110"));
		characterRefs.Add('7',new Letter(7,"1111111110001101100000011000000110000011000001100"));
		characterRefs.Add('8',new Letter(7,"0011110010001101001110011110111100111000010111110"));
		characterRefs.Add('9',new Letter(7,"0111110110001111000111111110110000001100000011110"));
		characterRefs.Add('0',new Letter(7,"0011100011001011000111100011110001101001100111100"));

		characterRefs.Add('.',new Letter(2,"00000000001111"));
		characterRefs.Add('!',new Letter(2,"11111111001111"));

		/*
		characterRef[0] = new Letter(4,"
		0000000
		0000000
		0000000
		0000000
		0000000
		0000000
		0000000
		");//B
		*/
		//CreateText("Cube Text");


	}
	public int FindLength(string s){
		int length = 0;

		for(int i=0;i<s.Length;i++){
			char c = s[i];
			if(characterRefs.ContainsKey(c)){
				length += characterRefs[c].GetWidth()+1;
			}else{
				length += 1;
			}

		}

		return length;
	}
	public void ClearText(){
		//TODO could have a timer to do the transition
		if(_cubes != null){
			foreach(Cube cube in _cubes){
				Destroy(cube.gameObject);
			}
		}
		_cubes = null;
	}

	public void CreateText(string s,ENTRY entry){
		CreateText(s,entry,_cubePrefab);
	}


	public void CreateText(string s,ENTRY entry,GameObject prefab){
		Debug.Log(TAG + "creating text");
		ClearText();
		//change string to uppercase
		s = s.ToUpper();

		length = FindLength(s);
		height = 7;

		Debug.Log("length = " + length);
		FindIdealDistance();

		_cubes = new Cube[length,height];

		for(int x=0;x<length;x++){
			for(int y=0;y<height;y++){
				GameObject g = Instantiate(prefab);
				g.transform.SetParent(_cubeContainer,false);
				g.transform.localPosition = new Vector3(x,y,0);
				Cube cube = g.GetComponent<Cube>();
				_cubes[x,y] = cube;
			}
		}
		int xOffset = 0;
		for(int i=0;i<s.Length;i++){

			xOffset += CreateLetter(xOffset,s[i],entry);
			xOffset += CreateSpace(xOffset);
		}




	}
	public void FindIdealDistance(){
		//finds a distance from the camera where the text will be visable

		int minLegth = 20;
		int zPos = length;
		if(zPos < minLegth){
			zPos = minLegth;
		}
		_cubeContainer.transform.localPosition = new Vector3((-length/2 )*_cubeContainer.localScale.x,0,zPos*(3.0f/4.0f)*_cubeContainer.localScale.x);

	}
	public int CreateSpace(int xOffset){
		for(int y=0;y<7;y++){

			Cube cube = _cubes[xOffset,y];

			cube.SetOn(false);

		}
		return 1;
	}

	public int CreateLetter(int xOffset,char c,ENTRY entry){
		//highlights the correct block and returns the letter's width to position the next letter
		if(!characterRefs.ContainsKey(c)){
			return 0;
		}
		Letter letter = characterRefs[c];
		long pos = 1;
		for(int y=0;y<7;y++){
			for(int x=0;x<letter.GetWidth();x++){

				Cube cube = _cubes[x+xOffset,y];
				bool isOn = ((pos & letter.GetData()) != 0);
				cube.SetOn(isOn);
				float r = Random.Range(0.0f,0.5f);
				//cube.SetAnimationSpeed(r);
				cube.SetEntryType(entry);
				cube.SetDelay(r);
				pos = pos << 1;
			}
		}
		return letter.GetWidth();
	}
	public void EndText(){
		//TODO add more options

	}
	public void Explode(bool gravity,bool explode,float force,float radius){
		if(_cubes == null){
			return;
		}
		float halfSize = _cubes[0,0].transform.localScale.x/2.0f;

		Vector3 center = _cubes[((_cubes.GetLength(0)-1)/2),2].transform.position + new Vector3(halfSize,halfSize,halfSize);
		foreach(Cube cube in _cubes){
			cube.SetRigidbody(false);

			float distance = Vector3.Distance(cube.transform.position, center);
			if(explode){
				cube.GetRigidbody().AddExplosionForce(distance*force,center , radius, 3.0F);
			}

			cube.GetRigidbody().useGravity = gravity;
			cube.transform.SetParent(_toCleanUp);
		}

		_cubes = null;
	}
	public void ClearAll(){
		//clears all the text pieces still floating around
		foreach (Transform child in _toCleanUp){
			Destroy(child.gameObject);
		}
	}
	public void AdjustSize(float size,bool autoDistance){

		_cubeContainer.transform.localScale = new Vector3(size,size,size);
		if(autoDistance){
			FindIdealDistance();
		}
		CenterText();
	}
	public void AdjustDistance(float distance){
		_cubeContainer.transform.localPosition = new Vector3(_cubeContainer.localPosition.x,0,distance);
	}
	public void CenterText(){
		_cubeContainer.transform.localPosition = new Vector3((-length/2.0f )*_cubeContainer.localScale.x,0,_cubeContainer.localPosition.z);
	}


}
