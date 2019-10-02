/**
 *  @file RoomTransition.cs
 *
 *  @brief Defines the way the camera should move and follow the player when
 *          they move to a different room
 *
 *  @author: Alex Baker
 *  @date:   September 17 2019
 */

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    /***** Variables *****/

    /* -- Private -- */

    /* -- Public -- */
    public float thrust;        /* thrust force of the player's knockback */
    public float knockbackTime; /* amount of time the knockback lasts */

    /***** Functions *****/

    /**
     * OnTriggerEnter2D(Collider2d)
     *
     * Runs the knockback script for whatever is supposed to be knocked back when
     *  the hitbox collides with the other object
     *
     * @param otherCollider: The thing that is colliding into the object this is
     *                        attached to
     *
     */
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
      /* get the rigidbody of the thing we are colliding in to */
      Rigidbody2D objectHit = otherCollider.GetComponent<Rigidbody2D>();

      /* if it's actually a thing */
      if(objectHit != null)
      {
        /* set the location difference equal to the position of the object we are
            colliding into minus ours position */
        Vector2 locationDifference = objectHit.transform.position - transform.position;

        /* nomalize the difference (so it's not more in the diagonal) and multiplying
            our desired thrust */
        locationDifference = locationDifference.normalized * thrust;

        /* add and impulse force to the object we are colliding into */
        objectHit.AddForce(locationDifference, ForceMode2D.Impulse);

        /* depending on what the object is, set their state to staggered and then
            start that object's knockback coroutine */
        if(otherCollider.gameObject.CompareTag("enemy"))
        {
          objectHit.GetComponent<EnemyParent>().currentState = EnemyState.staggered;

          otherCollider.GetComponent<EnemyParent>().startKnockback(objectHit, knockbackTime);

          /* we want to play the death animation for the enemy, but not if it's
              two enemies colliding into each other. So if this isn't an enemy,
              call the death animation. Otherwise, it is an enemy in which case
              we just want the knockback to occur, not death */
          //TODO this needs to be generalized some way because not every enemy is a green slime
          //      could make it so that the parent class has a call to its child's death function
          //      so we would call the parent classes childDeath() function (or something)
          if(this.gameObject.tag != "enemy")
          {
            otherCollider.GetComponent<GreenSlime>().death();
          }

        }
        else if(otherCollider.gameObject.CompareTag("Player"))
        {
          objectHit.GetComponent<Player>().currentState = PlayerState.staggered;

          otherCollider.GetComponent<Player>().startKnockback(knockbackTime);
        }
      }
    }
}
