using RimWorld;
using UnityEngine;
using Verse;

namespace HemogenGeneratorGene
{
    public class Gene_HemogenGenerator : Gene
    {
        private const float HemogenPerDay = 0.07f;
        private const int TicksPerDay = 60000;

        //накапливаем дробную часть
        private float hemogenBuffer = 0f;

        public override void Tick()
        {
            base.Tick();

            if (!Active || pawn?.Dead != false)
                return;

            var hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>();
            if (hemogen == null)
                return;

            float perTick = HemogenPerDay / TicksPerDay;
            hemogenBuffer += perTick;

            if (hemogenBuffer >= 0.001f)
            {
                float roomLeft = Mathf.Max(0f, hemogen.Max - hemogen.Value);

                if (roomLeft > 0f)
                {
                    float toAdd = Mathf.Min(hemogenBuffer, roomLeft);
                    hemogen.Value += toAdd;
                    hemogenBuffer -= toAdd;
                }


                // Предотвращение сверхнакапливания
                if (hemogen.Value >= hemogen.Max)
                {
                    hemogenBuffer = 0f;
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hemogenBuffer, "hemogenBuffer", 0f);
        }
    }
}