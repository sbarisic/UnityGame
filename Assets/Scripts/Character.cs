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

	public virtual void OnDie() {
		Debug.Log(this.gameObject + "I died");
		ObjectPool.Free(gameObject);
	}

	IEnumerator ChangeColor() {
		yield return new WaitForSeconds(0.1f);
		rnd.color = Color.white;
	}


}

