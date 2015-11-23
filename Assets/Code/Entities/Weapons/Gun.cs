using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Explosion;

    public void Shoot(Vector3 position, Vector3 direction)
    {
        var ray = new Ray2D(position, direction);

        const float shotDistance = 20;

        var hit = Physics2D.Raycast(ray.origin, ray.direction, shotDistance);
        if (hit.collider == null) return; //nothing hit, so return

        //we hit something, draw explosion @ impact location
        var vector2 = ray.origin + ray.direction * hit.distance;
        var explosion = Instantiate(Explosion, vector2, Quaternion.identity);

        Destroy(explosion, 0.03f);

        //but what did we hit ??
        var hitObject = hit.collider.attachedRigidbody.gameObject;

        if (hitObject.tag == "Badguy")
            Destroy(hitObject);
    }
}
