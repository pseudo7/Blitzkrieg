

var mainRotor : Transform; // set in the inspector
var tailRotor : Transform; // set in the inspector
var helicopterSound : AudioSource; // set in the inspector

// Set/Change from another script:
// Rotor rotation:
public var targetRPM : float; // 0.0 (low RPM) to 1.0 (full 1460 RPM)
public var engineOn : boolean;
public var useAcceleration : boolean; // offers a very basic rotor acceleration
public var currentRPM : float; // differs from targetRPM due to acceleration

private var disableAudio = false;
private var clampedRPM : float;
private var accelerationRate : float;
private var initialVolume : float;
private var hasRotors : boolean;
private var dontExecute : boolean = false;


function Awake()
{
	accelerationRate=0.2; // acceleration speed of the rotors
	WindowAccelerationRate=3.0;  // acceleration speed when opening the Windows
	ControlSetup();
}	

function Update() 
{
	if(hasRotors) HelicopterRotor();	
}

function HelicopterRotor()
{
	if(engineOn || (currentRPM>0.0 && useAcceleration))
	{	
		clampedRPM=Mathf.Clamp(targetRPM*0.7+0.3, 0.3, 1.0); // clamps the targetRPM to avoid unrealistically low continiuos RPM

		if(useAcceleration)
		{	
			if(engineOn)
			{	
				if(currentRPM<=clampedRPM)
				{
					currentRPM = Mathf.Clamp(currentRPM + (accelerationRate * Time.deltaTime), 0.0, clampedRPM);
				}
				else
				{					
					currentRPM = Mathf.Clamp(currentRPM - (accelerationRate * Time.deltaTime), 0.0, 1.0);
				}	
			}
			else
			{
				currentRPM = Mathf.Clamp(currentRPM - (accelerationRate * Time.deltaTime), 0.0, 1.0);
			}	
		}
		else
		{
			currentRPM=clampedRPM;
		}		

		mainRotor.Rotate(Vector3(0.0, -1460.0 * currentRPM * Time.deltaTime, 0.0), Space.Self);
		
			tailRotor.Rotate(Vector3(1460.0 * currentRPM * Time.deltaTime, 0.0, 0.0), Space.Self);

		// rotors spinning - synchronize particles and audio to rotor speed
		if(disableAudio==false)
		{
			if(helicopterSound.isPlaying==false) helicopterSound.Play();
			helicopterSound.pitch = 0.1+currentRPM;			
			helicopterSound.volume=Mathf.Clamp(initialVolume * Mathf.Clamp(currentRPM*3.333, 0.0, 1.0), 0.0, initialVolume);
		}
	}
	else
	{
		if(disableAudio==false) helicopterSound.Stop();
	}	
}

function ControlSetup()
{
	hasRotors=true;

	if(!mainRotor)
	{
		hasRotors=false;
		Debug.Log("AH-1: "+this.name+" – won't rotate rotors – manually attach the transform of the Main Rotor in the Inspector.");
	}

	if(!tailRotor) 
	{
		hasRotors=false;
		Debug.Log("AH-1: "+this.name+" –  won't rotate rotors – manually attach the transform of the Tail Rotor in the Inspector.");
	}

	if(!helicopterSound)
	{
		disableAudio=true;
	}
	else
	{
		initialVolume=helicopterSound.volume;
	}	
}	