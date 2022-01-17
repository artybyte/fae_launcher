using System;
using System.Collections.Generic;

namespace SimpleMinecraftLauncher
{
    public class VersionControl
    {
        public List<Version> versions { get; set; }
        public int checksum = 1;

        public void SetVersions(List<Version> Versions)
        {
            versions = Versions;
            checksum = GetHashCode();
        }

        /// <summary>
        /// Sets checksum of current object contains versions
        /// </summary>
        public void UpdateChecksum()
        {
            checksum = GetHashCode();
        }

        public override string ToString()
        {

            string ret = "";

            foreach (Version V in versions)
            {

                string _str = "";

                _str += "Version name: " + V.mVersionName + "\n";
                _str += "Version description: " + V.mVersionDescription + "\n";
                _str += "Version archive name: " + V.mVersionArchiveName + "\n";
                _str += "Version archive checksum: " + V.mArchiveChecksum + "\n";
                _str += "Version client server IP: " + V.mClientServerIP + "\n";

                ret += _str + "\n";

            }

            return ret;

        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 38; // prime

                result *= 397;
                if (versions != null)
                {
                    result *= versions.Count;
                    foreach (Version v in versions)
                        result += v.GetHashCode() * 11;
                }

                return result;
            }
        }
    }

    public class Version
    {

        public string mVersionName { get; set; }
        public string mVersionDescription { get; set; }
        public string mVersionDownloadURL { get; set; }
        public string mClientServerIP { get; set; }
        public string mVersionArchiveName { get; set; }
        public string mArchiveChecksum { get; set; }
        public bool mValidatedCRC { get; set; }
        public string[] mVersionMods { get; set; }

        private bool mLocalValidated = false;

        internal void Init()
        {
            mVersionName = "Номер версии";
            mVersionDescription = "Описание версии";
            mVersionDownloadURL = "URL версии";
            mClientServerIP = "IP сервера сборки";
            mVersionArchiveName = "Название архива версии";
            mArchiveChecksum = "Хэш архива";
            mValidatedCRC = false;
        }

        internal void SetValidated(bool Validated)
        {
            mLocalValidated = Validated;
        }

        internal bool GetValidated()
        {
            return mLocalValidated;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 37; // prime

                result *= 397; // also prime (see note)
                if (mVersionName != null)
                    result += mVersionName.GetHashCode();

                result *= 397;
                if (mVersionDescription != null)
                    result += mVersionDescription.GetHashCode();

                result *= 397;
                if (mVersionDownloadURL != null)
                    result += mVersionDownloadURL.GetHashCode();

                result *= 397;
                if (mClientServerIP != null)
                    result += mClientServerIP.GetHashCode();

                result *= 397;
                if (mVersionArchiveName != null)
                    result += mVersionArchiveName.GetHashCode();

                result *= 397;
                if (mValidatedCRC != null)
                    result += mValidatedCRC.GetHashCode();

                result *= 397;
                if (mArchiveChecksum != null)
                    result += mArchiveChecksum.GetHashCode();

                result *= 397;
                if (mVersionMods != null)
                    result += mVersionMods.GetHashCode();

                return result;
            }
        }
    }
}
