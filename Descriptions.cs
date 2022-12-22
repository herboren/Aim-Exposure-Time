using System.Collections.Generic;

namespace aimless_et
{
    class Descriptions
    {
        public List<(string, string)> Tooltips;

        public Descriptions()
        {
            Tooltips = new List<(string, string)>
            {
                ("Rules","The rules, 200, 300, and 500 are a measurement used to determine the maximum exposure time that can be used in a photograph before star trails appear or before the stars become blurry."),
                ("Sensor","Determine the Crop-Factor of images."),
                ("F/Stop","The aperture controls the amount of light that enters the camera lens, measured in f-stops"),
                ("Pixel Pitch","Pixel pitch is the distance between two adjacent pixels in the sensor, measured in microns, pixel center to pixel center.\n\nUse the database on the LEFT to identify your camera model. Right+Click > Add Pitch."),
                ("Focal Length","The distance (measured in millimeters) between the point of convergence of your lens and the sensor or film recording the image"),
                ("Declination","Declination is the angular distance in degrees measured from the celestial equator along the meridian through the star.")
            };
        }

    }
}