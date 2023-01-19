using System;

namespace aimless_et
{
    class ExposureFormulas
    {
        // Camera
        private static float[] Sensor { get; } = { 1.0f, 0.6f, 1.5f, 1.6f, 2.0f, 2.7f };

        // Geometry
        private static double _PI { get; set; } = 3.14159;
        private static int Radian { get; } = 180;
        private static int ArcSeconds { get; } = 206265;

        // Controls
        public float Fstop { get; set; }
        public int FocalLength { get; set; }
        public float Pitch { get; set; }
        public int Declination { get; set; }
        public int RuleInt { get; set; }
        public int Rule { get; set; }
        public float Aperture { get; set; }
        public int SensorInt { get; set; }

        /// Returns floated seconds
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>

        public float PlateExposureTime()
        {
            return (ArcSeconds * (Pitch / 1000) / FocalLength) / 15;
        }

        /// <summary>
        /// ** For stars not on the celestial equator, the exposure time will be divided by
        /// the cosine of the declination to get the same drift amount in pixels:
        /// Formula: 
        /// </summary>
        /// <returns></returns>
        public double PlatePlusExposureTime()
        {
            return ((ArcSeconds * (Pitch / 1000) / FocalLength) / 15) / Math.Cos((Declination * _PI) / Radian);
        }


        /// <summary>
        /// This rule replaces the old "nth" rule. Results may be uncertain.
        /// </summary>
        /// <returns></returns>
        public double AperturePixelFocal()
        {
            return RuleInt * ((16.856 * Aperture) + (0.0997 * FocalLength) + (13.713 * Pitch)) / (FocalLength * Math.Cos(Declination * _PI / Radian));
        }

        /// <summary>
        /// NPF Rule, simplified that does not take pizel density of the camera into account.
        /// </summary>
        /// <returns></returns>
        public double SimplifiedAperturePixelFocal()
        {
            return (35 * Aperture + 30 * Pitch) / FocalLength;
        }


        /// <summary>
        /// Crop factor is the size difference between the film frame and the camera's sensor
        /// Popular formats: 35mm
        /// Returns floated seconds
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public float FourCropExposureTime()
        {            
            return (4 - Sensor[SensorInt]) * 100 / FocalLength;
        }

        /// <summary>
        /// Rule of "nth" for exposure time before stars begin trailing
        /// </summary>
        /// <returns></returns>
        public float RuleExposureTime()
        {
           return Rule / (Sensor[SensorInt] * FocalLength); // Rule / (Sensor * FocalLength);
        }


        /// <summary>
        /// Validation checking for common values used in formulas
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public int IsNumeric(string text)
        {
            if (int.TryParse(text, out int i))
                return i;

            return 0;
        }

        public float IsFloat(string text)
        {
            if (float.TryParse(text, out float fl))
                return fl;

            return 0f;
        }
    }
}
