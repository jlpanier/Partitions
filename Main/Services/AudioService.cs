using Plugin.Maui.Audio;

public class AudioService: IAudioService
{
    private readonly IAudioManager _audioManager;
    private IAudioPlayer? _player;

    public AudioService(IAudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    public async Task PlayAsync(string fileName)
    {
        var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
        _player = _audioManager.CreatePlayer(stream);
        _player.Play();
    }

    public void Stop() => _player?.Stop();
}

