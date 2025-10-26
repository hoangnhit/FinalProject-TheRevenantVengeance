using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public static class PlayerState
    {
        public static bool acquiredSwordSpin = false;
        public static int Level = 1;
        public static float MaxHp = 300f;
        public static float CurrentHp = 300f;
        public static int MaxExp = 10;
        public static float CurrentExp = 0;
        public static float MoveSpeed = 5f;
        public static float NormalDamge = 20;
        public static float Skill1Damge = 20;
        public static float Skill2Damge = 10;


        public static bool acquiredFireball = false;
        public static bool acquiredAura = false;

        public static GameObject savedFireballPrefab = null;
        public static GameObject savedAuraPrefab = null;
    }
}
