using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleMinecraftLauncher
{

    internal class VersionControl
    {
        List<Version> AvailableVersions;
    }

    internal class Version
    {
        private string mVersionString = "1.15.2"; // for example
        private string mVersionDescription = "Description here...";
        private string mVersionDownloadURL = "";
        private string ArchiveChecksum = "";

        public string VersionName
        {
            get
            {
                return mVersionString;
            }
            set
            {
                mVersionString = value;
            }
        }

        public string VersionDescription
        {
            get
            {
                return mVersionDescription;
            }
            set
            {
                mVersionDescription = value;
            }
        }

        public string VersionURL
        {
            get
            {
                return mVersionDownloadURL;
            }
            set
            {
                mVersionDownloadURL = value;
            }
        }
    }
}
