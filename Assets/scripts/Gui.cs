using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gui : MonoBehaviour {

	public Text levelText;
	public Text timeText;
	public Text scrapText;
	public Text metalText;
	public Text messageText;
	public Text selDamage;
	
	public Animator endGame;
	
	public GameObject cursor;
	
	public static Gui instance;
	public static bool editorMode = false;
	
	public static TilePoint selectedTile = null;
	
	public static bool paused = true;
	
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
		if (!paused && ShipData.levelData.currentTime > 0) {
			ShipData.levelData.currentTime -= Time.deltaTime;
			int sec = (int)(ShipData.levelData.time - ShipData.levelData.currentTime);
			
			if (currentSec != sec) {
				currentSec = sec;
				timeText.text = sec.ToString("000");
				ShipData.update(currentSec);
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
	
	public static void UpdateScraps() {
		if (!instance) return;
		instance.scrapText.text = ShipData.scraps.ToString();
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
	
	public void swicthEditor() {
		editorMode = !editorMode;
		
		cursor.SetActive(editorMode);
		Debug.Log(editorMode);
		
	}
	public void LoadLevel(string level) {
		ShipData.loadLevel(int.Parse(level));
		levelText.text = ShipData.currentLevel.ToString();
	}
	
	public void SetPause(bool value) {
		paused = !value;
	}
	public static void AddMessage(string message) {
		Debug.Log(message);
		if (instance == null) return;
		
			
		instance.messageText.text = instance.messageText.text.Insert(0,message + "\n");
	}
	
}
