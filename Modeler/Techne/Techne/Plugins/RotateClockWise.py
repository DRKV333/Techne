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

class Plugin(Interfaces.IToolPlugin, Interfaces.IPythonPlugin):
    
    def getIcon(self):
	#System.Uri("D:/System/Users/Alex/Documents/Visual Studio 2010/Projects/Pokecraft/Modeler/McModeler/Techne/bin/Debug/Plugins/texture.png")
        return None
    def setIcon(self, value):
        return
            
    def getMenuHeader(self):
        return "Test"
    def setMenuHeader(self, value):
        return "Test"
    
    def getGuid(self):
        return System.Guid("9A7B24BE-0A1C-4F42-AE28-CE1B75A19A56")
    def setGuid(self, value):
        return
    
    def getPluginType(self):
        return Techne.Plugins.PluginType.Tool
    def setPluginType(self, value):
        return
		
    def getName(self):
        return "Rotate CW"

    def getVersion(self):
        return "1"
		
    def getAuthor(self):
        return "ZeuX"
		
    def getDescription(self):
        return ""
    
    Icon = property(fget=lambda self: self.getIcon(), fset=lambda self, v: self.setIcon(v))
    Guid = property(fget=lambda self: self.getGuid(), fset=lambda self, v: self.setGuid(v))
    MenuHeader = property(fget=lambda self: self.getMenuHeader(), fset=lambda self, v: self.setMenuHeader(v))
    PluginType = property(fget=lambda self: self.getPluginType(), fset=lambda self, v: self.setPluginType(v))
    Author = property(getAuthor, None)
    Version = property(getVersion, None)
    Name = property(getName, None)
    Description = property(getDescription, None)

    def OnClick(self):
        #mainWindowViewModel.SelectAll()
        #if TechneModel is None:
        #    Windows.MessageBox.Show("test", "hi")
        #    return
        #
        #for visual in TechneModel:
        #    #x&y == 1 -> switch sign
        #    
        #    signPosX = math.copysign(1, visual.Position.X);
        #    signPosZ = math.copysign(1, visual.Position.Z);
        #    
        #    signOffsetX = math.copysign(1, visual.Offset.X);
        #    signOffsetZ = math.copysign(1, visual.Offset.Z);
        #    
        #    if signPosX == signPosZ:
        #        Windows.MessageBox.Show(signPosX.ToString, signPosZ.ToString)
        #        signPosZ = signPosZ * -1
        #    else:
        #        signPosX = signPosX * -1
        #        
        #    if signOffsetX == signOffsetZ:
        #        signOffsetZ = signOffsetZ * -1
        #    else:
        #        signOffsetX = signOffsetX * -1
        #    
        #    visual.Position = Vector3D(math.copysign(visual.Position.Z, signPosZ), visual.Position.Y, math.copysign(visual.Position.X, signPosX))
        #    visual.Offset = Vector3D(math.copysign(visual.Offset.Z, signOffsetZ), visual.Offset.Y, math.copysign(visual.Offset.X, signOffsetX))
        #    
        #    width = visual.Width
        #    visual.Width = visual.Height
        #    visual.Height = width
        return
    
    def OnLoad(self):
        return

Plugin()

#import clr
#clr.AddReference("PresentationFramework")
#import System.Windows as Windows
#
#Windows.MessageBox.Show("test", "hi")