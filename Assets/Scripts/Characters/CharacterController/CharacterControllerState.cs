using UnityEngine;
using System.Collections;

[System.Serializable]
public class CharacterControllerState {
	/// is the character colliding right ?
	public bool IsCollidingRight { get; set; }
	/// is the character colliding left ?
	public bool IsCollidingLeft { get; set; }
	/// is the character colliding with something above it ?
	public bool IsCollidingAbove { get; set; }
	/// is the character colliding with something above it ?
	public bool IsCollidingBelow { get; set; }
	// is the character in border
	public bool IsOnBorder {get; set;}
	// is the character crouched
	public bool IsCrouched { get; set;}

	public bool IsCompletelyCrouched{ get; set;}

	/// is the character colliding with anything ?
	public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; }}

	public float RightSlopeAngle { get; set;}
	public float LeftSlopeAngle { get; set;}

//	/// returns the slope angle met horizontally
//	public float LateralSlopeAngle { get; set; }

	public float LateralRightSlopeAngle { get; set;}
	public float LateralLeftSlopeAngle { get; set;}

	/// returns the slope the character is moving on angle
	public float BelowSlopeAngle { get; set; }
	/// returns true if the slope angle is ok to walk on
	public bool SlopeAngleOK { get; set; }
	/// returns true if the character is standing on a moving platform
	public bool OnAMovingPlatform { get; set; }

	// returns true if the character is standing on a one-side platform
	public bool OnAOneSidePlatform { get; set; }

	/// Is the character grounded ? 
	public bool IsGrounded { get { return IsCollidingBelow; } }
	/// is the character falling right now ?
	public bool IsFalling { get; set; }
	/// was the character grounded last frame ?
	public bool WasGroundedLastFrame { get ; set; }
	/// was the character grounded last frame ?
	public bool WasTouchingTheCeilingLastFrame { get ; set; }
	/// did the character just become grounded ?
	public bool JustGotGrounded { get ; set;  }

	public bool UsingConstantMovement { get; set;}
	/// <summary>
	/// Reset all collision states to false
	/// </summary>
	public virtual void Reset()
	{
		IsCollidingLeft = 
			IsCollidingRight = 
				IsCollidingAbove =
					SlopeAngleOK =
						JustGotGrounded = false;
		IsFalling=true;
		LateralRightSlopeAngle = 0;
		LateralLeftSlopeAngle = 0;
		RightSlopeAngle = 0;
		LeftSlopeAngle = 0;
//		UsingConstantMovement = false;
	}

	/// <summary>
	/// Serializes the collision states
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current collision states.</returns>
	public override string ToString ()
	{
		return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}",
			IsCollidingRight,
			IsCollidingLeft,
			IsCollidingAbove,
			IsCollidingBelow,
			LateralLeftSlopeAngle,
			LateralRightSlopeAngle);
	}	
}
