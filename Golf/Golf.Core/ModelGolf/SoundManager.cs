using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    /// <summary>
    /// The class defining the management of the sound
    /// </summary>
    class SoundManager
    {
        /// <summary>
        /// The sound for a hard hit
        /// </summary>
        private readonly List<SoundEffect> _hitHard;
        /// <summary>
        /// The sound for a medium hit
        /// </summary>
        private readonly List<SoundEffect> _hitMedium;
        /// <summary>
        /// The sound for a short hit
        /// </summary>
        private readonly List<SoundEffect> _hitShort;
        /// <summary>
        /// The sound for a hard collision
        /// </summary>
        private readonly List<SoundEffect> _colHard;
        /// <summary>
        /// The sound for a medium collision
        /// </summary>
        private readonly List<SoundEffect> _colMedium;
        /// <summary>
        /// The sound for a short collision
        /// </summary>
        private readonly List<SoundEffect> _colShort;
        /// <summary>
        /// The sound for the rolling of the ball
        /// </summary>
        private readonly List<SoundEffectInstance> _rolls;
        /// <summary>
        /// The last roll sound
        /// </summary>
        private SoundEffectInstance _lastRoll;
        /// <summary>
        /// The clap sound
        /// </summary>
        private readonly SoundEffectInstance _clap;
        /// <summary>
        /// The success sound
        /// </summary>
        private readonly SoundEffectInstance _success;
        /// <summary>
        /// The reset sound
        /// </summary>
        private readonly SoundEffectInstance _reset;
        /// <summary>
        /// The ambient sound
        /// </summary>
        private readonly Song _ambient;
        /// <summary>
        /// The randomize class
        /// </summary>
        private readonly Random _random;
        /// <summary>
        ///Constructor of the sound manager
        /// </summary>
        /// <param name="game">the current game</param>
        public SoundManager(MiniGolf game)
        {
            _random = new Random();
            
            _hitHard = new List<SoundEffect>();
            _hitMedium = new List<SoundEffect>();
            _hitShort = new List<SoundEffect>();
            _colHard = new List<SoundEffect>();
            _colMedium = new List<SoundEffect>();
            _colShort = new List<SoundEffect>();
            _rolls = new List<SoundEffectInstance>();

            _hitMedium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-1"));
            _hitMedium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-2"));
            _hitMedium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-3"));
            _hitHard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-1"));
            _hitHard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-2"));
            _hitHard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-3"));
            _hitShort.Add(game.Content.Load<SoundEffect>("sound/hit-short-1"));
            _hitShort.Add(game.Content.Load<SoundEffect>("sound/hit-short-2"));
            _hitShort.Add(game.Content.Load<SoundEffect>("sound/hit-short-3"));

            _colHard.Add(game.Content.Load<SoundEffect>("sound/col-hard-1"));
            _colHard.Add(game.Content.Load<SoundEffect>("sound/col-hard-2"));
            _colMedium.Add(game.Content.Load<SoundEffect>("sound/col-medium-1"));
            _colShort.Add(game.Content.Load<SoundEffect>("sound/col-short-1"));
            _rolls.Add(game.Content.Load<SoundEffect>("sound/roll-1").CreateInstance());
            _rolls.Add(game.Content.Load<SoundEffect>("sound/roll-2").CreateInstance());
            _rolls.Add(game.Content.Load<SoundEffect>("sound/roll-3").CreateInstance());

            _success = game.Content.Load<SoundEffect>("sound/success").CreateInstance();
            _clap = game.Content.Load<SoundEffect>("sound/clap").CreateInstance();
            _reset = game.Content.Load<SoundEffect>("sound/out").CreateInstance();

            _ambient = game.Content.Load<Song>("sound/ambiant");
            MediaPlayer.IsRepeating = true;
            

            _rolls[2].Volume = 0.5f;
            _lastRoll = _rolls[0];
        }

        /// <summary>
        /// Method defining the sound on a hit
        /// </summary>
        /// <param name="chargeBar">the current charge bar</param>
        public void Hit(ChargeBar chargeBar)
        {
            int ratio = (int)((chargeBar.Charge * 400) / chargeBar.ChargeMax);
            if (ratio < (400 / 3) * 2)
            {
                if(ratio < 400 / 3)
                {
                    _hitShort[_random.Next(_hitShort.Count)].Play();
                }
                else
                {
                    _hitMedium[_random.Next(_hitMedium.Count)].Play();
                }
            }
            else
            {
                _hitHard[_random.Next(_hitHard.Count)].Play();
            }
            
            
        }

        /// <summary>
        /// Method defining the sound of an impact
        /// </summary>
        /// <param name="entity">the entity concerned by the impact</param>
        public void Impact(Entity entity)
        {
            float velocity = entity.LinearVelocity.Length();
            if (velocity < (200 / 3) * 2)
            {
                if (velocity < 200 / 3)
                {
                    _colShort[_random.Next(_colShort.Count)].Play();
                }
                else
                {
                    _colMedium[_random.Next(_colMedium.Count)].Play();
                }
            }
            else
            {
                _colHard[_random.Next(_colHard.Count)].Play();
            }
        }

        /// <summary>
        /// Method defining the rolling sound
        /// </summary>
        /// <param name="entity">The entity rolling</param>
        public void Roll(Entity entity)
        {
            if (SoundState.Stopped == _lastRoll.State)
            {
                float velocity = entity.LinearVelocity.Length();
                if (velocity < (200 / 3) * 2)
                {
                    if (velocity < 200 / 3)
                    {
                        _lastRoll = _rolls[0];
                    }
                    else
                    {
                        _lastRoll = _rolls[2];
                    }
                }
                else
                {
                    _lastRoll = _rolls[1];
                }
                _lastRoll.Play();

            }
            
            
        }

        /// <summary>
        /// Method defining when the rolling stops
        /// </summary>
        public void RollStop()
        {
            _lastRoll.Stop();
        }

        /// <summary>
        /// Method defining the ambient sound
        /// </summary>
        public void PlayAmbiant()
        {
            MediaPlayer.Play(_ambient);
        }

        /// <summary>
        /// Method defining the stop of the ambient sound
        /// </summary>
        public void StopAmbiant()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Method defining the clap sound
        /// </summary>
        public void Clap()
        {
            if (SoundState.Stopped == _clap.State)
            {
                _clap.Play();
            }
        }

        /// <summary>
        /// Method defining the success sound
        /// </summary>
        public void Success()
        {
            if (SoundState.Stopped == _success.State)
            {
                _success.Play();
            }
        }

        /// <summary>
        /// Method defining the out sound
        /// </summary>
        public void Out()
        {
            if (SoundState.Stopped == _reset.State)
            {
                _reset.Play();
            }
        }
    }
}
