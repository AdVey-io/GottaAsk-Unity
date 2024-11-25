using System;
using UnityEngine;

namespace GottaAsk
{
    [Serializable]
    public enum Gender
    {
        MALE,
        FEMALE,
        NON_BINARY,
        OTHER,
        UNKNOWN,
    }

    [Serializable]
    public class GottaAskDemographicData
    {
        [SerializeField]
        public Gender gender = Gender.UNKNOWN;

        [SerializeField]
        public int age;

        [SerializeField]
        public string country;

        [SerializeField]
        public int income;
    }
}
