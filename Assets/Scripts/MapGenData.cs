using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenData : MonoBehaviour {
    public int x, y;
    public bool isClear = false;
    public bool rightRoom = false;
    public bool leftRoom = false;
    public bool downRoom = false;
    public bool upRoom = false;

    [SerializeField] GameObject EastDoorObject = null;
    [SerializeField] GameObject WestDoorObject = null;
    [SerializeField] GameObject NorthDoorObject = null;
    [SerializeField] GameObject SouthDoorObject = null;
    [SerializeField] GameObject EastBridgeObject = null;
    [SerializeField] GameObject WestBridgeObject = null;
    [SerializeField] GameObject NorthBridgeObject = null;
    [SerializeField] GameObject SouthBridgeObject = null;
    [SerializeField] GameObject EastWallObject = null;
    [SerializeField] GameObject WestWallObject = null;
    [SerializeField] GameObject NorthWallObject = null;
    [SerializeField] GameObject SouthWallObject = null;

    public void SetMapPoint(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void DirectionalInfoUpdate() {
        EastWallObject.SetActive(true);
        WestWallObject.SetActive(true);
        NorthWallObject.SetActive(true);
        SouthWallObject.SetActive(true);

        if (rightRoom) {
            EastBridgeObject.SetActive(true);
            EastDoorObject.SetActive(false);
            EastWallObject.SetActive(false);
        }
        if (leftRoom) {
            WestBridgeObject.SetActive(true);
            WestDoorObject.SetActive(false);
            WestWallObject.SetActive(false);
        }
        if (upRoom) {
            NorthBridgeObject.SetActive(true);
            NorthDoorObject.SetActive(false);
            NorthWallObject.SetActive(false);
        }
        if (downRoom) {
            SouthBridgeObject.SetActive(true);
            SouthDoorObject.SetActive(false);
            SouthWallObject.SetActive(false);
        }
    }
}
