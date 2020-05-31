# To understand this script, a description of the full texture workflow follows.
#
# Artists create assets as desired and load the TexturePackTool program, selecting Textures.json under the Content directory.
# Artists reference their assets in sprite sheets based on how often they appear together. This optimizes for texture caching.
# The json file is updated when the generated sprite sheets are placed in the Content directory. This keeps textures neatly
# packed and optimized for loading speed. It allows sprite sheets to be easily recomputed as assets change.
# This script is set to run on pre-build of the project, generating Sprite/Textures.cs. The Textures.cs file creates static,
# strongly-typed definitions of all the textures for each spritesheet. This makes it easy to detect inconsistencies between the
# code and textures, look up the properties of a frame, and understand what the frame is (since it's reference by name).

# This script is intended to run during the pre-build step in Visual Studio.
# The textures JSON path, output path, and JSON shape are assumptions made in the script.

$textureJsonPath = $PSScriptRoot + '/../Content/Textures.json'
$outputPath = $PSScriptRoot + '/../Sprite/Textures.cs'
$newline = "`r`n"
$indent = '    '

$generatedFile = '// This file is auto-generated during the pre-build step. Modifications to this file will be lost.' + $newline
$generatedFile += 'namespace MapDescriptorTest.Sprite' + $newline + '{' + $newline + $indent + 'using Microsoft.Xna.Framework;' + $newline

try
{
    # Imports json serialization from .NET.
    Add-Type -AssemblyName System.Web.Extensions
    $JS = New-Object System.Web.Script.Serialization.JavaScriptSerializer

    # Gets and parses the json into an object.
    $text = Get-Content $textureJsonPath
    $json = $JS.DeserializeObject($text)

    # Iterates through the JSON structure to generate the file.
    for ($i = 0; $i -lt $json.sheets.length; $i++) {
        $name = $json.sheets[$i].name.replace(' ', '_')
        $frames = $json.sheets[$i].frames
        $exportUrl = $json.sheets[$i].'export-url'

        $generatedFile += $indent + "public static class $name" + $newline + $indent + '{' + $newline
  
        for ($j = 0; $j -lt $frames.length; $j++) {
            $frame = $frames[$j]
            $frameName = $frame.name.replace(' ', '_')
            $frameX = $frame.x
            $frameY = $frame.y
            $frameW = $frame.w
            $frameH = $frame.h

            $generatedFile += $indent + $indent + "public static Rectangle $frameName = new Rectangle($frameX, $frameY, $frameW, $frameH);" + $newline
        }
  
        $generatedFile += $indent + '}' + $newline
    }

    $generatedFile += '}'

    # Output the file to the specified location, overwriting any existing file.
    Set-Content -path $outputPath -value $generatedFile
}
catch {
    Exit -1;
}