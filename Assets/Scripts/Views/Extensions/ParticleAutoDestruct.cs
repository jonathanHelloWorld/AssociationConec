using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleAutoDestruct : MonoBehaviour
{
	public bool OnlyDeactivate;
	
    void FixedUpdate()
    {
        if (!GetComponent<ParticleSystem>().IsAlive(true))
        {
            if (OnlyDeactivate) this.gameObject.SetActive(false);

            else GameObject.Destroy(this.gameObject);
        }
    }
}
