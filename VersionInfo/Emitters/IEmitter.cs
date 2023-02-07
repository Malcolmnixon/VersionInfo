using System.Collections.Generic;

namespace VersionInfo.Emitters
{
    public interface IEmitter
    {
        /// <summary>
        /// Begin emitting
        /// </summary>
        /// <param name="options">Emitter options</param>
        void Begin(Options options);

        /// <summary>
        /// Emit an entry
        /// </summary>
        /// <param name="file">Single file</param>
        void Entry(FileInformation file);

        /// <summary>
        /// End emitting
        /// </summary>
        /// <param name="files">All files</param>
        /// <param name="summary">Summary information</param>
        void End(IList<FileInformation> files, FileInformation summary);
    }
}
