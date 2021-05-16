using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplays.Platformer
{
    public class EnemyPatrolling_Platformer : MonoBehaviour
    {
        #region public methods
        #endregion

        #region public variables
        public float Speed
        {
            get { return _speed; }
            private set { _speed = value; }
        }
        #endregion

        #region private methods
        private void Start()
        {
            state = EnemyPatrollingState.GoingLeft;
            originX = transform.position.x;
        }

        private void Update()
        {
            float movement = 0;
            if (state == EnemyPatrollingState.GoingRight)
            {
                movement = Speed * Time.deltaTime;
            }

            if (state == EnemyPatrollingState.GoingLeft)
            {
                movement = -Speed * Time.deltaTime;
            }

            Vector3 targetPosition = transform.position;
            targetPosition.x += movement;
            transform.position = targetPosition;

            //Check if enemy is too left
            if (transform.position.x < originX + _range.x)
            {
                state = EnemyPatrollingState.GoingRight;
            }

            //Check if enemy is too right
            if (transform.position.x > originX + _range.y)
            {
                state = EnemyPatrollingState.GoingLeft;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<HealthEntity>())
            {
                other.GetComponent<HealthEntity>().MakeDamage(1);
            }
        }
        #endregion

        #region private variables
        private EnemyPatrollingState state;

        [SerializeField]
        private float _speed = 0;
        [SerializeField]
        private Vector2 _range = new Vector2(-5, 5);

        private float originX = 0f;
        #endregion
    }
}
public enum EnemyPatrollingState
{
    GoingRight,
    GoingLeft
}
