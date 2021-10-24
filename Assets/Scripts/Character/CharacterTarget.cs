using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTarget : MonoBehaviour {
    [SerializeField] Transform AttackPosition;
    [SerializeField] GameObject bulletPrefab;

    void Update() {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;
        if (GroupPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointTolook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointTolook.x, transform.position.y, pointTolook.z));
        }
    }

    void AtkFire() {
        Instantiate(bulletPrefab, AttackPosition.position, transform.rotation); //AttackPoint지점에서 PlayerBullet을 생성후 발사함
    }
}
