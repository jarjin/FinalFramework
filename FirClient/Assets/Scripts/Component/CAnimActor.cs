using System;
using UnityEngine;

namespace FirClient.Component
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class CAnimActor : MonoBehaviour
	{
		[Serializable]
		public struct AntAnimation
		{
			public string name;
			public Sprite[] frames;
		}

		public string initialAnimation;
		public AntAnimation[] animations;
		public float timeScale = 1.0f;
		public bool reverse = false;
		public bool loop = true;
		public float loopDelay = 0.0f;

		public delegate void AnimationCompleteDelegate(CAnimActor aActor, string aAnimationName);
		public event AnimationCompleteDelegate EventAnimationComplete;

		private SpriteRenderer _sprite;
		private AntAnimation _currentAnimation;
		private float _animationSpeed = 29.0f;
		private float _currentFrame;
		private bool _isPlaying;
		private bool _isPaused;
		private int _prevFrame;
		private int _complete;
		private float _delay;

		protected virtual void Awake()
		{
			_sprite = GetComponent<SpriteRenderer>();
			_currentFrame = 1.0f;
			_isPlaying = true;
			_isPaused = false;
			_prevFrame = -1;
			_delay = 0.0f;

			if (initialAnimation != null)
			{
				SwitchAnimation(initialAnimation);
			}
		}

		protected virtual void Update()
		{
			if (_isPlaying)
			{
				if (_complete == 2)
				{
					_complete = 0;
				}
				else if (_complete == 1)
				{
					_complete = 2;
				}

				if (reverse)
				{
					PrevFrame();
					if (loop && _currentFrame < 0.0f)
					{
						_currentFrame = (float)(TotalFrames - 1);
						SetFrame(_currentFrame);
						AnimationComplete();
					}
				}
				else
				{
					NextFrame();
					if (loop && _currentFrame > (float)TotalFrames - 1)
					{
						_currentFrame = 0.0f;
						SetFrame(_currentFrame);
						AnimationComplete();	
					}
				}
			}
			else if (_isPaused)
			{
				_delay -= Time.deltaTime;
				if (_delay <= 0.0f)
				{
					_delay = 0.0f;
					Play();
				}
			}
		}

		public void SwitchAnimation(string aAnimationName)
		{
			if (aAnimationName != _currentAnimation.name)
			{
				bool animationFound = false;
				for (int i = 0, n = animations.Length; i < n; i++)
				{
					if (animations [i].name == aAnimationName)
					{
						animationFound = true;
						_currentAnimation = animations [i];
						_currentFrame = 1.0f;
						_prevFrame = -1;
						break;
					}
				}

				if (!animationFound)
				{
					Debug.LogWarning(string.Format("Can't find animation \"{0}\".", aAnimationName));
				}
			}
		}

		public void Play()
		{
			_isPlaying = true;
			_delay = 0.0f;
		}

		public void Stop()
		{
			_isPlaying = false;
		}

		public void GotoAndStop(int aFrame)
		{
			_currentFrame = (float)aFrame - 1.0f;
			_currentFrame = (_currentFrame < 0.0f) ? 0.0f : (_currentFrame >= TotalFrames) ? (float)(TotalFrames - 1) : _currentFrame;
			SetFrame(_currentFrame);
			Stop();
		}

		public void GotoAndPlay(int aFrame)
		{
			_currentFrame = (float)aFrame - 1.0f;
			_currentFrame = (_currentFrame < 0.0f) ? 0.0f : (_currentFrame >= TotalFrames) ? (float)(TotalFrames - 1) : _currentFrame;
			SetFrame(_currentFrame);
			Play();
		}

        public int RandomRangeInt(int aLower, int aUpper)
        {
            return Mathf.RoundToInt(UnityEngine.Random.value * (aUpper - aLower)) + aLower;
        }

        public void PlayRandomFrame()
		{
			GotoAndPlay(RandomRangeInt(1, TotalFrames));
		}

		public void NextFrame()
		{
			_currentFrame += (_animationSpeed * Time.deltaTime * Time.timeScale) * timeScale;
			SetFrame(_currentFrame);
		}

		public void PrevFrame()
		{
			_currentFrame -= (_animationSpeed * Time.deltaTime * Time.timeScale) * timeScale;
			SetFrame(_currentFrame);
		}

		private void AnimationComplete()
		{
			if (loop && loopDelay > 0.0f)
			{
				_delay = loopDelay;
				_isPlaying = false;
				_isPaused = true;
			}

			_complete = 1;
			if (EventAnimationComplete != null)
			{
				EventAnimationComplete(this, _currentAnimation.name);
			}
		}

		private void SetFrame(float aFrame)
		{
			if (_currentAnimation.frames != null)
			{
				int i = RoundFrame(aFrame);
				if (_prevFrame != i)
				{
					_sprite.sprite = _currentAnimation.frames[i];
					_prevFrame = i;
				}
			}
		}

		private int RoundFrame(float aFrame)
		{
			int i = Mathf.RoundToInt(aFrame);
			return (i <= 0) ? 0 : (i >= TotalFrames - 1) ? TotalFrames - 1 : i;
		}

		public bool IsPlaying
		{
			get { return _isPlaying; }
		}

		public bool JustFinished
		{
			get { return (_complete == 1); }
		}

		public string AnimationName
		{
			get { return _currentAnimation.name; }
			set { SwitchAnimation(value); }
		}

		public int TotalFrames
		{
			get { return (_currentAnimation.frames != null) ? _currentAnimation.frames.Length : 0; }
		}

		public int CurrentFrame
		{
			get { return RoundFrame(_currentFrame); }
		}
	}
}