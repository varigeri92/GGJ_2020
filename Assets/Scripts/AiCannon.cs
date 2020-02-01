using UnityEngine;

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

	//Conditions
	private bool targetWillBeHit = false;

    // Update is called once per frame
    void Update()
	{
		//Just for Testing
		//Delete Later
		CalculateHitPoint(barrel.transform.position, GetInitialVelocity(), timeStep, projectileTravelDuration);
		if (targetWillBeHit) {
			return;
		}
		launchForce = Mathf.PingPong(Time.time * 100, 100);
	}

	/// <summary>
	/// Use this for Test shooting by UI Button click
	/// </summary>
	public void Shoot()
	{
		if(targetWillBeHit) {
			Shoot(GetInitialVelocity());
		}
	}

	/// <summary>
	/// Shoot Projectile at Current angle and force
	/// </summary>
	/// <param name="velocity">Velocity.</param>
	private void Shoot(Vector3 velocity)
	{
		GameObject currentProjectile = Instantiate(projectilePrefab, projectieSpawn.position, Quaternion.identity);
		Rigidbody rigidbody = currentProjectile.GetComponent<Rigidbody>();
		rigidbody.velocity = velocity;
	}


	#region TrajectoryCalculations

	private Vector3 GetInitialVelocity()
	{
		return barrel.transform.forward * launchForce;
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
				if (hit.collider.CompareTag("Player")) {
					hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
					targetWillBeHit = true;
					break;
				} 
				else {
					targetWillBeHit = false;
				}
			} 
			else {
				targetWillBeHit = false;
			}
			Debug.DrawLine(prev, pos, Color.red);
			prev = pos;
		}
	}
	#endregion


}
