using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTarget : MonoBehaviour {
    [SerializeField] PlayerBase playerBase;
    [SerializeField] Transform AttackPosition;
    [SerializeField] GameObject bulletPrefab;

    void Update() {
        //카메라에서 씬을 향해 레이를 쏨
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //레이가 직접 닿을 플레인 (위를 향해 0에 위치)
        Plane GroupPlane = new Plane(Vector3.up, Vector3.zero);

        //레이가 닿은거리
        float rayLength;
        //레이가 플레인과 닿는가
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
