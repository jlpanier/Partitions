using Android.Content;

namespace Main.Services
{

    public static class AudioPlayerService
    {
        public static void Play(string filePath)
        {
#if ANDROID
            var context = Android.App.Application.Context;
            
            var file = new Java.IO.File(filePath);

            var uri = FileProvider.GetUriForFile( context, $"{context.PackageName}.fileprovider", file);

            var intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "audio/mpeg");
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            intent.AddFlags(ActivityFlags.NewTask);

            context.StartActivity(intent);
#endif
        }
    }
}
