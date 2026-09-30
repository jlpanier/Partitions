using Microsoft.Maui.Controls.PlatformConfiguration;
using System.IO;

namespace Main.Services;

public static class ImageRotationService
{
    /// <summary>
    /// Lit l’orientation EXIF (photos Android / iPhone).
    /// </summary>
    private static int GetExifRotation(string imagePath)
    {
#if ANDROID
        var exif = new Android.Media.ExifInterface(imagePath);
        var orientation = exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);

        return orientation switch
        {
            (int)Android.Media.Orientation.Rotate90 => 90,
            (int)Android.Media.Orientation.Rotate180 => 180,
            (int)Android.Media.Orientation.Rotate270 => 270,
            _ => 0
        };
#else
        throw new PlatformNotSupportedException("Rotation native uniquement disponible sur Android.");
#endif
    }

#if ANDROID

    /// <summary>
    /// Applique une rotation native Android via Matrix.
    /// </summary>
    private static Android.Graphics.Bitmap ApplyRotation(Android.Graphics.Bitmap bitmap, int angle)
    {
        var matrix = new Android.Graphics.Matrix();
        matrix.PostRotate(angle);

        Android.Graphics.Bitmap rotated = Android.Graphics.Bitmap.CreateBitmap(
            bitmap,
            0, 0,
            bitmap.Width,
            bitmap.Height,
            matrix,
            true);

        bitmap.Dispose();
        return rotated;
    }
#endif
}