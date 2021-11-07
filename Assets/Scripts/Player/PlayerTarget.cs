using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour {
    [SerializeField] PlayerBase playerBase;
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
        GameObject bulletObject = Instantiate(bulletPrefab, AttackPosition.position, transform.rotation);
        bulletObject.GetComponent<PlayerBullet>().InitBullet(playerBase.bulletSpd, playerBase.bulletRange, playerBase.attackDmg); //AttackPoint지점에서 PlayerBullet을 생성후 발사함
    }
}
