using System;
using System.Collections.Generic;
using System.IO;
using TC_basic;
using TC_enemy;
using UnityEngine;
using UnityEngine.U2D;

namespace TC_data {
    public class Account {

        private string username;
        private int UUID;

    }

    public class LevelInfo {

        public GameFieldClass field;
        public LevelWaves waves;
        public string level_name;
        public byte level_status;

        public LevelInfo(GameFieldClass field, LevelWaves waves, string level_name, byte level_status) {
            this.field = field;
            this.waves = waves;
            this.level_name = level_name;
            this.level_status = level_status;
        }

        public bool WriteLevelInfo(string dir, string file_name) {
            StreamWriter writer = new StreamWriter(dir + file_name);
            writer.WriteLine(level_name);
            writer.WriteLine(level_status);
            writer.Write(waves.ToString());

            writer.Close();
            return true;
        }

        public bool ReadLevelInfo(string dir, string file_name) {
            StreamReader reader = new StreamReader(dir + file_name);

            level_name = reader.ReadLine();
            level_status = byte.Parse(reader.ReadLine());

            reader.Close();
            return true;
        }
        
    }

    public static class SpriteManager {

        private static Dictionary<string, Dictionary<string, GameObject>> sprites;

        public static void Initializate(string[] paths) {
            sprites = new Dictionary<string, Dictionary<string, GameObject>>();

            foreach (string path in paths) {
                sprites[path.ToLower()] = new Dictionary<string, GameObject>();
                foreach (GameObject sprite in Resources.LoadAll<GameObject>(path)) {
                    sprites[path.ToLower()][sprite.name.ToLower()] = sprite;
                    Debug.Log(sprite.name);
                }  
            }

        }

        public static GameObject GetSprite(string directory, string sprite_name) {
            return sprites[directory.ToLower()][sprite_name.ToLower()];
        }
    }

    // public class SpriteManager {

    //     public static Dictionary<string, SpriteLoader> sprite_loaders = new Dictionary<string, SpriteLoader>();

    //     public static void AddSpriteLoader(string loader_name, SpriteAtlas atlas) => sprite_loaders[loader_name.ToLower()] = new SpriteLoader(atlas);

    //     public static SpriteLoader GetSpriteLoader(string loader_name) => sprite_loaders[loader_name.ToLower()];

    //     public class SpriteLoader {
    //         private SpriteAtlas atlas;

    //         public Sprite this[string sprite_name] {
    //             get => atlas.GetSprite(sprite_name);
    //         }

    //         public Sprite GetSprite(string sprite_name) => atlas.GetSprite(sprite_name);

    //         public SpriteLoader(SpriteAtlas atlas) {
    //             this.atlas = atlas;
    //         }

    //     } 
    // }
}