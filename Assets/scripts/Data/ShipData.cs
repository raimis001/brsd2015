using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ShipData  {
	public static Dictionary<TilePoint, int> ship = new Dictionary<TilePoint, int>();
	public static Dictionary<int, DeviceData> devices = new Dictionary<int, DeviceData>();
	public static Dictionary<int, LevelData> levels = new Dictionary<int, LevelData>();
	
	public static List<Sprite> tiles = new List<Sprite>();
	
	public static int currentLevel;
	public static LevelData levelData;
	public static int levelCount;
	
	public static int metals = 0;
	public static int metalPrice = 10;
	
	public static int scraps = 0;
	public static int scrapInventory = 200;
	
	public static int energyMax = 100;
	public static int energyCurrent = 100;
	
	public static GameObject tileResource;
	public static HexaShip mainShip;
	
	static float _offsetX = Mathf.Sqrt(3) / 2;
	static float _offsetY = 1f - Mathf.Sqrt(3) + 0.225f;
	
	public ShipData() {
		JSONNode json;
			
		Sprite[] sprites = Resources.LoadAll<Sprite>("textures/devices");
		Dictionary<string, Sprite> spritesNames = new Dictionary<string, Sprite>();
		for(int i=0; i< sprites.Length; i++) {
			spritesNames.Add(sprites[i].name, sprites[i]);
		}		
		
		sprites = Resources.LoadAll<Sprite>("textures/hexaTiles");
		for(int i=0; i< sprites.Length; i++) {
			if (sprites[i].name.IndexOf("cursor") < 0) {
				tiles.Add(sprites[i]);
			}
		}		
		
		
		json = JSONNode.Parse(Resources.Load<TextAsset>("Data/Devices").text)["devices"];
		for (int i = 0; i < json.Count; i++) {
			int id = int.Parse(json.AsObject.keyAt(i));
			devices.Add(id, new DeviceData(id, json[i],id == 0 ? null : spritesNames[json[i]["atlas"]]));
		}
		
		json = JSONNode.Parse(Resources.Load<TextAsset>("Data/MainShip").text);
		for (int i = 0; i < json["ship"].Count; i++) {
			ship.Add(new TilePoint(json["ship"][i]["x"].AsInt,json["ship"][i]["y"].AsInt),json["ship"][i]["device"]["id"].AsInt);
		}
		
		scraps = json["properties"]["scraps"].AsInt;
		scrapInventory = json["properties"]["scrapInventory"].AsInt;
		metals = json["properties"]["metals"].AsInt;
		metalPrice = json["properties"]["metalPrice"].AsInt;
		Gui.UpdateScraps();
		Gui.UpdateMetals();
		
		json = JSONNode.Parse(Resources.Load<TextAsset>("Data/EnemyWaves").text)["levels"];
		for (int i = 0; i < json.Count; i++) {
			int id = int.Parse(json.AsObject.keyAt(i));
			levels.Add(id, new LevelData(json[i]));
		}
		levelCount = levels.Count;
		
		tileResource = Resources.Load ("Tile") as GameObject;
		
		mainShip = HexaShip.createShip(ship,Vector3.zero);
		
		loadLevel(1);
		
	}
	
	public static bool loadLevel(int level) {
		if (!levels.ContainsKey(level)) return false;
		
		currentLevel = level;
		levelData = levels[currentLevel];
		Gui.updateLevel();
		
		return true;
	}
	
	public static bool nextLevel() {
		return loadLevel(currentLevel + 1);
	}
	public static bool prevLevel() {
		return loadLevel(currentLevel - 1);
	}
	
	public static void update(int sec) {
		if (levelData.events.ContainsKey(sec)) {
			levelData.events[sec].Dispatch();
		}
	}
	
	public static void addScraps(int value) {
		scraps += value;
		Gui.UpdateScraps();
	}
	
	public static void addMetals(int value) {
		metals += value;
		Gui.UpdateMetals();
		if (value < 0) {
			Gui.AddMessage("Spent metal: " + Mathf.Abs(value));
		}
	}
	
	public static void addEnergy(int value) {
		energyCurrent += value;
	}
	
	public static Vector2 HexOffset( int x, int y ) {
		Vector2 position = Vector2.zero;
		
		position.y = y * _offsetY;
		position.x = ((y % 2) == 0 ? x : x + 0.5f) * _offsetX;
		
		return position;
	}	
	public static Vector2 HexOffset( Vector2 position ) {
		return HexOffset((int)position.x, (int)position.y);
	}
	
	public static Vector2 GetMouse() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f);
		
		int mouseY = Mathf.FloorToInt((mousePos.y) / _offsetY);
		int mouseX = Mathf.CeilToInt( (mousePos.x - 0.5f * ((Mathf.Abs(mouseY)) % 2)) / _offsetX);
		
		return new Vector2(mouseX, mouseY);
	}
	public static Vector2 GetMouseHex() {
		return HexOffset(ShipData.GetMouse());
	}
	
}
