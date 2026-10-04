namespace Main.Services
{
    public interface IAudioService
    {
        /// <summary>
        /// Copie des fichiers de l'asset (cf. base de données) à l'initialisation
        /// </summary>
        Task PlayAsync(string fileName);

        void Stop();
    }
}
