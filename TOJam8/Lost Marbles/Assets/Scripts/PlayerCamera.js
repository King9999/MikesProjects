//PlayerCamera.js: A script to control the camera and make it smoothly follow Widget
//#pragma strict

//public class PlayerCamera : MonoBehaviour {

	var target : Transform;
	//the distance for x and z for the camera to stay from the target
	var distance: float = 5.0;
	//the distance in y for the camera to stay from the target
	var height: float = 4.0;
	
	//speed controls for the camera - how fast it catches up to the moving object
	var heightDamping: float = 2.0;
	var rotationDamping: float = 3.0;
	var distanceDampingX: float = 0.5;
	var distanceDampingZ: float = 0.2;
	
	function LateUpdate(){
		//check to make sure a target has been assigned in inspector
		if(!target){
			return;
		}
		
		//calculate current rotation angles, positions and where camera should end up
		wantedRotationAngle = target.eulerAngles.y;
		wantedHeight = target.position.y + height;
		wantedDistanceZ = target.position.z - distance;
		wantedDistanceX = target.position.x - distance;
		
		currentRotationAngle = transform.eulerAngles.y;
		currentHeight = transform.position.y;
		currentDistanceZ = transform.position.z;
		currentDistanceX = transform.position.x;
		
		//Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle
			(currentRotationAngle, wantedRotationAngle, 
			rotationDamping * Time.deltaTime);
			
		//Damp the distance
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, 
						heightDamping * Time.deltaTime);
		currentDistanceZ = Mathf.Lerp(currentDistanceZ, 
						wantedDistanceZ, distanceDampingZ * Time.deltaTime);
		currentDistanceX = Mathf.Lerp(currentDistanceX, 
						wantedDistanceX, distanceDampingX * Time.deltaTime);
		
		//Convert angel into a rotation
		currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
		
		//Set New Position of Camera
		transform.position -= currentRotation * Vector3.forward * distance;
		transform.position.x = currentDistanceX;
		transform.position.z = currentDistanceZ;
		transform.position.y = currentHeight;
		
		//make sure camera is looking at target
		LookAtMe();
	
	}
	
	function LookAtMe(){
		//camera controls for looking at target
		var camSpeed: float = 5.0;
		var smoothed: boolean = true;
		
		if(smoothed){
		//find new rotation values based on target and camera's current pos	
		//then interpolate smoothly between 2 using spec speed settings
		var camRotation = Quaternion.LookRotation(target.position 
			- transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, 
			camRotation, Time.deltaTime * camSpeed);
		}
		
		//this default will flatly move with targeted object
		else{
		transform.LookAt(target);
		}	
	
	}

@script AddComponentMenu("Player/Smooth Follow Camera")
