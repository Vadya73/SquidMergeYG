using System;
using System.Collections;
using UnityEngine;

namespace Timer
{
    [Serializable]
    public sealed class CountDownTimer : ICountdown, ISerializationCallbackReceiver
    {
        public event Action OnStarted;
        public event Action OnTimeChanged;
        public event Action OnStopped;
        public event Action OnEnded;
        public event Action OnReset;
        
        public bool IsPlaying { get; private set; }
        
        public float Progress
        {
            get => 1 - _remainingTime / _duration;
            set => SetProgress(value);
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }
        
        public float RemainingTime
        {
            get => _remainingTime;
            set => _remainingTime = Mathf.Clamp(value, 0, _duration);
        }
        
        [SerializeField] private float _duration;

        private float _remainingTime;
        private float _multiplying;
        public float Multiplying => _multiplying;

        private Coroutine _coroutine;
        private MonoBehaviour _monoBehaviour;

        public CountDownTimer(MonoBehaviour behaviour, float duration)
        {
            _monoBehaviour = behaviour;
            _duration = duration;
            _remainingTime = duration;
            _multiplying = 1f;
        }

        public void Play()
        {
            if (IsPlaying)
                return;

            IsPlaying = true;
            OnStarted?.Invoke();
            _coroutine = _monoBehaviour.StartCoroutine(TimerRoutine());
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                _monoBehaviour.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (IsPlaying)
            {
                IsPlaying = false;
                OnStopped?.Invoke();
            }
        }

        public void ResetTime()
        {
            _remainingTime = _duration;
            OnReset?.Invoke();
        }

        public void SetMultiplying(float multiplying)
        {
            _multiplying = multiplying;
        }

        public void ResetMultiplying()
        {
            _multiplying = 1;
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        private IEnumerator TimerRoutine()
        {
            while (_remainingTime > 0)
            {
                yield return null;
                _remainingTime -= UnityEngine.Time.deltaTime * _multiplying;
                OnTimeChanged?.Invoke();
            }

            IsPlaying = false;
            OnEnded?.Invoke();
        }

        private void SetProgress(float progress)
        {
            progress = Mathf.Clamp01(progress);
            _remainingTime = _duration * (1 - progress);
            OnTimeChanged?.Invoke();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _remainingTime = _duration;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }
    }
}