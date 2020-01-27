using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;



//handles main players and NPCs mutual properties (receiving damage) 
[ExecuteInEditMode]
public class Character : MonoBehaviour {

	protected Rigidbody2D body2d;
	protected SpriteRenderer rnd;

	protected int health;

	public void Start() {

		body2d = GetComponent<Rigidbody2D>();
		rnd = GetComponent<SpriteRenderer>();

		if (Application.isPlaying)
		OnStart();
	}

	public virtual void OnStart() {	}

	public virtual void OnReceiveDamage(int Amt) {

		health -= Amt;
		rnd.color = new Color(0.47f, 0.08f, 0.05f);
		StartCoroutine(ChangeColor());

		if (health <= 0) {
			//Health = 0;
			OnDie();
		} 

	}

	public virtual void SpawnParticles(GameObject Prefab) {
		GameObject partObj = ObjectPool.Alloc(Prefab);
		partObj.transform.position = transform.position;
		partObj.transform.rotation = transform.rotation;
	}

	private void Update() {
		if (Application.isEditor && !Application.isPlaying) {
			OnUpdateInEditor();
			return;
		}

		OnUpdate();
	}

	public virtual void OnUpdate() {
	}

	public virtual void OnUpdateInEditor() {
	}

	public virtual void OnDie() {
		//Debug.Log(this.gameObject + " died");
		ObjectPool.Free(gameObject);
	}

	public virtual void Respawn() {
	}

	IEnumerator ChangeColor() {
		yield return new WaitForSeconds(0.1f);
		rnd.color = Color.white;
	}


}

