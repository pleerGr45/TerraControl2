using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace TC_basic
{
    
    public class GameFieldClass
    {
        private Cell[][] field;
        private Cell main_cell;
        private Dictionary<ProductType, int> storage;
        private Transform parent;
        public static GameObject[] cell_prefabs;
        public static GameObject[] building_prefabs;

        public GameFieldClass(Transform parent, GameObject[] cell_prefabs, GameObject[] building_prefabs, int lenght = 10, int width = 10)
        {
            this.parent = parent;
            GameFieldClass.cell_prefabs = cell_prefabs;
            GameFieldClass.building_prefabs = building_prefabs;
            this.storage = new Dictionary<ProductType, int>() {
                {ProductType.LIMESTONE_RAW, 0},
                {ProductType.QUARTZ_RAW, 0},
                {ProductType.OIL_RAW, 0},
                {ProductType.IRON_RAW, 0},
                {ProductType.COPPER_RAW, 0},
                {ProductType.LEAD_RAW, 0},
                {ProductType.IRIDIUM_RAW, 0},
                {ProductType.URANIUM_RAW, 0},
                {ProductType.CONCRETE, 0},
                {ProductType.CHEMICALS, 0},
                {ProductType.FUEL, 0},
                {ProductType.STEELWORK, 0},
                {ProductType.LEAD_PLATE, 0},
                {ProductType.IRIDIUM_PLATE, 0},
                {ProductType.WIRES, 0},
                {ProductType.URANIUM_ROD, 0},
                {ProductType.GLASS, 0},
                {ProductType.BLOCK, 0},
                {ProductType.ELECTRIC, 0},
                {ProductType.TRANSFORMER, 0},
                {ProductType.LASER, 0},
                {ProductType.CORPUS, 0},
                {ProductType.AMMO, 0},
                {ProductType.EXPLOISIVE_AMMO, 0},
                {ProductType.PIERCING_ROCKET, 0},
                {ProductType.GLASS_ROCKET, 0},
                {ProductType.TOXIC_ROCKET, 0},
                {ProductType.SUPER_ROCET, 0}
            };
            
            this.field = GenerateField(lenght, width);
        }

        public GameFieldClass(int lenght = 10, int width = 10)
        {
            this.field = GenerateField(lenght, width);
        }

        public bool UpdateRules() {
            for (int i = 0; i < field.Length; i++)
                for (int j = 0; j < field[i].Length; j++) {
                    Cell cell = field[i][j];
                    if (cell.CellZone.Health.Toughness <= 0 && cell.CellZone.ZoneType != CellZoneType.NOT_ZONED) 
                        cell.DestroyZone();
                }

            return main_cell != null ? main_cell.CellZone.ZoneType == CellZoneType.NOT_ZONED : true;
        }

        public void UpdateCells() {
            for (int i = 0; i < field.Length; i++)
                for (int j = 0; j < field[i].Length; j++)
                    field[i][j].Update();
        }

        public void AddResource(ProductType type, int amount) => storage[type] += amount;

        public bool RemoveResource(ProductType type, int amount) {
            if(storage[type] - amount < 0) return false;
            storage[type] -= amount;
            return true;
        }

        internal List<Cell> GetCellNeighbors(int i, int j) {
            int q = i % 2 == 0 ? -1 : 1; 

            List<Cell> neighbors = new List<Cell>();
            
            if (CellExistance(i, j - 1)) neighbors.Add(field[i][j - 1]);
            if (CellExistance(i, j + 1)) neighbors.Add(field[i][j + 1]);
            if (CellExistance(i - 1, j)) neighbors.Add(field[i - 1][j]);
            if (CellExistance(i + 1, j)) neighbors.Add(field[i + 1][j]);
            if (CellExistance(i - 1, j + q)) neighbors.Add(field[i - 1][j + q]);
            if (CellExistance(i + 1, j + q)) neighbors.Add(field[i + 1][j + q]);
            
            return neighbors;
        }

        private bool CellExistance(int i, int j) => !(i < 0 || j < 0 || i >= field.Length || j >= field[i].Length);

        public static Cell[][] GenerateField(int length, int width)
        {
            Cell[][] field = new Cell[length][];

            // Ratio generate
            byte v1 = (byte)UnityEngine.Random.Range(12, 15);
            byte v2 = (byte)UnityEngine.Random.Range(50, 75);
            byte v3 = (byte)UnityEngine.Random.Range(80, 85);
            
            // Forming
            int n = (int)(length*width*0.2f);
            List<int[]> forming = new List<int[]>();

            for (int i = 0; i < n; i++) {
                forming.Add(new int[2]);
                forming[i][0] = UnityEngine.Random.Range(0, length);
                forming[i][1] = UnityEngine.Random.Range(0, width);
            }

            // Relief map
            byte[][] relief_map = new byte[length][];
            bool[][] bit_mask = new bool[length][];

            for (int i = 0; i < length; i++) {
                // Inits
                field[i] = new Cell[width];
                relief_map[i] = new byte[width];
                bit_mask[i] = new bool[width];

                for (int j = 0; j < width; j++) {
                        // Field init
                    field[i][j] = new Cell();
                        // Relief map init
                    byte v = (byte)UnityEngine.Random.Range(0, 100);
                    byte r = 2;
                    if (v < 15) r = 1;
                    else if (v >= v1 && v < v2) r = 2;
                    else if (v >= v2 && v < v3) r = 3;
                    else if (v >= v3) r = 4;
                    relief_map[i][j] = r;
                        // Bit mask init
                    bit_mask[i][j] = false;
                    for(int k = 0; k < n; k++)
                        if (forming[k][0] == i && forming[k][1] == j) {
                            bit_mask[i][j] = true;
                            forming.RemoveAt(k);
                            n--;
                            break;
                        }
                }
            }

            // Smooth
            while (FindBit(bit_mask)) {
                for (int i = 0; i < length; i++)
                    for (int j = 0; j < width; j++)
                        if (!bit_mask[i][j]) {
                            int q = i % 2 == 0 ? -1 : 1; 

                            List<int[]> neighbors = new List<int[]>() { 
                                new int[2] { i, j - 1 }, 
                                new int[2] { i, j + 1 }, 
                                new int[2] { i - 1, j }, 
                                new int[2] { i + 1, j }, 
                                new int[2] { i - 1, j + q }, 
                                new int[2] { i + 1, j + q } 
                            };

                            for (int k = 0; k < neighbors.Count; k++)
                                if (!(CellExistance(neighbors[k][0], neighbors[k][1], length, width) && bit_mask[neighbors[k][0]][neighbors[k][1]])) {
                                    neighbors.RemoveAt(k);
                                    k--;
                                }

                            if (neighbors.Count > 0) {
                                int r = UnityEngine.Random.Range(0, neighbors.Count);
                                if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.6f)
                                    relief_map[i][j] = relief_map[neighbors[r][0]][neighbors[r][1]];
                                bit_mask[i][j] = true;
                                    // Relief to cell
                                field[i][j].landscape.LandscapeType = (CellLandscapeType)relief_map[i][j];
                                break;
                            }
                        }
            }

            int lows = 0, plains = 0, highs = 0, mountains = 0;
            
            // Nature map
            for (int i = 0; i < length; i++)
                for (int j = 0; j < width; j++) {
                    switch (relief_map[i][j]) {
                        case 1: lows++; break;
                        case 2: plains++; break;
                        case 3: highs++; break;
                        case 4: mountains++; break;
                    }
                }
            
            int forest_lows = (int)(lows * UnityEngine.Random.Range(0.05f, 0.2f)),
                lake_lows = (int)(lows * UnityEngine.Random.Range(0.07f, 0.2f));
            int forest_plains = (int)(plains * UnityEngine.Random.Range(0.2f, 0.6f)),
                swamp_plains = (int)(plains * UnityEngine.Random.Range(0.08f, 0.1f));
            highs = (int)(highs * UnityEngine.Random.Range(0.1f, 0.3f));
            mountains = (int)(mountains * UnityEngine.Random.Range(0.1f, 0.3f));

            while (forest_lows+lake_lows+forest_plains+swamp_plains+highs+mountains > 0) {
                int i = UnityEngine.Random.Range(0, length), 
                    j = UnityEngine.Random.Range(0, width);
                
                if (field[i][j].landscape.LandscapeNature == CellLandscapeNature.LAWN)
                switch (relief_map[i][j]) {
                    case 1:
                        if (forest_lows > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.FOREST;
                            forest_lows--;
                        } else if (lake_lows > 0&& UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.LAKE;
                            lake_lows--;
                        }
                        break;
                    case 2:
                        if (forest_plains > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.FOREST;
                            forest_plains--;
                        } else if (swamp_plains > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.SWAMP;
                            swamp_plains--;
                        }
                        break;
                    case 3:
                        if (highs > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.FOREST;
                            highs--;
                        }
                        break;
                    case 4:
                        if (mountains > 0 && UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            field[i][j].landscape.LandscapeNature = CellLandscapeNature.FOREST;
                            highs--;
                        }
                        break;
                    }
            }

            //Result
            return field;
        }

        public static bool FindBit(bool[][] mask) {
            for (int i = 0; i < mask.Length; i++) {
                for (int j = 0; j < mask[i].Length; j++) {
                    if (!mask[i][j]) return true;
                }
            }
            return false;
        }


        public static bool CellExistance(int i, int j, int length, int width) => !(i < 0 || j < 0 || i >= length || j >= width);

        public GameObject[][] InitCells() {
            GameObject[][] cells = new GameObject[field.Length][];

            for (int i = 0; i < field.Length; i++) {
                cells[i] = new GameObject[field[i].Length];
                for (int j = 0; j < field[i].Length; j++) {
                    GameObject cell = GameObject.Instantiate(cell_prefabs[field[i][j].GetPrefabId()], new Vector3((i - parent.localScale.x/2 + 1)*10, 0, ((j - parent.localScale.z/2 + 1)*12)+(i % 2 == 1 ? 6 : 0)), quaternion.identity);
                    cell.name = "Cell_" + i + "_" + j;
                    HexClickReciver reciver = cell.GetComponent<HexClickReciver>();
                    reciver.cell = field[i][j];
                    reciver.i_cord = i;
                    reciver.j_cord = j;
                    cells[i][j] = cell;
                }
            }

            return cells;
        }

        public void RefreshCell(GameObject old_cell, int i, int j) {
            GameObject.Destroy(old_cell);
            old_cell = GameObject.Instantiate(cell_prefabs[field[i][j].GetPrefabId()], new Vector3(i * 10, 0, (j * 12) + (i % 2 == 1 ? 6 : 0)), quaternion.identity);
            
        }

        public GameFieldClass Clone() {
            GameFieldClass clone = new GameFieldClass();

            for (int i = 0; i < field.Length; i++) {
                clone.field[i] = new Cell[field[i].Length];
                for (int j = 0; j < field[i].Length; j++)
                    clone.field[i][j] = field[i][j].Clone();
            }

            return clone;
        }
    }

    /**
        <summary>
        <b> CLASS CELL </b> <i> (important) </i>
        <br> This class describes what any cell of the game field should be, its properties and behavior. </br>
        </summary>
     */
    public class Cell
    {
        // Dynamic fields
        private GameFieldClass parent;
        private float ecology_level;
        private float supply, demand;

        // Changing fields
        private bool is_active;
        private Zone zone;
        internal Landscape landscape;
        private List<CellEffect> effects;

        // Constant fields
        private CellResourceType[] resources;

        public GameFieldClass Parent { get => parent; }
        public Zone CellZone { get => zone; set => zone = value; }
        public CellResourceType[] Resources { get => resources; }
        public float Supply { get => supply; }
        public float Demand { get => demand; }
        public Landscape Landscape { get => landscape; }

        public Cell(GameFieldClass parent) : this() {
            this.parent = parent;
        }

        public Cell() {
            this.effects = new List<CellEffect>();
            this.zone = new Zone(CellZoneType.NOT_ZONED, new Health(0));
            this.landscape = new Landscape(CellLandscapeType.PLAIN, CellLandscapeNature.LAWN);
            resources = new CellResourceType[8] {CellResourceType.LIMESTONE, CellResourceType.QUARTZ, CellResourceType.OIL, CellResourceType.IRON, CellResourceType.COPPER, CellResourceType.LEAD, CellResourceType.IRIDIUM, CellResourceType.URANIUM}; 
            ecology_level = 1;
            supply = 1;
            demand = 1;
            is_active = true;
        }

        internal void Update() {
            foreach (CellEffect effect in effects) {
                effect.RealeseEffect(this);
            }
        }
        
        internal bool Terraform(Landscape new_landscape) {
            this.landscape = new_landscape;
            return true;
        }

        internal bool ChangeZone(CellZoneType new_zone) {
            if(landscape.LandscapeZoneRules(new_zone))
                return false;
            return this.zone.ChangeType(new_zone);
        }
 
        internal bool DestroyZone() {
            return zone.Destroy();
        }

        public int GetPrefabId() {
            int prefab_id = -1;

            if(zone.ZoneType == CellZoneType.NOT_ZONED)
                switch(landscape.LandscapeNature) {
                    case CellLandscapeNature.LAWN:
                        prefab_id = (byte)landscape.LandscapeType;
                        break;
                    case CellLandscapeNature.FOREST:
                        prefab_id = (byte)landscape.LandscapeType + 4;
                        break;
                    case CellLandscapeNature.SWAMP:
                        prefab_id = 9;
                        break;
                    case CellLandscapeNature.LAKE:
                        prefab_id = 10;
                        break;
                }
            else switch(zone.ZoneType) {
                case CellZoneType.PRODUCTION_ZONE:
                    switch(landscape.LandscapeType) {
                        case CellLandscapeType.QUARRY:
                            prefab_id = 11;
                            break;
                        case CellLandscapeType.MOUNTAIN:
                            prefab_id = 18;
                            break;
                    }
                    break;
                case CellZoneType.SUPPLY_ZONE:
                    switch(landscape.LandscapeType) {
                        case CellLandscapeType.PLAIN:
                            prefab_id = 12;
                            break;
                        case CellLandscapeType.HIGHLAND:
                            prefab_id = 16;
                            break;
                    }
                    break;
                case CellZoneType.INDUSTRY_ZONE:
                    prefab_id = 13;
                    break;
                case CellZoneType.PROTECTION_ZONE:
                    switch(landscape.LandscapeType) {
                        case CellLandscapeType.PLAIN:
                            prefab_id = 15;
                            break;
                        case CellLandscapeType.HIGHLAND:
                            prefab_id = 17;
                            break;
                        case CellLandscapeType.MOUNTAIN:
                            prefab_id = 19;
                            break;
                    }
                    break;
                case CellZoneType.MAIN_ZONE:
                    prefab_id = 20;
                    break;
            }

            return prefab_id;
        }

        public Cell Clone() {
            Cell clone = new Cell();
            clone.ecology_level = ecology_level;
            clone.supply = supply;
            clone.demand = demand;
            clone.is_active = is_active;

            clone.effects = new List<CellEffect>();
            for (int i = 0; i < effects.Count; i++)
                clone.effects.Add(effects[i]);

            clone.zone = zone.Clone();
            clone.landscape = new Landscape(landscape.LandscapeType, landscape.LandscapeNature);

            clone.resources = new CellResourceType[resources.Length];
            for (int i = 0; i < resources.Length; i++)
                clone.resources[i] = resources[i];

            return clone;
        }
    }

    public class CellMetaData {
        private Dictionary<string, CellMetaDataItem> metadata;
        public static string[] allowed_keys = {""};

        public static CellMetaData operator + (CellMetaData self, CellMetaData other) { return null; }

        public CellMetaData() {
            metadata = new Dictionary<string, CellMetaDataItem>();
        }

        public void AddItem(string key, CellMetaDataItem item) {
            metadata.Add(key, item);
        }
    }
    
    public interface CellMetaDataItem {
        public string ToString();
    }

    /**
        <summary>
        <b> CLASS ZONE </b> <i> (important) </i>
        <br> One of the main characteristics of the cell. </br>
        </summary>
    */
    public class Zone {
        private CellZoneType zone_type;
        private Building[] buildings;
        private GameObject[] building_models;
        private Health toughness;

        public CellZoneType ZoneType { get => zone_type; }
        internal Health Health { get => toughness; }

        public bool Destroy() {
            zone_type = CellZoneType.NOT_ZONED;
            buildings = new Building[4];

            return true;
        }

        public Zone(CellZoneType zone_type, Health toughness) {
            this.zone_type = zone_type;
            this.toughness = toughness;
            buildings = new Building[4];
            building_models = new GameObject[4];
        }

        internal bool ChangeType(CellZoneType zone_type) {
            this.zone_type = zone_type;
            return false;
        }

        internal bool Build(int slot, BuildingType type, Vector3 position) {
            if(slot < 0 || slot > 3 || buildings[slot] != null) return false;

            buildings[slot] = new Building(type);
            
            if (building_models[slot] != null) 
                GameObject.Destroy(building_models[slot]);
            
            GameObject building = GameObject.Instantiate(GameFieldClass.building_prefabs[buildings[slot].GetPrefabId()], position, Quaternion.Euler(0, slot*90, 0));

            building_models[slot] = building;
            
            return true;
        }

        internal bool DestroyBuild(int slot) {
            if(slot < 0 || slot > 3 || buildings[slot] == null) return false;

            GameObject.Destroy(building_models[slot]);

            buildings[slot] = null;

            return true;
        }

        public Zone Clone() {
            Health clone_toughness = toughness.Clone();
            Zone clone = new Zone(zone_type, clone_toughness);

            return clone;
        }

        public string GetZoneName() {
            string name = "";

            switch (zone_type)
            {
                case CellZoneType.NOT_ZONED: name = "Нет Зоны"; break;
                case CellZoneType.PRODUCTION_ZONE: name = "Зона Добычи"; break;
                case CellZoneType.SUPPLY_ZONE: name = "Зона Снабжения"; break;
                case CellZoneType.INDUSTRY_ZONE: name = "Зона Производства"; break;
                case CellZoneType.PROTECTION_ZONE: name = "Зона Защиты"; break;
                case CellZoneType.MAIN_ZONE: name = "Главная Зона"; break;
            }

            return name;
        }

        public Color GetZoneColor() {
            Color color = Color.white;
            switch (zone_type)
            {
                case CellZoneType.NOT_ZONED: color = Color.gray; break;
                case CellZoneType.PRODUCTION_ZONE: color = Color.cyan; break;
                case CellZoneType.SUPPLY_ZONE: color = Color.green; break;
                case CellZoneType.INDUSTRY_ZONE: color = new Color(196, 86, 17); break;
                case CellZoneType.PROTECTION_ZONE: color = Color.red; break;
                case CellZoneType.MAIN_ZONE: color = new Color(191, 143, 0); break;
            }
            return color;
        }
    }


    public class Health {
        public const float MAX_HEALTH_LIMIT = 100000;
        private float toughness, max_toughness;
        private float autoregeneration;
        private Dictionary<DamageType, float> resistances;

        public float Toughness { get => toughness; set => toughness = value; }
        public float MaxToughness { get => max_toughness; set => max_toughness = value; }

        public Health(float max_toughness) {
            this.max_toughness = max_toughness;
            toughness = max_toughness;
            autoregeneration = 0;
            resistances = new Dictionary<DamageType, float>();
        }

        internal float AddMaxHealth(float toughness) {
            return max_toughness = max_toughness + toughness > 0 || max_toughness + toughness < MAX_HEALTH_LIMIT ? max_toughness : max_toughness + toughness;
        }

        internal float Damage(DamageType type, float damage) {
            return toughness -= damage * (1 - resistances[type]);
        }

        internal void Update() {

        }

        public Health Clone() {
            Health clone = new Health(max_toughness);

            clone.toughness = toughness;
            clone.autoregeneration = autoregeneration;
            clone.resistances = new Dictionary<DamageType, float>(resistances);

            return clone;
        }
    }

        public class Resource {
            private ProductType resource_type;
            public float amount;

            public Resource(ProductType resource_type, float amount) {
                this.resource_type = resource_type;
                this.amount = amount;
            }
        }

    public class Building {
        private BuildingType building_type;

        public Building(BuildingType type) {
            building_type = type;
        }

        public int GetPrefabId() => (int)building_type;
    }

    /**
        <summary>
        <b> CLASS LANDSCAPE </b> <i> (important) </i>
        <br> One of the main characteristics of the cell. </br>
        </summary>
    */
    public class Landscape {
        private CellLandscapeType landscape;
        private CellLandscapeNature nature;

        public CellLandscapeType LandscapeType { get => landscape; set => landscape = value; }
        public CellLandscapeNature LandscapeNature { get => nature; set => nature = LandscapeNatureRules(landscape, nature) ? value : CellLandscapeNature.LAWN; }

        public Landscape(CellLandscapeType landscape, CellLandscapeNature nature) {
            LandscapeType = landscape;
            LandscapeNature = nature;
        }

        public static bool LandscapeNatureRules(CellLandscapeType landscape, CellLandscapeNature nature) {
            bool possiblility = true;
            
            switch(landscape) {
                case CellLandscapeType.QUARRY:
                    if (nature == CellLandscapeNature.FOREST || nature == CellLandscapeNature.SWAMP || nature == CellLandscapeNature.LAKE)
                        possiblility = false;
                    break;
                case CellLandscapeType.LOWLAND:
                    if (nature == CellLandscapeNature.SWAMP || nature == CellLandscapeNature.LAKE)
                        possiblility = false;
                    break;
                case CellLandscapeType.PLAIN:
                    if (nature == CellLandscapeNature.LAKE)
                        possiblility = false;
                    break;
                case CellLandscapeType.HIGHLAND:
                    if (nature == CellLandscapeNature.SWAMP || nature == CellLandscapeNature.LAKE)
                        possiblility = false;
                    break;
                case CellLandscapeType.MOUNTAIN:
                    if (nature == CellLandscapeNature.FOREST || nature == CellLandscapeNature.LAKE || nature == CellLandscapeNature.SWAMP)
                        possiblility = false;
                    break;
            }

            return possiblility;
        }

        /*
        MINING_ZONE:
            - QUARRY: LAWN
        SUPPLY_ZONE:
            - LOWLAND: LAKE
            - PLAIN: LAWN
            - HIGHLAND: LAWN
        PRODUCTION_ZONE:
            - PLAIN: LAWN
        HARD_ZONE:
            - PLAIN: LAWN
        PROTECTION_ZONE:
            - PLAIN: LAWN
            - HIGHLAND: LAWN
            - MOUNTAIN: LAWN
        RUINS_ZONE:
            - PLAIN: LAWN
        */
        public bool LandscapeZoneRules(CellZoneType zone) {
            bool possiblility = false;
            switch (zone) {
                case CellZoneType.NOT_ZONED:
                    possiblility = true;
                    break;
                case CellZoneType.PRODUCTION_ZONE:
                    if (landscape == CellLandscapeType.QUARRY && nature == CellLandscapeNature.LAWN)
                        possiblility = true;
                    break;
                case CellZoneType.SUPPLY_ZONE:
                    if ((landscape == CellLandscapeType.LOWLAND && nature == CellLandscapeNature.LAKE) || 
                        (landscape == CellLandscapeType.PLAIN && nature == CellLandscapeNature.LAWN) ||
                        (landscape == CellLandscapeType.HIGHLAND && nature == CellLandscapeNature.LAWN))
                        possiblility = true;
                    break;
                case CellZoneType.INDUSTRY_ZONE:
                    if (landscape == CellLandscapeType.PLAIN && nature == CellLandscapeNature.LAWN)
                        possiblility = true;
                    break;
                case CellZoneType.PROTECTION_ZONE:
                    if ((landscape == CellLandscapeType.PLAIN && nature == CellLandscapeNature.LAWN) ||
                        (landscape == CellLandscapeType.HIGHLAND && nature == CellLandscapeNature.LAWN) ||
                        (landscape == CellLandscapeType.MOUNTAIN && nature == CellLandscapeNature.LAWN))
                        possiblility = true;
                    break;
                case CellZoneType.MAIN_ZONE:
                    possiblility = false;
                    break;   
            }

            return possiblility;
        }
    }

}