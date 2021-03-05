using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider2D))]
/// <summary>
/// The character controller that handles the character's gravity and collisions.
/// It requires a Collider2D and a rigidbody to function.
/// </summary>
public class CharacterController2D : MonoBehaviour 
{
	/// the various states of our character
	public CharacterControllerState State { get; protected set; }
	/// the initial parameters
	public CharacterControllerParameters DefaultParameters;
	/// the current parameters
	public CharacterControllerParameters Parameters{get{
			return DefaultParameters;
		}}

	[SerializeField]
	public float disableOneSideCollisionTime = 0.3f;
    [SerializeField]
    public float verticalForceTime = 0.3f;

	[Space(10)]	
	[Header("Collision Masks")]
	/// The layer mask the platforms are on
	public LayerMask PlatformMask=0;
    /// The layer mask the moving platforms are on
    public LayerMask MovingPlatformMask = 0;
    /// The layer mask the one way platforms are on
    public LayerMask OneWayPlatformMask=0;
    /// The layer mask the moving one way platforms are on
    public LayerMask MovingOneWayPlatformMask = 0;
    /// gives you the object the character is standing on
    public GameObject StandingOn { get; protected set; }	
	/// the current velocity of the character
	public Vector2 Speed { get{ return _speed; } }
	/// the value of the forces applied at one point in time 
	public Vector2 ForcesApplied { get; protected set; }

	[Space(10)]	
	[Header("Raycasting")]
	/// the number of rays cast horizontally
	public int NumberOfHorizontalRays = 8;
	/// the number of rays cast vertically
	public int NumberOfVerticalRays = 8;
	/// a small value added to all raycasts to accomodate for edge cases	
	public float RayOffset=0.05f; 

	public Vector3 ColliderCenter {get
		{
			Vector3 colliderCenter = Vector3.Scale(transform.localScale, _boxCollider.offset);
			return colliderCenter;
		}}
	public Vector3 ColliderPosition {get
		{
			Vector3 colliderPosition = transform.position + ColliderCenter;
			return colliderPosition;
		}}
	public Vector3 ColliderSize {get
		{
			Vector3 colliderSize = Vector3.Scale(transform.localScale, _boxCollider.size);
			return colliderSize;
		}}
	public Vector3 BottomPosition {get
		{
			Vector3 colliderBottom = new Vector3(ColliderPosition.x,ColliderPosition.y - (ColliderSize.y / 2),ColliderPosition.z);
			return colliderBottom;
		}}
	public float Friction { get
		{
			return _friction;
		}}

	// parameters override storage
	[SerializeField]
	protected CharacterControllerParameters _overrideParameters;
	// private local references			
	protected Vector2 _speed;
	protected float _friction=0;
	protected float _fallSlowFactor;
	protected Vector2 _externalForce;
	protected Vector2 _newPosition;
	protected Transform _transform;
    [SerializeField]
	protected BoxCollider2D _boxCollider;
    public BoxCollider2D BoxCollider
    {
        get { return _boxCollider; }
        set { _boxCollider = value; }
    }
	protected GameObject _lastStandingOn;
	protected LayerMask _platformMaskSave;
    protected IMovingSystem _movingPlatform = null;
    public IMovingSystem MovingSystem { get { return _movingPlatform; } }

    protected float _movingPlatformCurrentGravity;
    protected const float no_movingPlatform_delay = 0.2f;
    protected float no_movingPlatform_Time = 0;

    protected bool _gravityActive=true;
	public bool isGravityActive{
		get{ return _gravityActive;}
	}

	protected const float _largeValue=500000f;
	protected const float _smallValue=0.0001f;
	protected const float _obstacleHeightTolerance=0.05f;
    protected const float _movingPlatformsGravity = 0;

    protected Vector2 _originalColliderSize;
	protected Vector2 _originalColliderOffset;

	// rays parameters
	protected Rect _rayBoundsRectangle;

	protected List<RaycastHit2D> _contactList;

