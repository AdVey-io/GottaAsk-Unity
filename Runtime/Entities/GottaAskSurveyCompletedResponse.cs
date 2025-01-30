using System;
using UnityEngine;

namespace GottaAsk
{
#nullable enable
    /// <summary>
    /// Represents the response from the GottaAsk SDK when a survey is completed.
    ///
    /// <para>It contains the rewards or an error message if the survey was not completed successfully.</para>
    /// </summary>
    [Serializable]
    public class GottaAskSurveyCompletedResponse
    {
        [SerializeField]
        public Reward? reward;

        [SerializeField]
        public string? error;

        public override string ToString()
        {
            return $"Reward: {reward?.ToString()}, Error: {error}";
        }
    }

    /// <summary>
    /// Represents the reward data from the GottaAsk SDK when a survey is completed.
    /// </summary>
    [Serializable]
    public class Reward
    {
        [SerializeField]
        public int amount;

        [SerializeField]
        public int mulitplier;

        public override string ToString()
        {
            return $"Amount: {amount}, Multiplier: {mulitplier}";
        }
    }
}
