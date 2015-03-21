using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gui : MonoBehaviour {

	public Text levelText;
	public Text timeText;
	
	public Text scrapText;
	public Text scrapText1;
	public Image scrapProgress;
	
	public Text metalText;
	
	public Text messageText;
	public Text selDamage;
	public Text buttonText;
	
	public Image timeProgress;
	
	
	public Text BaseName;
	public GameObject Base;
	public GameObject Fabric;
	
	public Animator endGame;
	
	public GameObject cursor;
	
	public static Gui instance;
	public static bool editorMode = false;
	
	public static TilePoint selectedTile = null;
	
	public static int gameMode = 0; //0 - in planet, 1 - in fly, 2 - fly pause
	
	int currentSec = 0;
	// Use this for initialization
	void Start () {
		instance = this;
		new ShipData();
		
		UpdateScraps();
		UpdateMetals();
		
		cursor.SetActive(false);
	}
	
	
	// Update is called once per frame
	void Update () {
		if ((gameMode == 1) && ShipData.levelData.currentTime > 0) {
			ShipData.levelData.currentTime -= Time.deltaTime;
			int sec = (int)(ShipData.levelData.time - ShipData.levelData.currentTime);
			timeProgress.fillAmount = ShipData.levelData.currentTime / ShipData.levelData.time;
			if (currentSec != sec) {
				currentSec = sec;
				timeText.text = sec.ToString("000");
				ShipData.update(currentSec);
			}
			
			if (ShipData.levelData.currentTime <= 0) {
				//End level
				gameMode = 0;
				Base.SetActive(true);
				timeProgress.fillAmount = 1f;
				buttonText.text = "LAUNCH";
				timeText.text = "000";
				ShipData.nextLevel();
			}
		
		}
		
		if (editorMode) {
			Vector2 mouseHex = ShipData.GetMouseHex();
			cursor.transform.position = new Vector3(mouseHex.x, mouseHex.y);
			
			if (Input.GetMouseButtonDown(0)) {
				if (ShipData.metals < ShipData.devices[0].price) {
					AddMessage("Pietrukst metala");
				} else {
					if (ShipData.mainShip.createTile(ShipData.GetMouse(),0,true)) {
						ShipData.addMetals(-ShipData.devices[0].price);
					}
				}
			}
			
		}
		
		if (Input.GetMouseButtonDown(1) && selectedTile != null) {
			selectedTile.ship.SetSelected(selectedTile.index, false);
			selectedTile = null;
		}
		
		if (selectedTile != null) {
			HexaTile tile = selectedTile.ship.GetTile(selectedTile.index);
			if (tile == null) {
				selectedTile = null;
				selDamage.text = "";
			} else {
				selDamage.text = tile.hp.ToString();
			}
			
		} else {
			selDamage.text = "";
		}
		
	}
	void OnEnable() {
		HexaTile.OnMouseClick += OnTileClick;
	}
	void OnDisable() {
		HexaTile.OnMouseClick -= OnTileClick;
	}
	public static void AddMessage(string message) {
		Debug.Log(message);
		if (instance == null) return;
		
		
		instance.messageText.text = instance.messageText.text.Insert(0,message + "\n");
	}
	public static void updateLevel() {
		AddMessage("Level " + ShipData.currentLevel + " loaded!");
		if (!instance) return;
		instance.BaseName.text = ShipData.levelData.name;
	}
		
	public static void UpdateScraps() {
		if (!instance) return;
		
		instance.scrapText.text = ShipData.scraps.ToString();
		instance.scrapText1.text = ShipData.scraps.ToString();
		
		instance.scrapProgress.fillAmount = (float)ShipData.scraps / (float)ShipData.scrapInventory;
	}
	public static void UpdateMetals() {
		if (!instance) return;
		instance.metalText.text = ShipData.metals.ToString();
	}
	
	void OnTileClick(TilePoint key) {
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (editorMode) return;
		
		//Debug.Log(key);
		if (selectedTile != null) selectedTile.ship.SetSelected(selectedTile.index,false);
		selectedTile = key;
		selectedTile.ship.SetSelected(selectedTile.index,true);
	}
	
	public void deleteTile() {
		if (selectedTile == null) return;

		HexaTile tile = selectedTile.ship.GetTile(selectedTile.index);
		if (tile.device.id == 1) {
			AddMessage("Cannot delete pilot cabine");
			return;
		}
		
		ShipData.addMetals(ShipData.devices[tile.device.id].price);
		AddMessage("Return metal:" + ShipData.devices[tile.device.id].price.ToString());
						
		selectedTile.ship.DeleteTile(selectedTile.index);
		selectedTile = null;
		
	}
	
	public void createDevice(int device) {
		if (selectedTile == null) return;
		if (ShipData.metals < ShipData.devices[device].price) {
			AddMessage("Pietrukst metala");
			return;
		}
		if (selectedTile.ship.CreateDevice(selectedTile.index, device)) {
			ShipData.addMetals(-ShipData.devices[device].price);
		}
	}
		
	/*GUI buttons*/
	public void SetGameMode() {
		switch (gameMode) {
			case 0: 
			case 2: 
				EditorMode(false);
				Base.SetActive(false);
				Fabric.SetActive(false);
				buttonText.text = "Pause";
				gameMode = 1;
				break;
			case 1: 
				buttonText.text = "Resume";
				gameMode = 2;
				break;
		}
	}
	public void FabricMode() {
		EditorMode(false);
		if (Fabric.activeSelf) {
			Fabric.SetActive(false);
		} else {
			Fabric.SetActive(true);
		}
	}
	
	public void FabricSellAll() {
		if (ShipData.scraps < 1) return;
		
		int scrap = (int)ShipData.scraps / ShipData.metalPrice;
		
		ShipData.addScraps(-scrap * ShipData.metalPrice);
		ShipData.addMetals(scrap);
		
	}

	public void EditorMode(bool value) {
		editorMode = value;
		cursor.SetActive(editorMode);
	}	
	
	public void swicthEditor() {
		EditorMode(!editorMode);
	}
	
	public void loadLevel(int tag) {
		if (tag == 0) 
			ShipData.prevLevel();
			else ShipData.nextLevel();
	}
	
}