	/// <summary>
	/// initialization
	/// </summary>
	protected virtual void Awake()
	{
		verticalHitStorage = new RaycastHit2D[NumberOfVerticalRays];
		rightHitStorage = new RaycastHit2D[NumberOfHorizontalRays];
		leftHitStorage = new RaycastHit2D[NumberOfHorizontalRays];
		// we get the various components
		_transform=transform;
        if (_boxCollider == null)
		_boxCollider = (BoxCollider2D)GetComponent<BoxCollider2D>();
		_originalColliderSize = _boxCollider.size;
		_originalColliderOffset = _boxCollider.offset;

		// we test the boxcollider's x offset. If it's not null we trigger a warning.
		if (_boxCollider.offset.x!=0)
		{
			Debug.LogWarning("The boxcollider for "+gameObject.name+" should have an x offset set to zero. Right now this may cause issues when you change direction close to a wall.");
		}

		// raycast list and state init
		_contactList = new List<RaycastHit2D>();
		State = new CharacterControllerState();

		// we add the edge collider platform and moving platform masks to our initial platform mask so they can be walked on	
		_platformMaskSave = PlatformMask;	
		PlatformMask |= OneWayPlatformMask;
        PlatformMask |= MovingPlatformMask;
        PlatformMask |= MovingOneWayPlatformMask;

        ResetParameters ();
		State.Reset();
		SetRaysParameters();
	}

	/// <summary>
	/// Use this to add force to the character
	/// </summary>
	/// <param name="force">Force to add to the character.</param>
	public virtual void AddForce(Vector2 force)
	{
		_speed += force;	
		_externalForce += force;
	}

	/// <summary>
	///  use this to set the horizontal force applied to the character
	/// </summary>
	/// <param name="x">The x value of the velocity.</param>
	public virtual void AddHorizontalForce(float x)
	{
		_speed.x += x;
		_externalForce.x += x;
	}

	/// <summary>
	///  use this to set the vertical force applied to the character
	/// </summary>
	/// <param name="y">The y value of the velocity.</param>
	public virtual void AddVerticalForce(float y)
	{
		_speed.y += y;
		_externalForce.y += y;
        if (y > 0)
        {
            DetachFromMovingPlatform();
        }
    }

	/// <summary>
	/// Use this to set the force applied to the character
	/// </summary>
	/// <param name="force">Force to apply to the character.</param>
	public virtual void SetForce(Vector2 force)
	{
		_speed = force;
		_externalForce = force;
	}

	/// <summary>
	///  use this to set the horizontal force applied to the character
	/// </summary>
	/// <param name="x">The x value of the velocity.</param>
	public virtual void SetHorizontalForce (float x)
	{
		_speed.x = x;
		_externalForce.x = x;
	}

	/// <summary>
	///  use this to set the vertical force applied to the character
	/// </summary>
	/// <param name="y">The y value of the velocity.</param>
	public virtual void SetVerticalForce (float y)
	{
		_speed.y = y;
		_externalForce.y = y;
        if (y > 0)
        {
            DetachFromMovingPlatform();
        }
    }

	/// <summary>
	/// This is called every frame
	/// </summary>
	protected virtual void Update()
	{	
		EveryFrame();
	}

