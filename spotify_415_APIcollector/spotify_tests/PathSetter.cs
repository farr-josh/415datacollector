using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotify_tests
{
    public class PathSetter
    {
        string workingDirectory;
        public PathSetter()
        {
            workingDirectory = Environment.CurrentDirectory;
        }

        public string AppendWorkingPath(string path)
        {
            return workingDirectory + path;
        }
    }
}
