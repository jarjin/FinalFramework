###### Version 1.3.15
* Add optional bitmap trimming
* Fix preview leaks in the Editor mode
* Add warning notes about outdated assets
* Add log message about successfully converting

###### Version 1.3.14
* Fix 2018.3.2f1 compilation

###### Version 1.3.13
* Fix preview shutdown warning

###### Version 1.3.12
* Upgrade to minimal LTS version
* Fix anchor frame label detector
* More readable conversion warnings

###### Version 1.3.11
* Fix trial version in Unity 2017
* Fix warnings in Unity 2017
* Fix possible mesh leak after scene switch

###### Version 1.3.10
* Fix (conversion error: 'Error: scaleSelection: Argument number 1 is invalid.')
* Fix (Parsing swf error: Failed to read past end of stream)
* Fix rasterization error with vector graphics in buttons

###### Version 1.3.9
* Not save generated meshes in scene
* Fix (At line 908 of file "FTMain.jsfl": ReferenceError: ft is not defined)
* Fix warning on add missing components
* Replace string shader properties to id
* Replace shader "if" instruction to "step"
* Replace mask shader "if + discard" instruction to "clip"

###### Version 1.3.8
* Fix shape groups in tweens problems
* Fix drawing object shape problems
* Fix locked elements problem
* Fix optimizator bitmap trim problems
* Fix Unity 5.6 submesh sorting bug
* Fix convertSelectionToBitmap for big item (over 4000px)
* Fix overlay blending mode
* Add HD, SD script export
* Add shape tween warning
* Add SwfClip bounds functions (currentLocalBounds, currentWorldBounds)
* Remove excess "if" instruction from shaders
* Remove excess animation reimports

###### Version 1.3.7
* Fix multiple import
* Fix single frame optimization

###### Version 1.3.6
* Fix for scale very small vector items
* Big vector item optimization
* More yield instructions and extensions

###### Version 1.3.5
* Fix sprite import problem

###### Version 1.3.4
* Fix CS6 export problem
* Fix unity postprocessor problems

###### Version 1.3.3
* Fix undefined unusedItems in CS6

###### Version 1.3.2
* Fix bug custom scale export with small items optimization

###### Version 1.3.1
* Fix some Unity 5.5 deprecated functions

###### Version 1.3.0
* ETC separated alpha support
* Export animations with custom scale (for retina)
* New small vector scaled items optimization

###### Version 1.2.0
* Add Yield instructions for wait in coroutines(SwfWaitPlayStopped, SwfWaitRewindPlaying, SwfWaitStopPlaying)
* Add unscaled delta time support(for separate animations, groups of animations or for all)
* Fix guide type flash layers
* Some fixes for reconvert asset problem

###### Version 1.1.1
* Add conversion error by shape tween in CS6
* Fix life after death (problem about destroying with big lag by frame event)

###### Version 1.1.0
* Sequence separator is anchor frame label (not common frame label)
* SwfClip access to frame labels (currentLabelCount, GetCurrentFrameLabel)
* SwfClip events (OnChangeClipEvent, OnChangeSequenceEvent, OnChangeCurrentFrameEvent)

###### Version 1.0.0
* Initial asset store version
* Sources to dll
* Palette bitmaps support

###### Version 0.5.0
* Flash optimizer twicks
* Blending modes (except Alpha and Erase)

###### Version 0.4.0
* Clip groups
* Fix move assets bug
* Tint color for clips
* Animation API improvements

###### Version 0.3.0
* Preview for animation assets and clips
* Animation API improvements
* Memory optimizations

###### Version 0.2.0
* Export optimizations
* Export clips with export names

###### Version 0.1.0
* Initial alpha version
