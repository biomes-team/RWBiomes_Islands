﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiomesCore.DefModExtensions;
using RimWorld;
using Verse;
using Verse.Noise;

namespace BiomesIslands.GenSteps
{
    public class GenStep_IslandCrescent : GenStep
    {
        public override int SeedPart
        {
            get
            {
                return 2115796768;
            }
        }


        public override void Generate(Map map, GenStepParams parms)
        {
            IntVec3 mapCenter = map.Center;

            MapGenFloatGrid fertility = MapGenerator.Fertility;

            float mapSize = map.Size.x;

            IntRange NoiseRange = new IntRange(10, 50);

            //NoiseRange.min = (int)(0.05 * map.Size.x);
            //NoiseRange.max = (int)(0.12 * map.Size.x);
            NoiseRange.min = (int)(0.04 * map.Size.x);
            NoiseRange.max = (int)(0.10 * map.Size.x);

            int x = NoiseRange.RandomInRange;
            int z = NoiseRange.RandomInRange;

            IntVec3 mainCenter = mapCenter;
            //mainCenter.x += x;
            //mainCenter.z += z;

            IntVec3 cutoutCenter = mapCenter;
            if (Rand.Bool)
            {
                x = 0 - x;
            }
            if (Rand.Bool)
            {
                z = 0 - z;
            }
            cutoutCenter.x -= x;
            cutoutCenter.z -= z;


            ModuleBase noise = new Perlin(Rand.Range(0.015f, 0.028f), 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);
            ModuleBase cutoutNoise = new Perlin(Rand.Range(0.015f, 0.028f), 2.0, 0.5, 6, Rand.Range(0, 2147483647), QualityMode.High);

            float sizeAdj = Rand.Range(2.8f, 3.8f);

            foreach (IntVec3 current in map.AllCells)
            {
                float mainDist = (float)Math.Sqrt(Math.Pow(current.x - mainCenter.x, 2) + Math.Pow(current.z - mainCenter.z, 2));
                float cutoutDist = (float)Math.Sqrt(Math.Pow(current.x - cutoutCenter.x, 2) + Math.Pow(current.z - cutoutCenter.z, 2));

                // Main island shape
                float addition = 20 * (1f - (2.7f * mainDist / mapSize)) + 4f * noise.GetValue(current);

                // Adds the cutout
                addition -= Math.Max(0, 20 * (1f - (sizeAdj * cutoutDist / mapSize)));

                // Shallowize the lagoon
                addition = Math.Max(0, addition) + Math.Min(0.2f, Math.Max(0, 0.5f - (mainDist / mapSize)));

                fertility[current] += addition;
            }
        }
    }
}