	/// <summary>
	/// Every frame, we apply the gravity to our character, then check using raycasts if an object's been hit, and modify its new position 
	/// accordingly. When all the checks have been done, we apply that new position. 
	/// </summary>
	protected virtual void EveryFrame()
	{
		_contactList.Clear();
		if (_gravityActive)
		{
			_speed.y += (Parameters.Gravity) * Time.deltaTime;
		}

		if (_fallSlowFactor!=0)
		{
			_speed.y*=_fallSlowFactor;
		}
		// we initialize our newposition, which we'll use in all the next computations	
		_newPosition=Speed * Time.deltaTime;

		State.WasGroundedLastFrame = State.IsCollidingBelow;
//		Debug.Log ("Was grounded last frame: "+State.IsCollidingBelow);
		State.WasTouchingTheCeilingLastFrame = State.IsCollidingAbove;
		State.Reset(); 
		// we initialize our rays
		SetRaysParameters();
        HandleMovingPlatforms();
        // we store our current speed for use in moving platforms mostly
        ForcesApplied = _speed;
		// we cast rays on all sides to check for slopes and collisions
		CastRaysToTheSides();
		CastRaysBelow();
		CastRaysAbove();
		// we move our transform to its next position
		_transform.Translate(_newPosition,Space.World);

		SetRaysParameters();
		// we compute the new speed
		if (Time.deltaTime > 0)
		{
			Vector2 tempspeed = _speed;
			_speed = _newPosition / Time.deltaTime;
		}
		// we apply our slope speed factor based on the slope's angle
		if (State.IsGrounded)
		{
			Vector3 _speedTemp = _speed;
			_speed.x *= Parameters.SlopeAngleSpeedFactor.Evaluate(State.BelowSlopeAngle * Mathf.Sign(_speed.y));

			float gravitySlope = 100f;
			if (_speed.x > 0 && State.LateralLeftSlopeAngle > 0){
				_speed.y += (-1 * _speed.x * gravitySlope);
			}
			if (_speed.x < 0 && State.LateralRightSlopeAngle > 0){
				_speed.y += ( 1 * _speed.x * gravitySlope);
			}
		}
		if (!State.OnAMovingPlatform)				
		{
			// we make sure the velocity doesn't exceed the MaxVelocity specified in the parameters
			_speed.x = Mathf.Clamp(_speed.x,-Parameters.MaxVelocity.x,Parameters.MaxVelocity.x);
			_speed.y = Mathf.Clamp(_speed.y,-Parameters.MaxVelocity.y,Parameters.MaxVelocity.y);
		}

        // we change states depending on the outcome of the movement
        if (!State.WasGroundedLastFrame && State.IsCollidingBelow)
        {
            State.JustGotGrounded = true;
        }

		if (State.IsCollidingLeft || State.IsCollidingRight || State.IsCollidingBelow || State.IsCollidingRight)
		{
			OnCharacterColliderHit();
		}

        _externalForce.x = 0;

        //Reduce Y force to 0
        if (verticalForceTime > 0)
        {
            externalForceReduceTime += (1 / verticalForceTime) * Time.deltaTime;
            _externalForce.y = Mathf.Lerp(_externalForce.y, 0, externalForceReduceTime);
        } else
        {
            _externalForce.y = 0;
        }
        
        if (Mathf.Abs(_externalForce.y) < 0.01)
        {
            _externalForce.y = 0;
            externalForceReduceTime = 0;
        }
	}
    private float externalForceReduceTime = 0;

    /// <summary>
    /// If the Character is standing on a moving platform, we match its speed
    /// </summary>
    protected virtual void HandleMovingPlatforms()
    {
        Vector2 newplatformposition;
        if (_movingPlatform != null)
        {
            #region last_code
            State.OnAMovingPlatform = true;
            _transform.Translate(_movingPlatform.CurrentSpeed * Time.deltaTime, Space.World);
            _movingPlatformCurrentGravity = _movingPlatformsGravity;
            _newPosition.y = _movingPlatform.CurrentSpeed.y * Time.deltaTime;
            _speed = -_newPosition / Time.deltaTime;
            SetRaysParameters();
            #endregion last_code
        }
    }

    /// <summary>
    /// Disconnects the Character from its current moving platform.
    /// </summary>
    public virtual void DetachFromMovingPlatform()
    {
        State.OnAMovingPlatform = false;
        _movingPlatform = null;
        _movingPlatformCurrentGravity = 0;
        no_movingPlatform_Time = Time.time;
    }

    RaycastHit2D[] rightHitStorage;
	RaycastHit2D[] leftHitStorage;
	int movingDirection = 1;
	private Vector2 fixPositionSpeed = new Vector2 (1.25f, 4.375f);
	public float epsilon_fix_position = 0.1f;

