
using System.Collections.Generic;
using UnityEngine;

namespace TC_func {
    public class Control {
        public static Dictionary<string, KeyCode[]> key_settings;

        public static void LoadSettings() {
            key_settings = new Dictionary<string, KeyCode[]>();
            
            key_settings["camera_move_forward"] = new KeyCode[] {KeyCode.W, KeyCode.UpArrow};
            key_settings["camera_move_back"] = new KeyCode[] {KeyCode.S, KeyCode.DownArrow};
            key_settings["camera_move_right"] = new KeyCode[] {KeyCode.D, KeyCode.RightArrow};
            key_settings["camera_move_left"] = new KeyCode[] {KeyCode.A, KeyCode.LeftArrow};
            key_settings["camera_rotate_right"] = new KeyCode[] {KeyCode.E, KeyCode.PageUp};
            key_settings["camera_rotate_left"] = new KeyCode[] {KeyCode.Q, KeyCode.PageDown};
            key_settings["camera_zoom_in"] = new KeyCode[] {KeyCode.Equals, KeyCode.Equals};
            key_settings["camera_zoom_out"] = new KeyCode[] {KeyCode.Minus, KeyCode.Minus};
        }

        public static bool GetKeyControl(string key) {
            try {
                return Input.GetKey(key_settings[key][0]) || Input.GetKey(key_settings[key][1]);
            } catch {
                Debug.LogError("Control key {" + key + "} not found");
                return false;
            }
        } 
    }
}