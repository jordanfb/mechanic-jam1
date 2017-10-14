using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField]
	private float _startingSpeed;
	[SerializeField]
	private float _startingAngle;
	[SerializeField]
	private AudioClip _hitSound;

	private Rigidbody2D _rigidBody2d;
	private Vector2 _velocity;

	private void Awake()
	{
		_rigidBody2d = GetComponent<Rigidbody2D>();
		_velocity = new Vector2(Mathf.Cos(_startingAngle * Mathf.Deg2Rad), Mathf.Sin(_startingAngle * Mathf.Deg2Rad)) * _startingSpeed;
	}
	private void Update()
	{
		_rigidBody2d.velocity = _velocity;
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		if (other.CompareTag("Wall"))
		{
			BounceWall(collision.contacts[0].normal);
			AudioSource.PlayClipAtPoint(_hitSound,
									collision.contacts[0].point,
									1.0f);
		}
		else if (other.CompareTag("Deadzone"))
		{
			Game.Instance.RemoveLives(1);
			Game.Instance.ReloadLevel();
		}
		else if (other.CompareTag("Paddle"))
		{
			BouncePaddle(other.GetComponent<Paddle>());
			AudioSource.PlayClipAtPoint(_hitSound,
									collision.contacts[0].point,
									1.0f);
		}
		else if (other.CompareTag("Block"))
		{
			BounceWall(collision.contacts[0].normal);
			other.GetComponent<Block>().OnBallHit();
			AudioSource.PlayClipAtPoint(_hitSound,
									collision.contacts[0].point,
									1.0f);
		}
	}
	private void BounceWall(Vector2 normal)
	{
		if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y))
		{
			_velocity.x = -_velocity.x;
		}
		else
		{
			_velocity.y = -_velocity.y;
		}
	}
	private void BouncePaddle(Paddle paddle)
	{
		_velocity = ((Vector2)transform.position - paddle.BounceCenter).normalized * _velocity.magnitude;
	}
}