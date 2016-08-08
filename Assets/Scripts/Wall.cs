using UnityEngine;
using System.Collections;

	public class Wall : MonoBehaviour
	{
		public AudioClip[] hitSounds;				
		public Sprite dmgSprite;					//Alternate sprite to display after Wall has been attacked by player.
		public bool destructible = false;
		public int hp = 3;							//hit points for the wall.
		public AudioClip breakSound;
		private SpriteRenderer spriteRenderer;		//Store a component reference to the attached SpriteRenderer.
		
		
		void Awake ()
		{
			//Get a component reference to the SpriteRenderer.
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}
		
		
		//DamageWall is called when the player attacks a wall.
		public void DamageWall (int loss)
		{
			if (destructible) {
				//Set spriteRenderer to the damaged wall sprite.
				spriteRenderer.sprite = dmgSprite;
				
				//Subtract loss from hit point total.
				hp -= loss;
				
				//If hit points are less than or equal to zero:
				if(hp <= 0) {
					//Disable the gameObject.
					SoundManager.instance.PlaySingle (breakSound);
					gameObject.SetActive (false);

				} else {
					SoundManager.instance.RandomizeSfx (hitSounds);
				}
			} else {
				SoundManager.instance.PlaySingle (breakSound, 0.3f);
			}



		}
	}
