using System;
using TC_basic;
using TC_data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

public class HexClickReciver : MonoBehaviour, IPointerClickHandler
{

    [NonSerialized] public Cell cell;
    [NonSerialized] public int i_cord, j_cord;
    [NonSerialized] public GameObject panel;
    [NonSerialized] public LocalizationCanvasDataScript canvas_locale;



    public void OnPointerClick(PointerEventData eventData)
    {
        ProjectCellInfo();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas canvas = GameObject.Find("MainScript").GetComponent<MainScript>().Canvas;
        panel = canvas.transform.GetChild(0).gameObject;
        canvas_locale = canvas.GetComponent<LocalizationCanvasDataScript>();
        canvas_locale.i_cord = i_cord.ToString();
        canvas_locale.j_cord = j_cord.ToString();

    }

    public void ProjectCellInfo() {
        if (cell == null) return;
        
        GameObject slider_background = panel.transform.GetChild(1).gameObject;
        slider_background.transform.GetChild(0).gameObject.GetComponent<Slider>().value = cell.Supply / (cell.Supply + cell.Demand);
        slider_background.transform.GetChild(1).gameObject.GetComponent<Slider>().value = cell.Demand / (cell.Supply + cell.Demand);

        // Hex Name
        SetText(panel.transform.GetChild(2).gameObject, "Клетка " + i_cord + " " + j_cord);
        TextMeshProUGUI zone = panel.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        zone.text = cell.CellZone.GetZoneName();
        zone.color = cell.CellZone.GetZoneColor();

        Transform resource_panel = panel.transform.GetChild(4);

        for (int i = 0; i < resource_panel.childCount; i++) {
            Destroy(resource_panel.GetChild(i).gameObject);
        }

        for (int i = 0; i < cell.Resources.Length; i++) {
            GameObject sprite = GameObject.Instantiate(SpriteManager.GetSprite("Icons", ResourceNameIcon(cell.Resources[i])), new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), resource_panel);
            sprite.transform.localPosition = new Vector3((i * 25)-100f, 0, 0);
        }

        // Buildings
        GameObject build_button_panel = panel.transform.GetChild(5).gameObject;
        BuildButtonScript b1 = build_button_panel.transform.GetChild(0).GetComponent<BuildButtonScript>(); b1.cell = cell; b1.i = i_cord; b1.j = j_cord;
        BuildButtonScript b2 = build_button_panel.transform.GetChild(2).GetComponent<BuildButtonScript>(); b2.cell = cell; b2.i = i_cord; b2.j = j_cord;
        BuildButtonScript b3 = build_button_panel.transform.GetChild(4).GetComponent<BuildButtonScript>(); b3.cell = cell; b3.i = i_cord; b3.j = j_cord;
        BuildButtonScript b4 = build_button_panel.transform.GetChild(6).GetComponent<BuildButtonScript>(); b4.cell = cell; b4.i = i_cord; b4.j = j_cord;

        BuildButtonScript d1 = build_button_panel.transform.GetChild(1).GetComponent<BuildButtonScript>(); d1.cell = cell; d1.i = i_cord; d1.j = j_cord;
        BuildButtonScript d2 = build_button_panel.transform.GetChild(3).GetComponent<BuildButtonScript>(); d2.cell = cell; d2.i = i_cord; d2.j = j_cord;
        BuildButtonScript d3 = build_button_panel.transform.GetChild(5).GetComponent<BuildButtonScript>(); d3.cell = cell; d3.i = i_cord; d3.j = j_cord;
        BuildButtonScript d4 = build_button_panel.transform.GetChild(7).GetComponent<BuildButtonScript>(); d4.cell = cell; d4.i = i_cord; d4.j = j_cord;
    }


    public static void SetText(GameObject text_object, string text) {
        text_object.GetComponent<TextMeshProUGUI>().text = text;
    }

    public static string ResourceNameIcon(CellResourceType type) {
        string resource_name = "";

        switch (type) {
            case CellResourceType.LIMESTONE: resource_name = "Limestone"; break;
            case CellResourceType.QUARTZ: resource_name = "Quartz"; break;
            case CellResourceType.OIL: resource_name = "Oil"; break;
            case CellResourceType.IRON: resource_name = "Iron"; break;
            case CellResourceType.COPPER: resource_name = "Copper"; break;
            case CellResourceType.LEAD: resource_name = "Plumbum"; break;
            case CellResourceType.IRIDIUM:resource_name = "Iridium"; break;
            case CellResourceType.URANIUM: resource_name = "Uranium"; break;
        }

        return resource_name;
    }
}
