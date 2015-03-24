using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gui : MonoBehaviour {

	public Text BaseName;
	public Text levelText;
	
	public Text timeText;
	public Image timeProgress;
	
	public Text scrapText;
	public Text scrapText1;
	public Image scrapProgress;
	
	public Text metalText;
	public Text knownText;
	
	public Text buttonText;
	
	public Text messageText;
	
	public GameObject Base;
	public GameObject Fabric;
	
	public GameObject PanelTile;
	public GameObject PanelDevice;
	
	public Animator endGame;
	
	
	public GameObject cursor;
	
	public static Gui instance;
	public static bool editorMode = false;
	
	private static TilePoint _selectedTile = null;
	public static TilePoint selectedTile {
		get { return _selectedTile; }
		set {
			TilePoint old = _selectedTile;
			_selectedTile = value;
			if (instance) {
				instance.setSelected(old);
			}
		}
	}
	
	public Text selectedCoord;
	public Text selectedDamageText;
	public Image selectedDamageProgress;
	
	public Text selectedDevice;
	
	public Text selectedEnergyText;
	public Text selectedEnergyNeed;
	public Image selectedEnergyProgress;
	
	public static int gameMode = 0; //0 - in planet, 1 - in fly, 2 - fly pause
	
	int currentSec = 0;
	// Use this for initialization
	void Start () {
		instance = this;
		new ShipData();
		
		UpdateScraps();
		UpdateMetals();
		
		cursor.SetActive(false);
		
		PanelTile.SetActive(false);
		PanelDevice.SetActive(false);
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
			selectedTile = null;
		}
		
		if (selectedTile != null) {
			HexaTile tile = ShipData.mainShip.GetTile(selectedTile.index);
			if (tile != null) {
				selectedDamageText.text = tile.hp.ToString("00");
				selectedDamageProgress.fillAmount = tile.hp / tile.hpMax;
				
				selectedEnergyText.text = Mathf.Abs(tile.device.energyCurrent).ToString();
				selectedEnergyNeed.text = Mathf.Abs(tile.device.energyNeed).ToString();
				
				if (tile.device.energyNeed < 1) {
					selectedEnergyProgress.fillAmount = 1f;
				} else {
					selectedEnergyProgress.fillAmount = (float)tile.device.energyCurrent / (float)tile.device.energyNeed;
				}
			} else {
				selectedTile = null;
			}
			
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
		instance.levelText.text = ShipData.currentLevel.ToString();
	}
		
	public static void UpdateKnowledge() {
		if (!instance) return;
		
		instance.knownText.text = ShipData.knowledge.ToString();
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
		
		selectedTile = key;
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
		
		//selectedTile = null;
		selectedTile = selectedTile;
		
	}
		
	public void setSelected(TilePoint oldValue) {
		if (oldValue != null) ShipData.mainShip.SetSelected(oldValue.index, false);
		
		
		PanelTile.SetActive(false);
		PanelDevice.SetActive(false);
		if (_selectedTile != null) {
			
			ShipData.mainShip.SetSelected(_selectedTile.index, true);
			HexaTile tile = ShipData.mainShip.GetTile(_selectedTile.index);
			
			if (tile.tileID == 0) {
				selectedCoord.text = "x:" + tile.key.x.ToString() + " y:" + tile.key.y.ToString();
				PanelTile.SetActive(true);
			} else {
				selectedDevice.text = tile.device.name;
				PanelDevice.SetActive(true);
			}
			
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
		selectedTile = null;
	}	
	
	public void swicthEditor() {
		EditorMode(!editorMode);
	}
	
	public void loadLevel(int tag) {
		if (tag == 0) 
			ShipData.prevLevel();
			else ShipData.nextLevel();
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
	public void deleteDevice() {
		if (selectedTile == null) return;
		
		HexaTile tile = selectedTile.ship.GetTile(selectedTile.index);
		if (tile.device.id == 1) {
			AddMessage("Cannot delete pilot cabine");
			return;
		}
		
		int price = ShipData.devices[tile.device.id].price;
		
		if (selectedTile.ship.DeleteDevice(selectedTile.index)) {
			ShipData.addMetals(price);
		}
		
		selectedTile = null;
		selectedTile = tile.key;
	}
	
}