	protected virtual void CastRaysToTheSides() 
	{
		if (_speed.x < 0 || _externalForce.x < 0)
			movingDirection = -1;
		if (_speed.x > 0 || _externalForce.x > 0)
			movingDirection = 1;

		//Calcula la distancia del ray a trazar
		float horizontalRayLength = Mathf.Abs(_speed.x*Time.deltaTime) + _rayBoundsRectangle.width/2 + RayOffset*2;

		//Calcula la maxima altura y minima altura de los origenes del ray, asi como su centro
		Vector2 horizontalRayCastFromBottom=new Vector2(_rayBoundsRectangle.center.x,
			_rayBoundsRectangle.yMin+_obstacleHeightTolerance);										
		Vector2 horizontalRayCastToTop=new Vector2(	_rayBoundsRectangle.center.x,
			_rayBoundsRectangle.yMax-_obstacleHeightTolerance);

		//Checa el lado derecho
		for (int i=0; i<NumberOfHorizontalRays;i++)
		{
			Vector2 rayOriginPoint = Vector2.Lerp(horizontalRayCastFromBottom,horizontalRayCastToTop,(float)i/(float)(NumberOfHorizontalRays-1));

			if (State.WasGroundedLastFrame && i == 0)
				rightHitStorage[i] = RayCast (rayOriginPoint,(Vector2.right),horizontalRayLength,PlatformMask,Color.red,Parameters.DrawRaycastsGizmos);	
			else
				rightHitStorage[i] = RayCast (rayOriginPoint,(Vector2.right),horizontalRayLength,PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask, Color.red,Parameters.DrawRaycastsGizmos);			


			if (rightHitStorage[i].distance >0)
			{
				float hitAngle = Mathf.Abs(Vector2.Angle(rightHitStorage[i].normal, Vector2.up));

				State.LateralRightSlopeAngle = hitAngle;
				if (Mathf.Abs (Vector2.Angle (rightHitStorage [i].normal, Vector2.left)) < 90) {
					State.RightSlopeAngle = Mathf.Abs (Vector2.Angle (rightHitStorage [i].normal, Vector2.left));
				} else {
					State.RightSlopeAngle = Mathf.Abs (Vector2.Angle (rightHitStorage [i].normal, Vector2.left)) - 180;
				}

				if (hitAngle > Parameters.MaximumSlopeAngle && _newPosition.x >= (epsilon_fix_position * Time.deltaTime)) {
					State.IsCollidingRight = true;						

					State.SlopeAngleOK = false;
			
					_newPosition.x = Mathf.Abs (rightHitStorage [i].point.x - horizontalRayCastFromBottom.x)
						- _rayBoundsRectangle.width / 2
						- RayOffset * 2;

					_newPosition = new Vector2 (Mathf.Clamp(_newPosition.x, -fixPositionSpeed.x * Time.deltaTime, fixPositionSpeed.x * Time.deltaTime), Mathf.Clamp(_newPosition.y, -fixPositionSpeed.y * Time.deltaTime, fixPositionSpeed.y * Time.deltaTime));

					_contactList.Add (rightHitStorage [i]);
					_speed = new Vector2 (0, _speed.y);
					break;
				}
			}						
		}

		//Checa el lado izquierdo
		for (int i=0; i<NumberOfHorizontalRays;i++)
		{
			Vector2 rayOriginPoint = Vector2.Lerp(horizontalRayCastFromBottom,horizontalRayCastToTop,(float)i/(float)(NumberOfHorizontalRays-1));

			if (State.WasGroundedLastFrame && i == 0 )			
				leftHitStorage[i] = RayCast (rayOriginPoint,(Vector2.left),horizontalRayLength,PlatformMask,Color.red,Parameters.DrawRaycastsGizmos);	
			else
				leftHitStorage[i] = RayCast (rayOriginPoint,(Vector2.left),horizontalRayLength,PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask, Color.red,Parameters.DrawRaycastsGizmos);			

			if (leftHitStorage[i].distance >0)
			{
				float hitAngle = Mathf.Abs(Vector2.Angle(leftHitStorage[i].normal, Vector2.up));		

				State.LateralLeftSlopeAngle = hitAngle;

				if (Mathf.Abs (Vector2.Angle (leftHitStorage [i].normal, Vector2.right)) < 90) {
					State.LeftSlopeAngle = Mathf.Abs (Vector2.Angle (leftHitStorage [i].normal, Vector2.right));
				} else {
					State.LeftSlopeAngle = Mathf.Abs (Vector2.Angle (leftHitStorage [i].normal, Vector2.right)) - 180;
				}

				if (hitAngle > Parameters.MaximumSlopeAngle && _newPosition.x <= (-epsilon_fix_position * Time.deltaTime)){

					State.IsCollidingLeft = true;

					State.SlopeAngleOK = false;
				
					_newPosition.x = -Mathf.Abs (leftHitStorage [i].point.x - horizontalRayCastFromBottom.x)
						+ _rayBoundsRectangle.width / 2
						+ RayOffset * 2;
					
					_newPosition = new Vector2 (Mathf.Clamp(_newPosition.x, -fixPositionSpeed.x * Time.deltaTime, fixPositionSpeed.x * Time.deltaTime), Mathf.Clamp(_newPosition.y, -fixPositionSpeed.y * Time.deltaTime, fixPositionSpeed.y * Time.deltaTime));

					_contactList.Add (leftHitStorage [i]);
					_speed = new Vector2 (0, _speed.y);
					break;
				}
			}				
		}
	}
	RaycastHit2D RayCast (Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color,bool drawGizmo=false){
#if UNITY_EDITOR
        if (drawGizmo)
        {
            Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        }
#endif
        return Physics2D.Raycast(rayOriginPoint,rayDirection,rayDistance,mask);		
	}

