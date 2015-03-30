using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Gui : MonoBehaviour {

	public Text metalText;
	public Text scrapText;
	public Text scrapText1;
	public Text knownText;
	
	public Text baseName;
	public Text baseLevel;
	public Text timerText;
	public Slider travelSlider;
	public Text travelText;
	
	public Text tilePriceText;
	public Text tileRepairText;
	
	public Text tileDamage;
	public Text tileEnergy;

	
	public Text[] buyNames;
	public Text[] buyPrices;
		
	public Toggle edditorToggle;
	
	public Sprite[] deviceSprites;
	public Image deviceImage;
	public Text deviceName;
		
	public GameObject buildPanel;
	public GameObject selectedPanel;
	public GameObject planetPanel;
	public GameObject travelPanel;
	public Image travelPlanet;
	
	public Sprite[] planetIcons;
	
	public GameObject blankPanel;
	public GameObject devicePanel;
		
	public GameObject cursor;
	
	public GuiUpgrade[] upgrades;
	
	public static Gui instance;
	public static bool editorMode = false;
	
	private static string _selected = null;
	public static string selected {
		get { return _selected; }
		set {
			string old = _selected;
			_selected = value;
			if (!instance) return;
			
			instance.SetSelected(old);
		}
	}
	private static int _gameMode = 0;//0 - in planet, 1 - in fly, 2 - fly pause
	public static int gameMode {
		get { return _gameMode; }
		set {
			_gameMode = value;
			if (!instance) return;
			
			instance.SetGameMode();
		}
	}
	
	//public Button[] upgrades;
	
	int currentSec = 0;
	Color[] energyColors = new Color[3];
	
	// Use this for initialization
	void Start () {
		instance = this;
		new ShipData();
		
		UpdateScraps();
		UpdateMetals();
		
		//cursor.SetActive(false);
		
		tilePriceText.text = ShipData.devices[0].price.ToString();
		
		buyNames[0].text = ShipData.devices[2].name;
		buyPrices[0].text = ShipData.devices[2].price.ToString();

		buyNames[1].text = ShipData.devices[6].name;
		buyPrices[1].text = ShipData.devices[6].price.ToString();
		
		buyNames[2].text = ShipData.devices[3].name;
		buyPrices[2].text = ShipData.devices[3].price.ToString();
		
		buyNames[3].text = ShipData.devices[4].name;
		buyPrices[3].text = ShipData.devices[4].price.ToString();
		
		buyNames[4].text = ShipData.devices[11].name;
		buyPrices[4].text = ShipData.devices[11].price.ToString();
		
		buyNames[5].text = ShipData.devices[8].name;
		buyPrices[5].text = ShipData.devices[8].price.ToString();
		
		buyNames[6].text = ShipData.devices[7].name;
		buyPrices[6].text = ShipData.devices[7].price.ToString();
		
		energyColors[0] = new Color(0, 174f / 255f, 239f / 255f);
		energyColors[1] = new Color(237f / 255f, 37f / 255f, 37f / 255f);
		energyColors[2] = new Color(0f / 255f, 174f / 255f, 24f / 255f);
		
		cursor.SetActive(false);
		gameMode = 2;
	}
	
	
	// Update is called once per frame
	void Update () {
		if ((gameMode == 1) && ShipData.levelData.currentTime > 0) {
			ShipData.levelData.currentTime -= Time.deltaTime;
			int sec = (int)(ShipData.levelData.time - ShipData.levelData.currentTime);
			
			travelSlider.value = 1 - ShipData.levelData.currentTime / ShipData.levelData.time;
			
			if (currentSec != sec) {
				currentSec = sec;
				if (timerText != null) timerText.text = sec.ToString("000");
				travelText.text = sec.ToString("000");
				ShipData.update(currentSec);
			}
			
			if (ShipData.levelData.currentTime <= 0) {
				//End level
				gameMode = 2;
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
			if (Input.GetMouseButtonDown(1)) {
				edditorToggle.isOn = false;
			}
			return;
		} 
		
		if (Input.GetMouseButtonDown(1) && selected != null) {
			if (EventSystem.current.IsPointerOverGameObject()) return;
			selected = null;
		}
		
		if (selected != null) {
			HexaTile tile = ShipData.mainShip.GetTile(selected);
			if (tile == null) {
				selected = null;
				return;
			}
			tileDamage.text = tile.device.hpCurrent.ToString("0") + "/" + tile.device.hpMax.ToString("0");
			tileRepairText.text = tile.RepairPrice().ToString();
			
			if (tile.device.energyProduce > 0) {
				tileEnergy.text = tile.device.energyProduce.ToString();
				tileEnergy.color = energyColors[2];
			} else {
				tileEnergy.text = tile.device.energyNeed.ToString("0") + "/" + tile.device.energyCurrent.ToString("0");
				if (tile.device.energyCurrent >= tile.device.energyNeed) {
					tileEnergy.color = energyColors[0];
				} else {
					tileEnergy.color = energyColors[1];
				}
			}
		}
}
	
	public void StartFly() {
		gameMode = 1;
	}
	
	public void SetGameMode() {
		switch (gameMode) {
		case 0: 
			edditorToggle.isOn = false;
			planetPanel.SetActive(true);
			travelPanel.SetActive(false);
			selected = null;
			break;
		case 1: 
			edditorToggle.isOn = false;
			buildPanel.SetActive(false);
			planetPanel.SetActive(false);
			
			travelSlider.value = 0;
			travelText.text = "";
			travelPlanet.sprite = planetIcons[ShipData.currentLevel];
			travelPanel.SetActive(true);
			selected = null;
			break;
		case 2:
			edditorToggle.isOn = false;
			buildPanel.SetActive(false);
			planetPanel.SetActive(false);
			selectedPanel.SetActive(false);
			selected = null;
			travelPanel.SetActive(false);
			Base.StartBase();
			break;
		}
	}
	
	public void SetEditorMode(bool value) {
		editorMode = value;
		cursor.SetActive(editorMode);
		selected = null;
	}
	
	
	public void SetSelected(string oldValue) {
		if (oldValue != null) ShipData.mainShip.SetSelected(oldValue, false);
		
		if (selected != null) {
			buildPanel.SetActive(false);
			ShipData.mainShip.SetSelected(selected, true);
			selectedPanel.SetActive(true);
			
			HexaTile tile = ShipData.mainShip.GetTile(selected);
				blankPanel.SetActive(tile.device.id == 0);
				devicePanel.SetActive(tile.device.id != 0);
				deviceImage.sprite = deviceSprites[tile.device.id];
				deviceName.text = ShipData.devices[tile.device.id].name;
				
				foreach (GuiUpgrade obj in upgrades) {
					if (tile.device.upgrades.ContainsKey(obj.gameObject.name)) {
						obj.value.text = tile.valueByName(obj.gameObject.name);
						
						if (tile.device.upgrades[obj.gameObject.name].level <= 3) {
							obj.price.text = tile.device.upgrades[obj.gameObject.name].price.ToString();
							obj.label.text = tile.device.upgrades[obj.gameObject.name].label;
							obj.button.gameObject.SetActive(true);
						} else {
							obj.button.gameObject.SetActive(false);
						}
						
						
						obj.gameObject.SetActive(true);
					} else {
						obj.gameObject.SetActive(false);
					}
				}
				
		} else {
			selectedPanel.SetActive(false);
			if (_gameMode == 0) {
				buildPanel.SetActive(true);
			}
		}

	}
	
	void OnEnable() {
		HexaTile.OnMouseClick += OnTileClick;
	}
	void OnDisable() {
		HexaTile.OnMouseClick -= OnTileClick;
	}
	void OnTileClick(TilePoint key) {
		if (EventSystem.current.IsPointerOverGameObject()) return;
		if (editorMode) return;
		if (gameMode == 2) return;
		
		selected = key.index;		
	}
	
	public static void AddMessage(string message) {
		//Debug.Log(message);
		if (instance == null) return;
		
		
		//instance.messageText.text = instance.messageText.text.Insert(0,message + "\n");
	}
	public static void updateLevel() {
		AddMessage("Level " + ShipData.currentLevel + " loaded!");
		if (!instance) return;
		instance.baseName.text = ShipData.levelData.name;
		instance.baseLevel.text = ShipData.currentLevel.ToString();
	}
		
	
	public static void UpdateMetals() {
		if (!instance) return;
		if (instance.metalText == null) return;
		
		instance.metalText.text = ShipData.metals.ToString();
	}
	public static void UpdateScraps() {
		if (!instance) return;
		if (instance.scrapText == null) return;
		
		instance.scrapText.text = ShipData.scraps.ToString();
		instance.scrapText1.text = ShipData.scraps.ToString();
	}
	public static void UpdateKnowledge() {
		if (!instance) return;
		if (instance.knownText == null) return;
		
		instance.knownText.text = ShipData.knowledge.ToString();
	}
	
	
	
	public void createDevice(int device) {
		if (selected == null) return;
		
		if (ShipData.metals < ShipData.devices[device].price) {
			AddMessage("Pietrukst metala");
			return;
		}
		
		if (ShipData.mainShip.CreateDevice(selected, device)) {
			ShipData.addMetals(-ShipData.devices[device].price);
		}

				
		selected = selected;
		
	}
		
		
	/*GUI buttons*/
	public void FabricSellAll() {
		if (ShipData.scraps < 1) return;
		
		int scrap = (int)ShipData.scraps / ShipData.metalPrice;
		
		ShipData.addScraps(-scrap * ShipData.metalPrice);
		ShipData.addMetals(scrap);
	}

	
	
	public void loadLevel(int tag) {
		if (tag == 0) 
			ShipData.prevLevel();
			else ShipData.nextLevel();
	}
	
	public void deleteTile() {
		if (selected == null) return;
		
		HexaTile tile = ShipData.mainShip.GetTile(selected);
		if (tile.device.id == 1) {
			AddMessage("Cannot delete pilot cabine");
			return;
		}
		
		int price = (int)((float)ShipData.devices[tile.device.id].price * 0.75f);
		ShipData.addMetals(price);
		AddMessage("Return metal:" + price.ToString());
		
		ShipData.mainShip.DeleteTile(selected);
		selected = null;
		
	}
	public void deleteDevice() {
		if (selected == null) return;
		
		HexaTile tile = ShipData.mainShip.GetTile(selected);
		if (tile.device.id == 1) {
			AddMessage("Cannot delete pilot cabine");
			return;
		}
		
		int price = (int)((float)ShipData.devices[tile.device.id].price * 0.75f);
		
		if (ShipData.mainShip.DeleteDevice(selected)) {
			ShipData.addMetals(price);
		}
		
		selected = tile.key.index;
		
	}
	public void upgradeDevice(string param) {
		if (selected == null) return;
		
		HexaTile tile = ShipData.mainShip.GetTile(selected);
		tile.ugradeDevice(param);
		
		ShipData.mainShip.RecalcEnergy();
		
		selected = selected;
	}
	
	public void repairTile() {
		if (selected == null) return;
		HexaTile tile = ShipData.mainShip.GetTile(selected);
		if (tile == null || tile.device.hpCurrent >= tile.device.hpMax) {
			return;
		}
		
		int price = tile.RepairPrice();
		
		if (ShipData.metals < price) {
			AddMessage("Pietrukst metala");
			return;
		}
		
		ShipData.addMetals(-price);
		tile.Repair();
		
		selected = selected;
		
	}
	
	public void DestroyShip(int condition) {
		StartCoroutine(WaitAndClose(3.0F, condition));
	}

	IEnumerator WaitAndClose(float waitTime, int condition) {
		yield return new WaitForSeconds(waitTime);
		Debug.Log("WaitAndPrint " + Time.time);
		if (condition == 1) {
			Application.LoadLevel("lostScene");
		} else {
			Application.LoadLevel("victoryScene");
		}
	}
	
}
