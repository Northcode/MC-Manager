#TO USE THIS INSTALL IRONPYTHON AND VISUAL STUDIO PYTHON TOOLS
#YOU NEED VS FULL EDITION!

#use: from PyMacros import *
#to use in python interactive in VS

import os

def setver(ver):
    """Sets the version of MCManager both in version.cs and ver.txt"""
    cfs = open("version.cs","w")
    cfs.write("namespace MCManager\n{\nclass v\n{\npublic const string G_VERSION = \"" + ver + "\";\n}\n}")
    cfs.close()
    tfs = open("ver.txt","w")
    tfs.write(ver)
    tfs.close()

def addChangeLog(text):
    """Adds some text to the changelog"""
    if(not os.path.exists("changelog.cs")):
        print("changelog not found")
        fs = open("changelog.cs","w")
        fs.write(
"""
namespace MCManager
{
    class changelog
    {
        public static string[] loglines = new string[] {""" + text + """};
    }
}
""")
        fs.close()
    else:
        print("changelog found!")
        rfs = open("changelog.cs","r")
        lines = rfs.readlines()
        rfs.close()
        for line in lines:
            pline = line.strip()
            if(pline.startswith("public static string")):
                newdata = pline[49:-2] + " , \"" + text + "\""
                wfs = open("changelog.cs","w")
                wfs.write("""
namespace MCManager
{
    class changelog
    {
        public static string[] loglines = new string[] {""" + newdata + """};
    }
}
""")
                wfs.close()