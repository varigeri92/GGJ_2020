using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Behaviour of AI controlled Cannon
/// Looks for Target Objects, calculates a Hit Point and Fires
/// </summary>
public class AiCannon : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject barrel;
	[SerializeField] private GameObject projectilePrefab;
	[SerializeField] private Transform projectieSpawn;
	[Header("Shooting Parameters")]
	[SerializeField] private float launchForce;
	[SerializeField] private float timeStep;
	[SerializeField] private float projectileTravelDuration = 5;

	//Math Stuff
	private float launchAngle;
	private const float gravity = 9.81f;
	private float elapsedTime = 0;

	//Conditions
	private bool targetLocked = false;
	private bool detectionFinished = false;
	private bool detectionTopReached = false;

	//Target stuff
	private GameObject targetToShoot;
	private List<GameObject> potentialTargets = new List<GameObject>();
	private List<float> launchAngles = new List<float>();

    // Update is called once per frame
    void FixedUpdate()
	{
		//Just for Testing
		//Delete Later
		if(detectionFinished == false) {
			LookForTargets();
		}
	}

	/// <summary>
	/// Use this for Test shooting by UI Button click
	/// </summary>
	public void Shoot()
	{
		if(targetLocked) {
			StartCoroutine(Shoot(true));
		}
	}

	/// <summary>
	/// Shoot Projectile at Current angle and force
	/// </summary>
	/// <param name="velocity">Velocity.</param>
	IEnumerator Shoot(bool guiCall = true)
	{
		elapsedTime = 0;
		yield return RotateTo(launchAngle);
		yield return new WaitForSecondsRealtime(.5f);

		GameObject currentProjectile = Instantiate(projectilePrefab, projectieSpawn.position, Quaternion.identity);
		Rigidbody rigidbody = currentProjectile.GetComponent<Rigidbody>();
		rigidbody.velocity = GetInitialVelocity();

		yield break;
	}

	IEnumerator RotateTo(float angle)
	{
		float currentAngle = barrel.transform.localEulerAngles.z; 
		while ((angle - currentAngle) > 0.1f) {
			elapsedTime += Time.fixedDeltaTime;
			if (angle > currentAngle) {
				currentAngle += Mathf.PingPong(elapsedTime * 15, 90);
			} else {
				currentAngle -= Mathf.PingPong(elapsedTime * 15, 90);
			}
			SetCannonRotation(currentAngle);
			yield return new WaitForEndOfFrame();
		}
	}

	private void LookForTargets()
	{
		CalculateHitPoint(barrel.transform.position, GetInitialVelocity(), timeStep, projectileTravelDuration);

		elapsedTime += Time.fixedDeltaTime;
		launchAngle = Mathf.PingPong(elapsedTime * 30, 90);
		SetCannonRotation(launchAngle);

		if(Mathf.Abs(launchAngle) >= 89) {
			detectionTopReached = true;
		}
		//Cannon has checked level once
		//Decide which enemy to shoot
		if (Mathf.Abs(launchAngle) < 1) {
			if (detectionTopReached == true) {
				if (targetLocked == false) {
					PickTarget();
				}
			}
		}
	}

	/// <summary>
	/// Selects a random Target and marks it in Red
	/// </summary>
	private void PickTarget()
	{
		if(potentialTargets.Count == 0){
			return;
		}
		int targetIndex = Random.Range(0, potentialTargets.Count);
		targetToShoot = potentialTargets[targetIndex];
		launchAngle = launchAngles[targetIndex];
		MarkTarget(targetToShoot, Color.red);
		detectionFinished = true;
		targetLocked = true;

		Shoot();
	}

	/// <summary>
	/// Add New target to list of potential targets (if its not already in it)
	/// Mark it blue
	/// </summary>
	/// <param name="newTarget">New target.</param>
	private void AddTargetToList(GameObject newTarget)
	{
		if(!potentialTargets.Contains(newTarget)) {
			potentialTargets.Add(newTarget);
			launchAngles.Add(launchAngle);
			MarkTarget(newTarget, Color.blue);
		}
	}

	private void MarkTarget(GameObject target, Color color)
	{
		target.GetComponent<MeshRenderer>().material.color = color;
	}

	private void SetCannonRotation(float newAngle)
	{
		barrel.transform.localEulerAngles = new Vector3(
		0,
		0,
		newAngle);
	}

	public void Reset()
	{
		potentialTargets.Clear();
		launchAngles.Clear();
		targetLocked = false;
		detectionTopReached = false;
		detectionFinished = false;
		launchAngle = 0;
		elapsedTime = 0;
		StartCoroutine(RotateTo(0));
	}

	#region TrajectoryCalculations

	private Vector3 GetInitialVelocity()
	{
		return barrel.transform.right * launchForce;
	}

	/// <summary>
	/// Calculate Projectile Trajectory at given Time
	/// </summary>
	/// <returns>The trajectory at time.</returns>
	/// <param name="start">Start.</param>
	/// <param name="startVelocity">Start velocity.</param>
	/// <param name="time">Time.</param>
	public Vector3 GetTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
	{
		return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
	}

	/// <summary>
	/// Actual Calculation if Projectile hits anything after given Time
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="startVelocity">Start velocity.</param>
	/// <param name="timestep">Timestep.</param>
	/// <param name="maxTime">Max time.</param>
	private void CalculateHitPoint(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
	{
		Vector3 prev = start;
		for (int i = 1; ; i++) {
			float t = timestep * i;
			if (t > maxTime) break;
			Vector3 pos = GetTrajectoryAtTime(start, startVelocity, t);
			RaycastHit hit;
			if (Physics.Linecast(prev, pos, out hit)) {
				//Check if correct Target will be hit
				if (hit.collider.CompareTag("OurHeroes")) {
					AddTargetToList(hit.collider.gameObject);
					break;
				}
			}
			Debug.DrawLine(prev, pos, Color.red);
			prev = pos;
		}
	}
	#endregion


}
