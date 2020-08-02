using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapEnemy : Enemy
{
    [SerializeField]
    private float captureTime = 2f;

    // Move in front of the photon when normal time
    // Move behind the photon when rewinding
    // Capture photon for 2 seconds when hit
    // Die when hit by laser

    public override void PhotonHit()
    {
        // TODO: capture photon
    }

    public override void LaserHit()
    {
        Die();
    }
}
