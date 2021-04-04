using BEPUphysics.Entities;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golf.Core.ModelGolf
{
    class SoundManager
    {
        List<SoundEffect> hit_hard;
        List<SoundEffect> hit_medium;
        List<SoundEffect> hit_short;
        List<SoundEffect> col_hard;
        List<SoundEffect> col_medium;
        List<SoundEffect> col_short;
        List<SoundEffectInstance> rolls;
        SoundEffectInstance lastRoll;
        SoundEffectInstance clap;
        SoundEffectInstance success;
        SoundEffectInstance reset;
        Song ambiant;
        Random random;

        public SoundManager(MiniGolf game)
        {
            random = new Random();
            
            hit_hard = new List<SoundEffect>();
            hit_medium = new List<SoundEffect>();
            hit_short = new List<SoundEffect>();
            col_hard = new List<SoundEffect>();
            col_medium = new List<SoundEffect>();
            col_short = new List<SoundEffect>();
            rolls = new List<SoundEffectInstance>();

            hit_medium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-1"));
            hit_medium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-2"));
            hit_medium.Add(game.Content.Load<SoundEffect>("sound/hit-medium-3"));
            hit_hard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-1"));
            hit_hard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-2"));
            hit_hard.Add(game.Content.Load<SoundEffect>("sound/hit-hard-3"));
            hit_short.Add(game.Content.Load<SoundEffect>("sound/hit-short-1"));
            hit_short.Add(game.Content.Load<SoundEffect>("sound/hit-short-2"));
            hit_short.Add(game.Content.Load<SoundEffect>("sound/hit-short-3"));

            col_hard.Add(game.Content.Load<SoundEffect>("sound/col-hard-1"));
            col_hard.Add(game.Content.Load<SoundEffect>("sound/col-hard-2"));
            col_medium.Add(game.Content.Load<SoundEffect>("sound/col-medium-1"));
            col_short.Add(game.Content.Load<SoundEffect>("sound/col-short-1"));
            rolls.Add(game.Content.Load<SoundEffect>("sound/roll-1").CreateInstance());
            rolls.Add(game.Content.Load<SoundEffect>("sound/roll-2").CreateInstance());
            rolls.Add(game.Content.Load<SoundEffect>("sound/roll-3").CreateInstance());

            success = game.Content.Load<SoundEffect>("sound/success").CreateInstance();
            clap = game.Content.Load<SoundEffect>("sound/clap").CreateInstance();
            reset = game.Content.Load<SoundEffect>("sound/out").CreateInstance();

            ambiant = game.Content.Load<Song>("sound/ambiant");
            MediaPlayer.IsRepeating = true;
            

            rolls[2].Volume = 0.5f;
            lastRoll = rolls[0];
        }

        public void Hit(ChargeBar chargeBar)
        {
            int ratio = (int)((chargeBar.Charge * 400) / chargeBar.CHARGE_MAX);
            if (ratio < (400 / 3) * 2)
            {
                if(ratio < 400 / 3)
                {
                    hit_short[random.Next(hit_short.Count)].Play();
                }
                else
                {
                    hit_medium[random.Next(hit_medium.Count)].Play();
                }
            }
            else
            {
                hit_hard[random.Next(hit_hard.Count)].Play();
            }
            
            
        }

        public void Impact(Entity entity)
        {
            float velocity = entity.LinearVelocity.Length();
            if (velocity < (200 / 3) * 2)
            {
                if (velocity < 200 / 3)
                {
                    col_short[random.Next(col_short.Count)].Play();
                }
                else
                {
                    col_medium[random.Next(col_medium.Count)].Play();
                }
            }
            else
            {
                col_hard[random.Next(col_hard.Count)].Play();
            }
        }

        public void Roll(Entity entity)
        {

            SoundEffectInstance roll;

            if (SoundState.Stopped == lastRoll.State)
            {
                float velocity = entity.LinearVelocity.Length();
                if (velocity < (200 / 3) * 2)
                {
                    if (velocity < 200 / 3)
                    {
                        lastRoll = rolls[0];
                    }
                    else
                    {
                        lastRoll = rolls[2];
                    }
                }
                else
                {
                    lastRoll = rolls[1];
                }
                lastRoll.Play();

            }
            
            
        }

        public void RollStop()
        {
            lastRoll.Stop();
        }

        public void PlayAmbiant()
        {
            MediaPlayer.Play(ambiant);
        }

        public void StopAmbiant()
        {
            MediaPlayer.Stop();
        }

        public void Clap()
        {
            if (SoundState.Stopped == clap.State)
            {
                clap.Play();
            }
        }

        public void Success()
        {
            if (SoundState.Stopped == success.State)
            {
                success.Play();
            }
        }

        public void Out()
        {
            if (SoundState.Stopped == reset.State)
            {
                reset.Play();
            }
        }
    }
}
