# PiskelBuild

Converts .piskel files into a format useable by MonoGame

## Usage

```

PiskelBuild <source directory> <output directory>

```

Converts each .piskel file in the source directory into a .png file in the output directory as a sprite sheet. 
It also generates a MonoGame content build file (.mgcb) as Content.mgcb relative to the output directory that lists all generated sprite sheets to be built