	/// <summary>
	/// Every frame, we cast a number of rays below our character to check for platform collisions
	/// </summary>
	RaycastHit2D[] verticalHitStorage;
	protected virtual void CastRaysBelow()
	{
		_friction=0;

		if (_newPosition.y < -_smallValue)
		{
			State.IsFalling=true;
		}
		else
		{
			State.IsFalling = false;
		}
        
		if ((Parameters.Gravity > 0) && (!State.IsFalling))
			return;

		float rayLength = _rayBoundsRectangle.height/2 + RayOffset ;

        if (State.OnAMovingPlatform)
		{
			rayLength*=2;
		}	

		if (_newPosition.y<0)
		{
			rayLength+=Mathf.Abs(_newPosition.y);
		}			

		Vector2 verticalRayCastFromLeft=new Vector2(_rayBoundsRectangle.xMin+_newPosition.x,
			_rayBoundsRectangle.center.y+RayOffset);
		Vector2 verticalRayCastToRight=new Vector2(	_rayBoundsRectangle.xMax+_newPosition.x,
			_rayBoundsRectangle.center.y+RayOffset);					

		float smallestDistance=_largeValue; 
		int smallestDistanceIndex=0; 						
		bool hitConnected=false;
		int numberHits = 0;
		for (int i=0; i<NumberOfVerticalRays;i++)
		{
			Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastFromLeft,verticalRayCastToRight,(float)i/(float)(NumberOfVerticalRays-1));

			verticalHitStorage [i] = RayCast (rayOriginPoint, Vector2.down, rayLength, PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask, Color.blue, Parameters.DrawRaycastsGizmos);
			//Si el raycast no detecto una platafoma solida
			if (!verticalHitStorage [i]) {
				if (Speed.y > 0 &&
					!State.OnAMovingPlatform) {
					continue;
				}

				verticalHitStorage [i] = RayCast (rayOriginPoint, Vector2.down, rayLength, PlatformMask, Color.blue, Parameters.DrawRaycastsGizmos);
				if (verticalHitStorage [i].distance < (_rayBoundsRectangle.height / 2)) {
					continue;
				}
			}

			if ((Mathf.Abs(verticalHitStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y)) <  _smallValue)
			{
				break;
			}

			if (verticalHitStorage[i])
			{
				hitConnected=true;
				numberHits++;
				State.BelowSlopeAngle = Vector2.Angle( verticalHitStorage[i].normal, Vector2.up )  ;
				if (verticalHitStorage[i].distance<smallestDistance)
				{
					smallestDistanceIndex=i;
					smallestDistance = verticalHitStorage[i].distance;
				}
            }					
		}

		State.IsOnBorder = (numberHits <= (int)(NumberOfVerticalRays / 2));

