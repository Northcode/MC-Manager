#TO USE THIS INSTALL IRONPYTHON AND VISUAL STUDIO PYTHON TOOLS
#YOU NEED VS FULL EDITION!

#use: from PyMacros import *
#to use in python interactive in VS

def setver(ver):
    """Sets the version of MCManager both in version.cs and ver.txt"""
    cfs = open("version.cs","w")
    cfs.write("namespace MCManager\n{\nclass v\n{\npublic const string G_VERSION = \"" + ver + "\";\n}\n}")
    cfs.close()
    tfs = open("ver.txt","w")
    tfs.write(ver)
    tfs.close()