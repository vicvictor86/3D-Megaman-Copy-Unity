using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetEnemy : HorizontalEnemy
{
    protected override void Move()
    {
        if (!isViewingPlayer)
        {
            canTakeDamage = true;
            var velocity = Rb.velocity;
            var moveDirection = horizontalFacing == HorizontalFacing.Left ? -1 : 1;
            Rb.velocity = new Vector3(speed * moveDirection, velocity.y, velocity.z);
        }
        else
        {
            Hidden();
        }
    }

    private void Hidden()
    {
        canTakeDamage = false;
        //Hidden animation
    }
}
