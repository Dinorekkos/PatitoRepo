using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoMoveY : MonoBehaviour
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
            state = MosquitoPatrollingState.GoingUp;
            originY = transform.position.y;
        }

        private void Update()
        {
            float movement = 0;
            if (state == MosquitoPatrollingState.GoingUp)
            {
                movement = Speed * Time.deltaTime;
                Vector3 targetPosition = transform.position;
                targetPosition.y += movement;
                transform.position = targetPosition;
                
            }

            if (state == MosquitoPatrollingState.GoingDown)
            {   
                movement = Speed * Time.deltaTime;
                Vector3 targetPosition = transform.position;
                targetPosition.y -= movement;
                transform.position = targetPosition;
            }

            

            //Check if enemy is too down
            if (transform.position.y < -_range.y)
            {
                state = MosquitoPatrollingState.GoingUp;
            }

            //Check if enemy is too up
            if (transform.position.y > _range.y)
            {
                state = MosquitoPatrollingState.GoingDown;
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
        public MosquitoPatrollingState state;

        [SerializeField]
        private float _speed = 0;
        [SerializeField]
        private Vector2 _range = new Vector2(0, 5);

        private float originY = 0f;

        [SerializeField]
        public Transform visualTransform;
        #endregion
}

public enum MosquitoPatrollingState
{
    GoingUp,
    GoingDown
}

