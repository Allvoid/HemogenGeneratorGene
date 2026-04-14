using RimWorld;
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

            if (!Active || pawn?.Dead != false) return;

            var hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>();
            if (hemogen == null || hemogen.Value >= hemogen.Max) return;

            float perTick = HemogenPerDay / TicksPerDay;
            hemogenBuffer += perTick;

            if (hemogenBuffer >= 0.001f)
            {
                float toAdd = UnityEngine.Mathf.Min(
                    hemogenBuffer,
                    hemogen.Max - hemogen.Value
                );

                hemogen.Value += toAdd;
                hemogenBuffer -= toAdd;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref hemogenBuffer, "hemogenBuffer", 0f);
        }
    }
}