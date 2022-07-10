# PATCHED DLLS

Any DLLs that need to be overwritten for the Tests project are here. They will copied into ColonistModTests' `$(TargetDir)`.

Update the `CopyOverPatchedLibs` target in the project to add additional overwrites.

* Mohawk.SystemCore.dll - This patches the assert method out.
