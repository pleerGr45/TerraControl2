namespace TC_basic {
    public enum CellZoneType : byte {
        NOT_ZONED = 0,
        PRODUCTION_ZONE = 1,
        SUPPLY_ZONE = 2,
        INDUSTRY_ZONE = 3,
        PROTECTION_ZONE = 4,
        MAIN_ZONE = 5
    }

    public enum CellResourceType : byte {
        LIMESTONE = 0,
        QUARTZ = 1,
        OIL = 2,
        IRON = 3,
        COPPER = 4,
        LEAD = 5,
        IRIDIUM = 6,
        URANIUM = 7,
    }

    public enum ProductType : byte {
        LIMESTONE_RAW = 0,
        QUARTZ_RAW = 1,
        OIL_RAW = 2,
        IRON_RAW = 3,
        COPPER_RAW = 4,
        LEAD_RAW = 5,
        IRIDIUM_RAW = 6,
        URANIUM_RAW = 7,
        CONCRETE = 8,
        CHEMICALS = 9,
        FUEL = 10,
        STEELWORK = 11,
        LEAD_PLATE = 12,
        IRIDIUM_PLATE = 13,
        WIRES = 14,
        URANIUM_ROD = 15,
        GLASS = 16,
        BLOCK = 17,
        ELECTRIC = 18,
        TRANSFORMER = 19,
        LASER = 20,
        CORPUS = 21,
        AMMO = 22,
        EXPLOISIVE_AMMO = 23,
        PIERCING_ROCKET = 24,
        GLASS_ROCKET = 25,
        TOXIC_ROCKET = 26,
        SUPER_ROCET = 27
    }

    public enum CellLandscapeType : byte {
        
        /**<summary> <b>QUARRY (terraforming the lawn)</b> - NON-building area
            <br>standard ecology = 1</br>
            <br>standard logistics = 1</br>
            <br>Can place on: <i>LOWLAND, PLAINS</i></br> 
        </summary>*/
        QUARRY = 0,
        LOWLAND = 1,
        PLAIN = 2,
        HIGHLAND = 3,
        MOUNTAIN = 4,
    }

    public enum CellLandscapeNature : byte {
        
        /**<summary> 
            <b>LAWN (terraforming the forest, swamp, natural)</b> - building area
            <br>standard ecology = 0.9</br>
            <br>standard logistics = 2</br>
            <br>Can place on: <i>QUARRY, LOWLAND, PLAINS, HIGHLAND, MOUNTAIN</i></br>
        </summary>*/
        LAWN = 1,
        /**<summary> 
            <b>FORETS (natural)</b> - NON-building area, 
            <br>standard ecology = 1.0f</br>
            <br>standard logistics = 1</br>
            <br>Can place on: <i>PLAINS, HILL</i></br>
        </summary>*/
        FOREST = 0,
        /**<summary> 
            <b>SWAMP (natural)</b> - NON-building area
            <br>standard ecology = 1</br>
            <br>standard logistics = 1</br>
            <br>Can place on: <i>PLAINS</i></br> 
        </summary>*/
        SWAMP = 3,
        /**<summary> <b>LAKE (natural)</b> - NON-building area
            <br>standard ecology = 1</br>
            <br>standard logistics = 1</br>
            <br>Can place on: <i>LOWLAND, PLAINS</i></br> 
        </summary>*/
        LAKE = 4,
    }

    public enum BuildingType : byte {
        MINE_BUILDING = 0,
        TPP_BUILDING = 1,
        NPP_BUILDING = 2,
        WINDMILL_BUILDING = 3,
        PRODUCT_BUILDING = 4
    }

    public enum DamageType : byte {
        PHYSICAL = 0,
        FIRE = 1,
        EXPLOSIVE = 2,
        TOXIC = 3   
    }
}