		if (hitConnected)
		{
			StandingOn=verticalHitStorage[smallestDistanceIndex].collider.gameObject;
            
			State.IsFalling=false;			
			State.IsCollidingBelow=true;
            
			if (_externalForce.y > 0) {
				if (_speed.y >= 0) {
					_newPosition.y = _speed.y * Time.deltaTime;
					State.IsCollidingBelow = false;
				} else {
					_newPosition.y = -Mathf.Abs(verticalHitStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y) 
						+ _rayBoundsRectangle.height/2 
						+ RayOffset;
				}
			} else {
				_newPosition.y = -Mathf.Abs(verticalHitStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y) 
					+ _rayBoundsRectangle.height/2 
					+ RayOffset;
			}

			if (Mathf.Abs (_newPosition.y) < _smallValue) {
				_newPosition.y = 0;
			}

			// we check if whatever we're standing on applies a friction change
			if (verticalHitStorage[smallestDistanceIndex].collider.GetComponent<CharacterSurfaceModifier>()!=null)
			{
				_friction=verticalHitStorage[smallestDistanceIndex].collider.GetComponent<CharacterSurfaceModifier>().Friction;
			}

            if (Time.time > no_movingPlatform_Time + no_movingPlatform_delay)
            {
                if (State.IsGrounded)
                {
                    _movingPlatform = verticalHitStorage[smallestDistanceIndex].collider.GetComponentInParent<IMovingSystem>();
                }
            }

            if (OneWayPlatformMask.CompareToInt((int)verticalHitStorage[smallestDistanceIndex].collider.gameObject.layer) ||
                MovingOneWayPlatformMask.CompareToInt((int)verticalHitStorage[smallestDistanceIndex].collider.gameObject.layer))
            {
                State.OnAOneSidePlatform = true;
            }
            else {
				State.OnAOneSidePlatform = false;
			}
		}
		else
		{
			State.IsCollidingBelow=false;
            if (State.OnAMovingPlatform)
            {
                DetachFromMovingPlatform();
            }
        }	
	}

	/// <summary>
	/// If we're in the air and moving up, we cast rays above the character's head to check for collisions
	/// </summary>
	protected virtual void CastRaysAbove()
	{
		if (_newPosition.y<0)
			return;

		float rayLength = State.IsGrounded?RayOffset : _newPosition.y*Time.deltaTime;
		rayLength+=_rayBoundsRectangle.height/2;

		bool hitConnected=false; 

		Vector2 verticalRayCastStart=new Vector2(_rayBoundsRectangle.xMin+_newPosition.x,
			_rayBoundsRectangle.center.y);	
		Vector2 verticalRayCastEnd=new Vector2(	_rayBoundsRectangle.xMax+_newPosition.x,
			_rayBoundsRectangle.center.y);	

//		RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfVerticalRays];
		float smallestDistance=_largeValue; 

		for (int i=0; i<NumberOfVerticalRays;i++)
		{							
			Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastStart,verticalRayCastEnd,(float)i/(float)(NumberOfVerticalRays-1));
			verticalHitStorage[i] = RayCast (rayOriginPoint,(Vector2.up),rayLength, PlatformMask & ~OneWayPlatformMask & ~MovingOneWayPlatformMask, Color.green,Parameters.DrawRaycastsGizmos);	

			if (verticalHitStorage[i])
			{
				hitConnected=true;
				if (verticalHitStorage[i].distance<smallestDistance)
				{
					smallestDistance = verticalHitStorage[i].distance;
				}
			}	

		}	

		if (hitConnected)
		{
			if (_newPosition.y > 0){
				_speed.y=0;
				_newPosition.y = 0;
			}

			if ( (State.IsGrounded) && (_newPosition.y<0) )
			{
				_newPosition.y=0;
			}

			State.IsCollidingAbove=true;

//			if (!State.WasTouchingTheCeilingLastFrame)
//			{
//				_newPosition.x=0;
//				_speed = new Vector2(0, _speed.y);
//			}
		}	
	}

	/// <summary>
	/// Creates a rectangle with the boxcollider's size for ease of use and draws debug lines along the different raycast origin axis
	/// </summary>
	public virtual void SetRaysParameters() 
	{
		_rayBoundsRectangle = new Rect(_boxCollider.bounds.min.x,
			_boxCollider.bounds.min.y,
			_boxCollider.bounds.size.x,
			_boxCollider.bounds.size.y);

		Debug.DrawLine(new Vector2(_rayBoundsRectangle.center.x,_rayBoundsRectangle.yMin),new Vector2(_rayBoundsRectangle.center.x,_rayBoundsRectangle.yMax),Color.yellow);  
		Debug.DrawLine(new Vector2(_rayBoundsRectangle.xMin,_rayBoundsRectangle.center.y),new Vector2(_rayBoundsRectangle.xMax,_rayBoundsRectangle.center.y),Color.yellow);
	}


	/// <summary>
	/// Disables the collisions for the specified duration
	/// </summary>
	/// <param name="duration">the duration for which the collisions must be disabled</param>
	public virtual IEnumerator DisableCollisions(float duration = -1)
	{
		// we turn the collisions off
		CollisionsOff();
		// we wait for a few seconds
		yield return new WaitForSeconds (duration);
		// we turn them on again
		CollisionsOn();
	}

	/// <summary>
	/// Disables the collisions with one way platforms for the specified duration
	/// </summary>
	/// <param name="duration">the duration for which the collisions must be disabled</param>
	public virtual IEnumerator DisableCollisionsWithOneWayPlatforms(float duration = -1)
	{
		if (duration == -1){
			duration = disableOneSideCollisionTime;
		}
		// we turn the collisions off
		CollisionsOffWithOneWayPlatforms ();
		// we wait for a few seconds
		yield return new WaitForSeconds (duration);
		// we turn them on again
		CollisionsOn();
	}

    /// <summary>
    /// Disables the collisions with moving platforms for the specified duration
    /// </summary>
    /// <param name="duration">the duration for which the collisions must be disabled</param>
    public virtual IEnumerator DisableCollisionsWithMovingPlatforms(float duration = -1)
    {
        // we turn the collisions off
        CollisionsOffWithMovingPlatforms();
        // we wait for a few seconds
        yield return new WaitForSeconds(duration);
        // we turn them on again
        CollisionsOn();
    }

    /// <summary>
    /// Resets the collision mask with the default settings
    /// </summary>
    public virtual void CollisionsOn()
	{
		PlatformMask=_platformMaskSave;
		PlatformMask |= OneWayPlatformMask;
        PlatformMask |= MovingPlatformMask;
        PlatformMask |= MovingOneWayPlatformMask;
    }

	/// <summary>
	/// Turns all collisions off
	/// </summary>
	public virtual void CollisionsOff()
	{
		PlatformMask=0;
	}

	/// <summary>
	/// Disables collisions only with the one way platform layers
	/// </summary>
	public virtual void CollisionsOffWithOneWayPlatforms()
	{

		PlatformMask -= OneWayPlatformMask;
        PlatformMask -= MovingOneWayPlatformMask;
    }
    /// <summary>
    /// Disables collisions only with moving platform layers
    /// </summary>
    public virtual void CollisionsOffWithMovingPlatforms()
    {
        PlatformMask -= MovingPlatformMask;
        PlatformMask -= MovingOneWayPlatformMask;
    }

    /// <summary>
    /// Resets all overridden parameters.
    /// </summary>
    public virtual void ResetParameters()
	{
		_overrideParameters = DefaultParameters;
	}

	/// <summary>
	/// Slows the character's fall by the specified factor.
	/// </summary>
	/// <param name="factor">Factor.</param>
	public virtual void SlowFall(float factor)
	{
		_fallSlowFactor=factor;
	}

	/// <summary>
	/// Activates or desactivates the gravity for this character only.
	/// </summary>
	/// <param name="state">If set to <c>true</c>, activates the gravity. If set to <c>false</c>, turns it off.</param>	   
	public virtual void GravityActive(bool state)
	{
 		if (state)
		{
			_gravityActive = true;
		}
		else
		{
			_gravityActive = false;
		}
	}

	public virtual void ResizeCollider(Vector2 newSize)
	{
		float newYOffset =_originalColliderOffset.y -  (_originalColliderSize.y - newSize.y)/2 ;

		_boxCollider.size = newSize;
		_boxCollider.offset = newYOffset*Vector3.up;
		SetRaysParameters();
	}

	public virtual void ResetColliderSize()
	{
		_boxCollider.size = _originalColliderSize;
		_boxCollider.offset = _originalColliderOffset;
		SetRaysParameters();
	}

	public virtual bool CanGoBackToOriginalSize()
	{
		// if we're already at original size, we return true
		if (_boxCollider.size == _originalColliderSize)
		{
			return true;
		}
		float headCheckDistance = _originalColliderSize.y*transform.localScale.y ;
		bool headCheck = RayCast(_boxCollider.bounds.min+(Vector3.up*_smallValue),Vector2.up,headCheckDistance,PlatformMask,Color.cyan,true);
		return headCheck;
	}
    
	/// <summary>
	/// triggered when the character's raycasts collide with something 
	/// </summary>
	protected virtual void OnCharacterColliderHit() 
	{
		foreach (RaycastHit2D hit in _contactList )
		{			
			Rigidbody2D body = hit.collider.attachedRigidbody;
			if (body == null || body.isKinematic)
				return;

			Vector3 pushDir = new Vector3(_externalForce.x, 0, 0);

			body.velocity = pushDir.normalized * Parameters.Physics2DPushForce;		
		}		
	}		
}