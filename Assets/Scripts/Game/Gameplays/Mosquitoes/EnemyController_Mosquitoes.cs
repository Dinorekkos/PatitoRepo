using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Mosquitoes : MonoBehaviour
{
    #region public methods
    public void Initialize()
    {
        float targetSpeed = Random.Range(minMaxSpeed.x, minMaxSpeed.y);

        Speed = targetSpeed;
    }
    #endregion

    #region public variables
    public float Speed
    {
        get { return _speed; }
        private set { _speed = value; }
    }
    #endregion

    #region private methods
    private void Update()
    {
        //Updates the target position
        Vector3 targetPosition = transform.position;
        targetPosition.x -= Speed * Time.deltaTime;
        transform.position = targetPosition;

        if (transform.position.x <= minPosition)
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region private variables
    [SerializeField]
    private Vector2 minMaxSpeed = new Vector2(4,6);

    [SerializeField]
    private float minPosition = -10f;

    private float _speed = 0;
    #endregion
}
