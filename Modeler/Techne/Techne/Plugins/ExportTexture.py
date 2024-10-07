import clr
clr.AddReference("PresentationFramework")
clr.AddReference("PresentationCore")
clr.AddReference("Techne.Plugins.Interfaces")
clr.AddReference("mscorlib")
#import System.Windows.Media.Imaging as Imaging
import System
import System.Windows as Windows
import Techne.Plugins
import Techne.Plugins.Interfaces as Interfaces
import System.Windows.Media.Media3D
import System.Windows.Media.Media3D.Vector3D as Vector3D
import math

class Plugin(Interfaces.IExportPlugin, Interfaces.IPythonPlugin):
    
    def getIcon(self):
        return None
    def setIcon(self, value):
        return
            
    def getMenuHeader(self):
        return "Texture"
    def setMenuHeader(self, value):
        return "Texture"
    
    def getGuid(self):
        return System.Guid("9A7B24BE-0A1C-4F42-AE28-CE1B75A19AAA")
    def setGuid(self, value):
        return
    
    def getPluginType(self):
        return Techne.Plugins.PluginType.Tool
    def setPluginType(self, value):
        return
		
    def getName(self):
        return "Textureexporter"

    def getVersion(self):
        return "1"
		
    def getAuthor(self):
        return "ZeuX"
		
    def getDescription(self):
        return ""
    
    def getDefaultExtension(self):
        return "png"
    
    def getFilter(self):
        return "png"
    
    Icon = property(fget=lambda self: self.getIcon(), fset=lambda self, v: self.setIcon(v))
    Guid = property(fget=lambda self: self.getGuid(), fset=lambda self, v: self.setGuid(v))
    MenuHeader = property(fget=lambda self: self.getMenuHeader(), fset=lambda self, v: self.setMenuHeader(v))
    PluginType = property(fget=lambda self: self.getPluginType(), fset=lambda self, v: self.setPluginType(v))
    Author = property(getAuthor, None)
    Version = property(getVersion, None)
    Name = property(getName, None)
    Filter = property(getFilter, None)
    DefaultExtension = property(getDefaultExtension, None)
    Description = property(getDescription, None)

    def Export(self, filename, visuals, shapes, saveModel):
        
        return
    
    def OnLoad(self):
        return

Plugin()

#import clr
#clr.AddReference("PresentationFramework")
#import System.Windows as Windows
#
#Windows.MessageBox.Show("test", "hi")