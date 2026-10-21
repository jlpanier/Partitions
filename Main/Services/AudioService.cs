using Plugin.Maui.Audio;

/// <summary>
/// Service audio
/// </summary>
public class AudioService: IAudioService
{
    private readonly IAudioManager _audioManager;

#if !ANDROID
    private IAudioPlayer? _player;
#endif

    /// <summary>
    /// Initialisation
    /// </summary>
    public AudioService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    /// <summary>
    /// Choisi l'application pour jouer le fichier mp3
    /// </summary>
    public async Task PlayAsync(string fileName)
    {
#if ANDROID
        AudioPlayerService.Play(fileName);
#else
        using var stream = File.OpenRead(fileName);
        {
            _player = _audioManager.CreatePlayer(stream);
            _player.Speed = 2.0f;
            _player.Play();
        }
#endif
    }

    public void Stop()
    {
#if !ANDROID
        _player?.Stop();
#endif
    }
}

